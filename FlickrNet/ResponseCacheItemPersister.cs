using System;
using System.IO;
using System.Text;

namespace FlickrNet
{
    internal class ResponseCacheItemPersister : CacheItemPersister
    {
        public override ICacheItem Read(Stream inputStream)
        {
            string s = UtilityMethods.ReadString(inputStream);
            string response = UtilityMethods.ReadString(inputStream);

            string[] chunks = s.Split('\n');

            // Corrupted cache record, so throw IOException which is then handled and returns partial cache.
            if (chunks.Length != 2)
            {
                throw new IOException("Unexpected number of chunks found");
            }

            string url = chunks[0];
            var creationTime = new DateTime(long.Parse(chunks[1], System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo));
            var item = new ResponseCacheItem(new Uri(url), response, creationTime);
            return item;
        }

        public override void Write(Stream outputStream, ICacheItem cacheItem)
        {
            var item = (ResponseCacheItem)cacheItem;
            var result = new StringBuilder();
            result.Append(item.Url.AbsoluteUri + "\n");
            result.Append(item.CreationTime.Ticks.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            UtilityMethods.WriteString(outputStream, result.ToString());
            UtilityMethods.WriteString(outputStream, item.Response);
        }
    }
}
