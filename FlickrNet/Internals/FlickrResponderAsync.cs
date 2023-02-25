using FlickrNet.Classes;
using FlickrNet.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace FlickrNet
{
    public static partial class FlickrResponder
    {
        /// <summary>
        /// Gets a data response for the given base url and parameters, 
        /// either using OAuth or not depending on which parameters were passed in.
        /// </summary>
        /// <param name="flickr">The current instance of the <see cref="Flickr"/> class.</param>
        /// <param name="baseUrl">The base url to be called.</param>
        /// <param name="parameters">A dictionary of parameters.</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static void GetDataResponseAsync(Flickr flickr, string baseUrl, Dictionary<string, string> parameters, Action<FlickrResult<string>> callback)
        {
            bool oAuth = parameters.ContainsKey("oauth_consumer_key");

            if (oAuth)
            {
                GetDataResponseOAuthAsync(flickr, baseUrl, parameters, callback);
            }
            else
            {
                GetDataResponseNormalAsync(baseUrl, parameters, callback);
            }
        }

        private static void GetDataResponseNormalAsync(string baseUrl, Dictionary<string, string> parameters, Action<FlickrResult<string>> callback)
        {
            var method = "POST";

            var data = new StringBuilder();

            foreach (var k in parameters)
            {
                data.Append(k.Key + "=" + UtilityMethods.EscapeDataString(k.Value) + "&");
            }

            DownloadDataAsync(method, baseUrl, data.ToString(), PostContentType, null, callback);
        }

        private static void GetDataResponseOAuthAsync(Flickr flickr, string baseUrl, Dictionary<string, string> parameters, Action<FlickrResult<string>> callback)
        {
            const string method = "POST";

            // Remove api key if it exists.
            parameters.Remove("api_key");
            parameters.Remove("api_sig");

            // If OAuth Access Token is set then add token and generate signature.
            if (!string.IsNullOrEmpty(flickr.OAuthAccessToken) && !parameters.ContainsKey("oauth_token"))
            {
                parameters.Add("oauth_token", flickr.OAuthAccessToken);
            }
            if (!string.IsNullOrEmpty(flickr.OAuthAccessTokenSecret) && !parameters.ContainsKey("oauth_signature"))
            {
                string sig = flickr.OAuthCalculateSignature(method, baseUrl, parameters, flickr.OAuthAccessTokenSecret);
                parameters.Add("oauth_signature", sig);
            }

            // Calculate post data, content header and auth header
            string data = OAuthCalculatePostData(parameters);
            string authHeader = OAuthCalculateAuthHeader(parameters);

            // Download data.
            try
            {
                DownloadDataAsync(method, baseUrl, data, PostContentType, authHeader, callback);
            }
            catch (WebException ex)
            {
                var response = ex.Response as HttpWebResponse;
                if (response == null)
                {
                    throw;
                }

                if (response.StatusCode != HttpStatusCode.BadRequest && response.StatusCode != HttpStatusCode.Unauthorized)
                {
                    throw;
                }

                using (var responseReader = new StreamReader(response.GetResponseStream()))
                {
                    string responseData = responseReader.ReadToEnd();
                    responseReader.Close();

                    throw new OAuthException(responseData, ex);
                }
            }
        }

        private static void DownloadDataAsync(string method, string baseUrl, string data, string contentType, string? authHeader, Action<FlickrResult<string>> callback)
        {
            var client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            if (!string.IsNullOrEmpty(contentType))
            {
                client.Headers["Content-Type"] = contentType;
            }

            if (!string.IsNullOrEmpty(authHeader))
            {
                client.Headers["Authorization"] = authHeader;
            }

            if (method == "POST")
            {
                client.UploadStringCompleted += delegate (object sender, UploadStringCompletedEventArgs e)
                {
                    client.Dispose();

                    var result = new FlickrResult<string>();
                    if (e.Error != null)
                    {
                        result.Error = e.Error;
                        callback(result);
                        return;
                    }

                    result.Result = e.Result;
                    callback(result);
                };

                if (data == null)
                {
                    var result = new FlickrResult<string>();
                    result.Error = new FlickrException("Invalid data in POST request");
                    callback(result);
                    client.Dispose();
                    return;
                }

                client.UploadStringAsync(new Uri(baseUrl), data);
            }
            else
            {
                client.DownloadStringCompleted += delegate (object sender, DownloadStringCompletedEventArgs e)
                {
                    client.Dispose();

                    var result = new FlickrResult<string>();
                    if (e.Error != null)
                    {
                        result.Error = e.Error;
                        callback(result);
                        return;
                    }

                    result.Result = e.Result;
                    callback(result);
                };

                client.DownloadStringAsync(new Uri(baseUrl));
            }
        }
    }
}
