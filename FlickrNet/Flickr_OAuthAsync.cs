﻿using FlickrNet.Classes;
using FlickrNet.Exceptions;
using System;
using System.Collections.Generic;

namespace FlickrNet
{
    public partial class Flickr
    {
        /// <summary>
        /// Get an <see cref="OAuthRequestToken"/> for the given callback URL.
        /// </summary>
        /// <remarks>Specify 'oob' as the callback url for no callback to be performed.</remarks>
        /// <param name="callbackUrl">The callback Uri, or 'oob' if no callback is to be performed.</param>
        /// <param name="callback"></param>
        public void OAuthGetRequestTokenAsync(string callbackUrl, Action<FlickrResult<OAuthRequestToken>> callback)
        {
            CheckApiKey();

            string url = "https://www.flickr.com/services/oauth/request_token";

            Dictionary<string, string> parameters = OAuthGetBasicParameters();

            parameters.Add("oauth_callback", callbackUrl);

            string sig = OAuthCalculateSignature("POST", url, parameters, null);

            parameters.Add("oauth_signature", sig);

            FlickrResponder.GetDataResponseAsync(this, url, parameters, (r) =>
            {
                var result = new FlickrResult<OAuthRequestToken>();
                if (r.Error != null)
                {
                    if (r.Error is System.Net.WebException)
                    {
                        var ex = new OAuthException(r.Error);
                        result.Error = ex;
                    }
                    else
                    {
                        result.Error = r.Error;
                    }
                    callback(result);
                    return;
                }
                result.Result = OAuthRequestToken.ParseResponse(r.Result);
                callback(result);
            });
        }

        /// <summary>
        /// Returns an access token for the given request token, secret and authorization verifier.
        /// </summary>
        /// <param name="requestToken"></param>
        /// <param name="verifier"></param>
        /// <param name="callback"></param>
        public void OAuthGetAccessTokenAsync(OAuthRequestToken requestToken, string verifier, Action<FlickrResult<OAuthAccessToken>> callback)
        {
            OAuthGetAccessTokenAsync(requestToken.Token, requestToken.TokenSecret, verifier, callback);
        }

        /// <summary>
        /// For a given request token and verifier string return an access token.
        /// </summary>
        /// <param name="requestToken"></param>
        /// <param name="requestTokenSecret"></param>
        /// <param name="verifier"></param>
        /// <param name="callback"></param>
        public void OAuthGetAccessTokenAsync(string requestToken, string requestTokenSecret, string verifier, Action<FlickrResult<OAuthAccessToken>> callback)
        {
            CheckApiKey();

            string url = "https://www.flickr.com/services/oauth/access_token";

            Dictionary<string, string> parameters = OAuthGetBasicParameters();

            parameters.Add("oauth_verifier", verifier);
            parameters.Add("oauth_token", requestToken);

            string sig = OAuthCalculateSignature("POST", url, parameters, requestTokenSecret);

            parameters.Add("oauth_signature", sig);

            FlickrResponder.GetDataResponseAsync(this, url, parameters, (r) =>
                {
                    var result = new FlickrResult<OAuthAccessToken>();
                    if (r.Error != null)
                    {
                        if (r.Error is System.Net.WebException)
                        {
                            var ex = new OAuthException(r.Error);
                            result.Error = ex;
                        }
                        else
                        {
                            result.Error = r.Error;
                        }

                        callback(result);
                        return;
                    }
                    result.Result = FlickrNet.Classes.OAuthAccessToken.ParseResponse(r.Result);
                    callback(result);
                });
        }


    }
}
