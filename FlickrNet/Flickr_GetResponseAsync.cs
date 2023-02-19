using System;
using System.Collections.Generic;

namespace FlickrNet
{
    public partial class Flickr
    {
        private void GetResponseAsync<T>(Dictionary<string, string> parameters, Action<FlickrResult<T>> callback) where T : IFlickrParsable, new()
        {
            CheckApiKey();

            parameters["api_key"] = ApiKey;

            // If performing one of the old 'flickr.auth' methods then use old authentication details.
            string method = parameters["method"];

            if (method.StartsWith("flickr.auth", StringComparison.Ordinal) && !method.EndsWith("oauth.checkToken", StringComparison.Ordinal))
            {
                if (!string.IsNullOrEmpty(AuthToken))
                {
                    parameters["auth_token"] = AuthToken;
                }
            }
            else
            {
                // If OAuth Token exists or no authentication required then use new OAuth
                if (!string.IsNullOrEmpty(OAuthAccessToken) || string.IsNullOrEmpty(AuthToken))
                {
                    OAuthGetBasicParameters(parameters);
                    if (!string.IsNullOrEmpty(OAuthAccessToken))
                    {
                        parameters["oauth_token"] = OAuthAccessToken;
                    }
                }
                else
                {
                    parameters["auth_token"] = AuthToken;
                }
            }


            var url = CalculateUri(parameters, !string.IsNullOrEmpty(sharedSecret));

            lastRequest = url;

            try
            {
                FlickrResponder.GetDataResponseAsync(this, BaseUri.AbsoluteUri, parameters, (r)
                    =>
                    {
                        var result = new FlickrResult<T>();
                        if (r.HasError)
                        {
                            result.Error = r.Error;
                        }
                        else
                        {
                            if (r.Result == null)
                            {
                                result.Error = new FlickrException("Empty Flickr response");
                            }
                            else
                            {
                                try
                                {
                                    LastResponse = r.Result;

                                    var t = new T();
                                    t.Load(r.Result);
                                    result.Result = t;
                                    result.HasError = false;
                                }
                                catch (Exception ex)
                                {
                                    result.Error = ex;
                                }
                            }
                        }

                        if (callback != null)
                        {
                            callback(result);
                        }
                    });
            }
            catch (Exception ex)
            {
                var result = new FlickrResult<T>();
                result.Error = ex;
                if (null != callback)
                {
                    callback(result);
                }
            }
        }
    }
}
