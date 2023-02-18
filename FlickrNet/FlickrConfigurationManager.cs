using System;
using System.Configuration;
using System.Xml;

#if !(MONOTOUCH || WindowsCE || SILVERLIGHT)
namespace FlickrNet
{
    /// <summary>
    /// Summary description for FlickrConfigurationManager.
    /// </summary>
    internal class FlickrConfigurationManager : IConfigurationSectionHandler
    {
        private static string configSection = "flickrNet";
        private static FlickrConfigurationSettings settings;

        public static FlickrConfigurationSettings Settings
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

        public object Create(object parent, object configContext, XmlNode section)
        {
            configSection = section.Name;
            return new FlickrConfigurationSettings(section);
        }
    }
}
#endif
