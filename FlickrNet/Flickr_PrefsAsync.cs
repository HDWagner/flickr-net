using System;
using System.Collections.Generic;
using FlickrNet.Classes;

namespace FlickrNet
{
    public partial class Flickr
    {
        /// <summary>
        /// Gets the currently authenticated users default content type.
        /// </summary>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public void PrefsGetContentTypeAsync(Action<FlickrResult<ContentType>> callback)
        {
            CheckRequiresAuthentication();

            var parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.prefs.getContentType");

            GetResponseAsync<UnknownResponse>(
                parameters,
                r =>
                {
                    var result = new FlickrResult<ContentType>();
                    result.Error = r.Error;
                    if (!r.HasError)
                    {
                        var value = r.Result?.GetAttributeValue("*", "content_type");
                        if (value != null)
                        {
                            result.Result = (ContentType)int.Parse(value, System.Globalization.NumberFormatInfo.InvariantInfo);
                        }
                    }
                    callback(result);
                });
        }

        /// <summary>
        /// Returns the default privacy level for geographic information attached to the user's photos and whether 
        /// or not the user has chosen to use geo-related EXIF information to automatically geotag their photos.
        /// </summary>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public void PrefsGetGeoPermsAsync(Action<FlickrResult<UserGeoPermissions>> callback)
        {
            CheckRequiresAuthentication();

            var parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.prefs.getGeoPerms");

            GetResponseAsync(parameters, callback);
        }

        /// <summary>
        /// Gets the currently authenticated users default hidden from search setting.
        /// </summary>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public void PrefsGetHiddenAsync(Action<FlickrResult<HiddenFromSearch>> callback)
        {
            CheckRequiresAuthentication();

            var parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.prefs.getHidden");

            GetResponseAsync<UnknownResponse>(
                parameters,
                r =>
                {
                    var result = new FlickrResult<HiddenFromSearch>();
                    result.Error = r.Error;
                    if (!r.HasError)
                    {
                        var value = r.Result?.GetAttributeValue("*", "hidden");
                        if (value != null)
                        {
                            result.Result = (HiddenFromSearch)int.Parse(value, System.Globalization.NumberFormatInfo.InvariantInfo);
                        }
                    }
                    callback(result);
                });
        }

        /// <summary>
        /// Returns the default privacy level preference for the user. 
        /// </summary>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public void PrefsGetPrivacyAsync(Action<FlickrResult<PrivacyFilter>> callback)
        {
            CheckRequiresAuthentication();

            var parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.prefs.getPrivacy");

            GetResponseAsync<UnknownResponse>(
                parameters,
                r =>
                {
                    var result = new FlickrResult<PrivacyFilter>();
                    result.Error = r.Error;
                    if (!r.HasError)
                    {
                        var value = r.Result?.GetAttributeValue("*", "privacy");
                        if (value != null)
                        {
                            result.Result = (PrivacyFilter)int.Parse(value, System.Globalization.NumberFormatInfo.InvariantInfo);
                        }
                    }
                    callback(result);
                });

        }

        /// <summary>
        /// Gets the currently authenticated users default safety level.
        /// </summary>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public void PrefsGetSafetyLevelAsync(Action<FlickrResult<SafetyLevel>> callback)
        {
            CheckRequiresAuthentication();

            var parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.prefs.getSafetyLevel");

            GetResponseAsync<UnknownResponse>(
                parameters,
                r =>
                {
                    var result = new FlickrResult<SafetyLevel>();
                    result.Error = r.Error;
                    if (!r.HasError)
                    {
                        var value = r.Result?.GetAttributeValue("*", "safety_level");
                        if (value != null)
                        {
                            result.Result = (SafetyLevel)int.Parse(value, System.Globalization.NumberFormatInfo.InvariantInfo);
                        }
                    }
                    callback(result);
                });
        }
    }
}
