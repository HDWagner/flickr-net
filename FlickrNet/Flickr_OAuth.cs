﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlickrNet
{
    public partial class Flickr
    {
        /// <summary>
        /// Calculates the signature for an OAuth call.
        /// </summary>
        /// <param name="method">POST or GET method.</param>
        /// <param name="url">The URL the request will be sent to.</param>
        /// <param name="parameters">Parameters to be added to the signature.</param>
        /// <param name="tokenSecret">The token secret (either request or access) for generating the SHA-1 key.</param>
        /// <returns>Base64 encoded SHA-1 hash.</returns>
        public string OAuthCalculateSignature(string method, string url, Dictionary<string, string> parameters, string? tokenSecret)
        {
            string baseString = "";
            string key = ApiSecret + "&" + tokenSecret;
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);

            var sorted = parameters.OrderBy(p => p.Key);
            var sb = new StringBuilder();
            foreach (KeyValuePair<string, string> pair in sorted)
            {
                sb.Append(pair.Key);
                sb.Append('=');
                sb.Append(UtilityMethods.EscapeOAuthString(pair.Value));
                sb.Append('&');
            }

            sb.Remove(sb.Length - 1, 1);

            baseString = method + "&" + UtilityMethods.EscapeOAuthString(url) + "&" + UtilityMethods.EscapeOAuthString(sb.ToString());

            var sha1 = new System.Security.Cryptography.HMACSHA1(keyBytes);

            byte[] hashBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(baseString));

            string hash = Convert.ToBase64String(hashBytes);

            return hash;
        }

        /// <summary>
        /// Returns the authorization URL for OAuth authorization, based off the request token and permissions provided.
        /// </summary>
        /// <param name="requestToken">The request token to include in the authorization url.</param>
        /// <param name="perms">The permissions being requested.</param>
        /// <returns></returns>
        public static string OAuthCalculateAuthorizationUrl(string requestToken, AuthLevel perms)
        {
            return OAuthCalculateAuthorizationUrl(requestToken, perms, false);
        }

        /// <summary>
        /// Returns the authorization URL for OAuth authorization, based off the request token and permissions provided.
        /// </summary>
        /// <param name="requestToken">The request token to include in the authorization url.</param>
        /// <param name="perms">The permissions being requested.</param>
        /// <param name="mobile">Should the url be generated be the mobile one or not.</param>
        /// <returns></returns>
        public static string OAuthCalculateAuthorizationUrl(string requestToken, AuthLevel perms, bool mobile)
        {
            string permsString = (perms == AuthLevel.None) ? "" : "&perms=" + UtilityMethods.AuthLevelToString(perms);

            return "https://" + (mobile ? "m" : "www") + ".flickr.com/services/oauth/authorize?oauth_token=" + requestToken + permsString;
        }

        /// <summary>
        /// Populates the given dictionary with the basic OAuth parameters, oauth_timestamp, oauth_noonce etc.
        /// </summary>
        /// <param name="parameters">Dictionary to be populated with the OAuth parameters.</param>
        private void OAuthGetBasicParameters(Dictionary<string, string> parameters)
        {
            var oAuthParameters = OAuthGetBasicParameters();
            foreach (var k in oAuthParameters)
            {
                parameters.Add(k.Key, k.Value);
            }
        }

        /// <summary>
        /// Returns a new dictionary containing the basic OAuth parameters.
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> OAuthGetBasicParameters()
        {
            string oauthtimestamp = UtilityMethods.DateToUnixTimestamp(DateTime.UtcNow);
            string oauthnonce = Guid.NewGuid().ToString("N");

            var parameters = new Dictionary<string, string>();
            parameters.Add("oauth_nonce", oauthnonce);
            parameters.Add("oauth_timestamp", oauthtimestamp);
            parameters.Add("oauth_version", "1.0");
            parameters.Add("oauth_signature_method", "HMAC-SHA1");
            parameters.Add("oauth_consumer_key", ApiKey);
            return parameters;
        }
    }
}
