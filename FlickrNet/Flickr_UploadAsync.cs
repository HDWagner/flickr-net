﻿using FlickrNet.Classes;
using FlickrNet.Exceptions;
using FlickrNet.Internals;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;

namespace FlickrNet
{
    public partial class Flickr
    {
        /// <summary>
        /// UploadPicture method that does all the uploading work.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> object containing the pphoto to be uploaded.</param>
        /// <param name="fileName">The filename of the file to upload. Used as the title if title is null.</param>
        /// <param name="title">The title of the photo (optional).</param>
        /// <param name="description">The description of the photograph (optional).</param>
        /// <param name="tags">The tags for the photograph (optional).</param>
        /// <param name="isPublic">false for private, true for public.</param>
        /// <param name="isFamily">true if visible to family.</param>
        /// <param name="isFriend">true if visible to friends only.</param>
        /// <param name="contentType">The content type of the photo, i.e. Photo, Screenshot or Other.</param>
        /// <param name="safetyLevel">The safety level of the photo, i.e. Safe, Moderate or Restricted.</param>
        /// <param name="hiddenFromSearch">Is the photo hidden from public searches.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        [SuppressMessage("Major Code Smell", "S107:Methods should not have too many parameters", Justification = "Public API needs to be unchanged.")]
        public void UploadPictureAsync(Stream stream, string fileName, string title, string description, string tags,
                                       bool isPublic, bool isFamily, bool isFriend, ContentType contentType,
                                       SafetyLevel safetyLevel, HiddenFromSearch hiddenFromSearch,
                                       Action<FlickrResult<string>> callback)
        {
            CheckRequiresAuthentication();

            var uploadUri = new Uri(UploadUrl);

            var parameters = new Dictionary<string, string>();

            if (title != null && title.Length > 0)
            {
                parameters.Add("title", title);
            }
            if (description != null && description.Length > 0)
            {
                parameters.Add("description", description);
            }
            if (tags != null && tags.Length > 0)
            {
                parameters.Add("tags", tags);
            }

            parameters.Add("is_public", isPublic ? "1" : "0");
            parameters.Add("is_friend", isFriend ? "1" : "0");
            parameters.Add("is_family", isFamily ? "1" : "0");

            if (safetyLevel != SafetyLevel.None)
            {
                parameters.Add("safety_level", safetyLevel.ToString("D"));
            }
            if (contentType != ContentType.None)
            {
                parameters.Add("content_type", contentType.ToString("D"));
            }
            if (hiddenFromSearch != HiddenFromSearch.None)
            {
                parameters.Add("hidden", hiddenFromSearch.ToString("D"));
            }

            parameters.Add("api_key", ApiKey);

            if (!string.IsNullOrEmpty(OAuthAccessToken))
            {
                parameters.Remove("api_key");
                OAuthGetBasicParameters(parameters);
                parameters.Add("oauth_token", OAuthAccessToken);
                string sig = OAuthCalculateSignature("POST", uploadUri.AbsoluteUri, parameters, OAuthAccessTokenSecret);
                parameters.Add("oauth_signature", sig);
            }
            else
            {
                parameters.Add("auth_token", apiToken ?? string.Empty);
            }

            UploadDataAsync(stream, fileName, uploadUri, parameters, callback);
        }

        /// <summary>
        /// Replace an existing photo on Flickr.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> object containing the photo to be uploaded.</param>
        /// <param name="fileName">The filename of the file to replace the existing item with.</param>
        /// <param name="photoId">The ID of the photo to replace.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public void ReplacePictureAsync(Stream stream, string fileName, string photoId, Action<FlickrResult<string>> callback)
        {
            var replaceUri = new Uri(ReplaceUrl);

            var parameters = new Dictionary<string, string>();

            parameters.Add("photo_id", photoId);
            parameters.Add("api_key", ApiKey);
            parameters.Add("auth_token", apiToken ?? string.Empty);

            UploadDataAsync(stream, fileName, replaceUri, parameters, callback);
        }

        private void UploadDataAsync(Stream imageStream, string fileName, Uri uploadUri, Dictionary<string, string> parameters, Action<FlickrResult<string>> callback)
        {
            string boundary = "FLICKR_MIME_" + DateTime.Now.ToString("yyyyMMddhhmmss", System.Globalization.DateTimeFormatInfo.InvariantInfo);

            string authHeader = FlickrResponder.OAuthCalculateAuthHeader(parameters);

            var dataBuffer = CreateUploadData(imageStream, fileName, parameters, boundary);

            var req = (HttpWebRequest)WebRequest.Create(uploadUri);
            req.Method = "POST";
            req.ContentType = "multipart/form-data; boundary=" + boundary;
            req.SendChunked = true;
            req.AllowWriteStreamBuffering = false;

            if (!string.IsNullOrEmpty(authHeader))
            {
                req.Headers["Authorization"] = authHeader;
            }

            req.BeginGetRequestStream(
                r =>
                {
                    var result = new FlickrResult<string>();

                    using (var reqStream = req.EndGetRequestStream(r))
                    {
                        try
                        {
                            var bufferSize = 32 * 1024;
                            if (dataBuffer.Length / 100 > bufferSize)
                            {
                                bufferSize = bufferSize * 2;
                            }

                            dataBuffer.UploadProgress += (o, e) =>
                                                         {
                                                             if (OnUploadProgress != null)
                                                             {
                                                                 OnUploadProgress(this, e);
                                                             }
                                                         };
                            dataBuffer.CopyTo(reqStream, bufferSize);
                            reqStream.Close();
                        }
                        catch (Exception ex)
                        {
                            result.Error = ex;
                            callback(result);
                        }
                    }

                    req.BeginGetResponse(
                        r2 =>
                        {

                            try
                            {
                                var res = req.EndGetResponse(r2);
                                var sr = new StreamReader(res.GetResponseStream());
                                var responseXml = sr.ReadToEnd();
                                sr.Close();

                                var t = new UnknownResponse();
                                t.Load(responseXml);
                                result.Result = t.GetElementValue("photoid");
                                result.HasError = false;
                            }
                            catch (Exception ex)
                            {
                                if (ex is WebException)
                                {
                                    var oauthEx = new OAuthException(ex);
                                    result.Error = string.IsNullOrEmpty(oauthEx.Message) ? ex : oauthEx;
                                }
                                else
                                {
                                    result.Error = ex;
                                }
                            }

                            callback(result);

                        },
                        this);
                },
                this);

        }

    }
}
