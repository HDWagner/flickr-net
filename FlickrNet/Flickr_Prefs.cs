﻿using FlickrNet.Classes;
using FlickrNet.Exceptions;
using System.Collections.Generic;

namespace FlickrNet
{
    public partial class Flickr
    {
        /// <summary>
        /// Gets the currently authenticated users default content type.
        /// </summary>
        /// <returns></returns>
        public ContentType PrefsGetContentType()
        {
            CheckRequiresAuthentication();

            var parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.prefs.getContentType");

            UnknownResponse response = GetResponseCache<UnknownResponse>(parameters);

            var nav = response.GetXmlDocument().SelectSingleNode("*/@content_type");
            if (nav?.Value == null)
            {
                throw new ParsingException("Unable to find content type preference in returned XML.");
            }

            return (ContentType)int.Parse(nav.Value, System.Globalization.NumberFormatInfo.InvariantInfo);
        }

        /// <summary>
        /// Returns the default privacy level for geographic information attached to the user's photos 
        /// and whether or not the user has chosen to use geo-related EXIF information to automatically geotag their photos.
        /// </summary>
        public UserGeoPermissions PrefsGetGeoPerms()
        {
            CheckRequiresAuthentication();

            var parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.prefs.getGeoPerms");

            return GetResponseCache<UserGeoPermissions>(parameters);
        }

        /// <summary>
        /// Gets the currently authenticated users default hidden from search setting.
        /// </summary>
        /// <returns></returns>
        public HiddenFromSearch PrefsGetHidden()
        {
            CheckRequiresAuthentication();

            var parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.prefs.getHidden");

            UnknownResponse response = GetResponseCache<UnknownResponse>(parameters);

            var nav = response.GetXmlDocument().SelectSingleNode("*/@hidden");
            if (nav?.Value == null)
            {
                throw new ParsingException("Unable to find hidden preference in returned XML.");
            }

            return (HiddenFromSearch)int.Parse(nav.Value, System.Globalization.NumberFormatInfo.InvariantInfo);
        }

        /// <summary>
        /// Returns the default privacy level preference for the user. 
        /// </summary>
        /// <returns></returns>
        public PrivacyFilter PrefsGetPrivacy()
        {
            CheckRequiresAuthentication();

            var parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.prefs.getPrivacy");

            UnknownResponse response = GetResponseCache<UnknownResponse>(parameters);

            var nav = response.GetXmlDocument().SelectSingleNode("*/@privacy");
            if (nav?.Value == null)
            {
                throw new ParsingException("Unable to find safety level in returned XML.");
            }

            return (PrivacyFilter)int.Parse(nav.Value, System.Globalization.NumberFormatInfo.InvariantInfo);
        }

        /// <summary>
        /// Gets the currently authenticated users default safety level.
        /// </summary>
        /// <returns></returns>
        public SafetyLevel PrefsGetSafetyLevel()
        {
            CheckRequiresAuthentication();

            var parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.prefs.getSafetyLevel");

            UnknownResponse response = GetResponseCache<UnknownResponse>(parameters);

            var nav = response.GetXmlDocument().SelectSingleNode("*/@safety_level");
            if (nav?.Value == null)
            {
                throw new ParsingException("Unable to find safety level in returned XML.");
            }

            return (SafetyLevel)int.Parse(nav.Value, System.Globalization.NumberFormatInfo.InvariantInfo);
        }

    }
}
