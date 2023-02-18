using System;
using System.IO;

namespace FlickrNet
{
    /// <summary>
    /// Internal Cache class
    /// </summary>
    public static class Cache
    {
        private static PersistentCache? responses;

        /// <summary>
        /// A static object containing the list of cached responses from Flickr.
        /// </summary>
        public static PersistentCache Responses
        {
            get
            {
                lock (lockObject)
                {
                    responses ??= new PersistentCache(Path.Combine(CacheLocation, "responseCache.dat"), new ResponseCacheItemPersister());

                    return responses;
                }
            }
        }


        private static readonly object lockObject = new object();

        private enum Tristate
        {
            Null, True, False
        }
        private static Tristate cacheDisabled;

        /// <summary>
        /// Returns weither of not the cache is currently disabled.
        /// </summary>
        public static bool CacheDisabled
        {
            get
            {
#if !(WindowsCE || MONOTOUCH || SILVERLIGHT)
                if (cacheDisabled == Tristate.Null && FlickrConfigurationManager.Settings != null)
                {
                    cacheDisabled = FlickrConfigurationManager.Settings.CacheDisabled ? Tristate.True : Tristate.False;
                }
#endif
                if (cacheDisabled == Tristate.Null)
                {
                    cacheDisabled = Tristate.False;
                }

                return cacheDisabled == Tristate.True;
            }
            set
            {
                cacheDisabled = value ? Tristate.True : Tristate.False;
            }
        }

        private static string? cacheLocation;

        /// <summary>
        /// Returns the currently set location for the cache.
        /// </summary>
        public static string CacheLocation
        {
            get
            {
#if !(WindowsCE || MONOTOUCH || SILVERLIGHT)
                if (cacheLocation == null && FlickrConfigurationManager.Settings != null)
                {
                    cacheLocation = FlickrConfigurationManager.Settings.CacheLocation;
                }
#endif
                if (cacheLocation == null)
                {
                    try
                    {
#if !(WindowsCE || MONOTOUCH || SILVERLIGHT)
                        cacheLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FlickrNet");
#endif
#if MONOTOUCH
                        cacheLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "../Library/Caches");
#endif
#if WindowsCE
                        cacheLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "FlickrNetCache");
#endif
#if SILVERLIGHT
                        cacheLocation = string.Empty;
#endif

                    }
                    catch (System.Security.SecurityException)
                    {
                        // Permission to read application directory not provided.
                        throw new CacheException("Unable to read default cache location. Please cacheLocation in configuration file or set manually in code");
                    }
                }

                if (cacheLocation == null)
                {
                    throw new CacheException("Unable to determine cache location. Please set cacheLocation in configuration file or set manually in code");
                }

                return cacheLocation;
            }
            set
            {
                cacheLocation = value;
            }
        }

        internal static long CacheSizeLimit { get; set; } = 52428800;

        /// <summary>
        /// The default timeout for cachable objects within the cache.
        /// </summary>
        public static TimeSpan CacheTimeout { get; set; } = new TimeSpan(0, 1, 0, 0, 0);

        /// <summary>
        /// Remove a specific URL from the cache.
        /// </summary>
        /// <param name="url"></param>
        public static void FlushCache(Uri url)
        {
            Responses[url.AbsoluteUri] = null;
        }

        /// <summary>
        /// Clears all responses from the cache.
        /// </summary>
        public static void FlushCache()
        {
            Responses.Flush();
        }

    }
}
