using System;
using System.Configuration;

namespace FlickrNet
{
    /// <summary>
    /// Summary description for FlickrConfigurationManager.
    /// </summary>
    internal static class FlickrConfigurationManager 
    {
        private static readonly string configSection = "flickrNet";
        private static FlickrConfigurationSettings? settings;

        public static FlickrConfigurationSettings? Settings
        {
            get
            {
                if (settings == null)
                {
                    try
                    {
                        settings = (FlickrConfigurationSettings)ConfigurationManager.GetSection(configSection);
                    }
                    catch (PlatformNotSupportedException)
                    {
                        // not supported on android
                    }
                    catch (ConfigurationErrorsException)
                    {
                        // not supported on android
                    }
                }

                return settings;
            }
        }

    }
}
