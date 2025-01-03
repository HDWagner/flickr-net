using FlickrNet.Internals;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace FlickrNet.Caching
{
    /// <summary>
    /// A threadsafe cache that is backed by disk storage.
    /// 
    /// All public methods that read or write state must be 
    /// protected by the lockFile.  Private methods should
    /// not acquire the lockFile as it is not reentrant.
    /// </summary>
    public sealed class PersistentCache : IDisposable
    {
        // The in-memory representation of the cache.
        // Use SortedList instead of Hashtable only to maintain backward 
        // compatibility with previous serialization scheme.  If we
        // abandon backward compatibility, we should switch to Hashtable.
        private Dictionary<string, ICacheItem> dataTable = new Dictionary<string, ICacheItem>();

        private readonly CacheItemPersister persister;

        // true if dataTable contains changes vs. on-disk representation
        private bool dirty;

        // The persistent file representation of the cache.
        private readonly FileInfo dataFile;
        private DateTime timestamp;  // last-modified time of dataFile when cache data was last known to be in sync
        private long length;         // length of dataFile when cache data was last known to be in sync

        // The file-based mutex.  Named (dataFile.FullName + ".lock")
        private readonly LockFile lockFile;

        /// <summary>
        /// Initializes a new instance of the <see cref="PersistentCache"/> for the given filename and cache type.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="persister"></param>
        public PersistentCache(string filename, CacheItemPersister persister)
        {
            this.persister = persister;
            dataFile = new FileInfo(filename);
            lockFile = new LockFile(filename + ".lock");
        }

        /// <summary>
        /// Return all items in the cache.  Works similarly to
        /// ArrayList.ToArray(Type).
        /// </summary>
        public ICacheItem[] ToArray(Type valueType)
        {
            using (lockFile.Acquire())
            {
                Refresh();
                InternalGetAll(valueType, out string[] keys, out ICacheItem[] values);
                return values;
            }
        }

        /// <summary>
        /// Gets or sets cache values.
        /// </summary>
        public ICacheItem? this[string key]
        {
            set
            {
                if (key == null)
                {
                    throw new ArgumentNullException(nameof(key));
                }

                ICacheItem? oldItem;

                using (lockFile.Acquire())
                {
                    Refresh();
                    oldItem = InternalSet(key, value);
                    Persist();
                }

                oldItem?.OnItemFlushed();
            }
        }

        /// <summary>
        /// Gets the specified key from the cache unless it has expired.
        /// </summary>
        /// <param name="key">The key to look up in the cache.</param>
        /// <param name="maxAge">The maximum age the item can be before it is no longer returned.</param>
        /// <param name="removeIfExpired">Whether to delete the item if it has expired or not.</param>
        /// <returns></returns>
        public ICacheItem? Get(string key, TimeSpan maxAge, bool removeIfExpired)
        {
            Debug.Assert(maxAge > TimeSpan.Zero || maxAge == TimeSpan.MinValue, "maxAge should be positive, not negative");

            ICacheItem? item;
            bool expired;
            using (lockFile.Acquire())
            {
                Refresh();

                item = InternalGet(key);
                expired = item != null && Expired(item.CreationTime, maxAge);
                if (expired)
                {
                    if (removeIfExpired)
                    {
                        item = RemoveKey(key);
                        Persist();
                    }
                    else
                    {
                        item = null;
                    }
                }
            }

            if (expired && removeIfExpired)
            {
                item?.OnItemFlushed();
            }

            return expired ? null : item;
        }

        /// <summary>
        /// Clears the current cache of items.
        /// </summary>
        public void Flush()
        {
            Shrink(0);
        }

        /// <summary>
        /// Shrinks the current cache to a specific size, removing older items first.
        /// </summary>
        /// <param name="size"></param>
        public void Shrink(long size)
        {
            if (size < 0)
            {
                throw new ArgumentException("Cannot shrink to a negative size", nameof(size));
            }

            var flushed = new List<ICacheItem?>();

            using (lockFile.Acquire())
            {
                Refresh();

                InternalGetAll(typeof(ICacheItem), out string[] keys, out ICacheItem[] values);
                var totalSize = 0L;
                foreach (ICacheItem cacheItem in values)
                {
                    totalSize += cacheItem.FileSize;
                }

                for (int i = 0; i < keys.Length; i++)
                {
                    if (totalSize <= size)
                    {
                        break;
                    }

                    var cacheItem = (ICacheItem?)values.GetValue(i);
                    totalSize -= cacheItem == null ? 0 : cacheItem.FileSize;
                    flushed.Add(RemoveKey(keys[i]));
                }

                Persist();
            }

            foreach (var flushedItem in flushed)
            {
                Debug.Assert(flushedItem != null, "Flushed item was null--programmer error");
                flushedItem?.OnItemFlushed();
            }
        }

        private static bool Expired(DateTime test, TimeSpan age)
        {
            if (age == TimeSpan.MinValue)
            {
                return true;
            }
            if (age == TimeSpan.MaxValue)
            {
                return false;
            }
            return test < DateTime.UtcNow - age;
        }


        private void InternalGetAll(Type valueType, out string[] keys, out ICacheItem[] values)
        {
            if (!typeof(ICacheItem).IsAssignableFrom(valueType))
            {
                throw new ArgumentException("Type " + valueType.FullName + " does not implement ICacheItem", nameof(valueType));
            }

            keys = new List<string>(dataTable.Keys).ToArray();

            var tmpValues = new List<ICacheItem>();
            foreach (var key in keys)
            {
                tmpValues.Add(dataTable[key]);
            }
            values = tmpValues
                .Order(new CreationTimeComparer())
                .ToArray();
        }

        private ICacheItem? InternalGet(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (dataTable.TryGetValue(key, out ICacheItem? value))
            {
                return value;
            }

            return null;
        }

        /// <returns>The old value associated with <c>key</c>, if any.</returns>
        private ICacheItem? InternalSet(string key, ICacheItem? value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            ICacheItem? flushedItem;

            flushedItem = RemoveKey(key);
            if (value != null)
            {
                // don't ever let nulls get in
                dataTable[key] = value;
            }

            dirty = dirty || !ReferenceEquals(flushedItem, value);

            return flushedItem;
        }

        private ICacheItem? RemoveKey(string key)
        {
            if (!dataTable.ContainsKey(key))
            {
                return null;
            }

            var cacheItem = dataTable[key];
            dataTable.Remove(key);
            dirty = true;
            return cacheItem;
        }

        private void Refresh()
        {
            Debug.Assert(!dirty, "Refreshing even though cache is dirty");

            DateTime newTimestamp = DateTime.MinValue;
            long newLength = -1;

            dataFile.Refresh();

            if (dataFile.Exists)
            {
                newTimestamp = dataFile.LastWriteTime;
                newLength = dataFile.Length;
            }

            if (timestamp != newTimestamp || length != newLength)
            {
                // file changed
                if (!dataFile.Exists)
                {
                    dataTable.Clear();
                }
                else
                {
                    Debug.WriteLine("Loading cache from disk");
                    using (FileStream inStream = dataFile.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        dataTable = Load(inStream);
                    }
                }
            }

            timestamp = newTimestamp;
            length = newLength;
            dirty = false;
        }

        private void Persist()
        {
            if (!dirty)
            {
                return;
            }

            Debug.WriteLine("Saving cache to disk");
            using (FileStream outStream = dataFile.Open(FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
            {
                Store(outStream, dataTable);
            }

            dataFile.Refresh();
            timestamp = dataFile.LastWriteTime;
            length = dataFile.Length;

            dirty = false;
        }

        private Dictionary<string, ICacheItem> Load(Stream s)
        {
            var table = new Dictionary<string, ICacheItem>();
            int itemCount = UtilityMethods.ReadInt32(s);
            for (int i = 0; i < itemCount; i++)
            {
                try
                {
                    string key = UtilityMethods.ReadString(s);
                    ICacheItem val = persister.Read(s);
                    if (val == null) // corrupt cache file 
                    {
                        return table;
                    }

                    table[key] = val;
                }
                catch (IOException)
                {
                    return table;
                }
            }
            return table;
        }

        private void Store(Stream s, Dictionary<string, ICacheItem> table)
        {
            UtilityMethods.WriteInt32(s, table.Count);
            foreach (KeyValuePair<string, ICacheItem> entry in table)
            {
                UtilityMethods.WriteString(s, entry.Key);
                persister.Write(s, entry.Value);
            }
        }

        private sealed class CreationTimeComparer : IComparer<ICacheItem>, IComparer
        {
            public int Compare(ICacheItem? x, ICacheItem? y)
            {
                if (x == null && y == null)
                {
                    return 0;
                }
                if (x == null)
                {
                    return -1;
                }
                return x.CreationTime.CompareTo(y?.CreationTime);
            }

            public int Compare(object? x, object? y)
            {
                return Compare((ICacheItem?)x, (ICacheItem?)y);
            }
        }

        void IDisposable.Dispose()
        {
            lockFile?.Dispose();
        }
    }
}
