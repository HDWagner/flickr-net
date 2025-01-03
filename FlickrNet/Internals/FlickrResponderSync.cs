﻿using FlickrNet.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace FlickrNet
{
    internal static partial class FlickrResponder
    {
        /// <summary>
        /// Gets a data response for the given base url and parameters, 
        /// either using OAuth or not depending on which parameters were passed in.
        /// </summary>
        /// <param name="flickr">The current instance of the <see cref="Flickr"/> class.</param>
        /// <param name="baseUrl">The base url to be called.</param>
        /// <param name="parameters">A dictionary of parameters.</param>
        /// <returns></returns>
        public static string GetDataResponse(Flickr flickr, string baseUrl, Dictionary<string, string> parameters)
        {
            bool oAuth = parameters.ContainsKey("oauth_consumer_key");

            if (oAuth)
            {
                return GetDataResponseOAuth(flickr, baseUrl, parameters);
            }
            else
            {
                return GetDataResponseNormal(baseUrl, parameters);
            }
        }

        private static string GetDataResponseNormal(string baseUrl, Dictionary<string, string> parameters)
        {
            string method = "POST";

            var data = new StringBuilder();

            foreach (var k in parameters)
            {
                data.Append(k.Key + "=" + UtilityMethods.EscapeDataString(k.Value) + "&");
            }

            return DownloadData(method, baseUrl, data.ToString(), PostContentType, null);
        }

        private static string GetDataResponseOAuth(Flickr flickr, string baseUrl, Dictionary<string, string> parameters)
        {
            string method = "POST";

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
                return DownloadData(method, baseUrl, data, PostContentType, authHeader);
            }
            catch (WebException ex)
            {
                if (ex.Status != WebExceptionStatus.ProtocolError)
                {
                    throw;
                }

                var response = ex.Response as HttpWebResponse;
                if (response == null)
                {
                    throw;
                }

                string? responseData = null;

                using (var stream = response.GetResponseStream())
                {
                    if (stream != null)
                    {
                        using (var responseReader = new StreamReader(stream))
                        {
                            responseData = responseReader.ReadToEnd();
                            responseReader.Close();
                        }
                    }
                }
                if (response.StatusCode == HttpStatusCode.BadRequest ||
                    response.StatusCode == HttpStatusCode.Unauthorized)
                {

                    throw new OAuthException(responseData, ex);
                }

                if (string.IsNullOrEmpty(responseData))
                {
                    throw;
                }

                throw new WebException("WebException occurred with the following body content: " + responseData, ex, ex.Status, ex.Response);
            }
        }


        private static string DownloadData(string method, string baseUrl, string data, string contentType, string? authHeader)
        {
            Func<string> f = () =>
            {
                using (WebClient client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    if (!string.IsNullOrEmpty(contentType))
                    {
                        client.Headers.Add("Content-Type", contentType);
                    }

                    if (!string.IsNullOrEmpty(authHeader))
                    {
                        client.Headers.Add("Authorization", authHeader);
                    }

                    if (method == "POST")
                    {
                        // we don't care that data might be null: client.UploadString would throw correctly and we'll get rid of WebClient anyway
                        return client.UploadString(baseUrl, data);
                    }
                    return client.DownloadString(baseUrl);
                }
            };

            try
            {
                return f();
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    var response = ex.Response as HttpWebResponse;
                    if (response != null && (response.StatusCode == HttpStatusCode.BadGateway || response.StatusCode == HttpStatusCode.GatewayTimeout))
                    {
                        System.Threading.Thread.Sleep(1000);
                        return f();
                    }
                }
                throw;
            }
        }
    }
}
