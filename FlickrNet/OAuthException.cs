using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

namespace FlickrNet
{
    /// <summary>
    /// An OAuth error occurred when calling one of the OAuth authentication flow methods.
    /// </summary>
    public class OAuthException : Exception
    {
        /// <summary>
        /// The full response of the exception.
        /// </summary>
        public string? FullResponse { get; }

        /// <summary>
        /// The list of error parameters returned by the OAuth exception.
        /// </summary>
        public Dictionary<string, string>? OAuthErrorPameters { get; }

        /// <summary>
        /// The message for the exception.
        /// </summary>
        public override string Message { get; }


        /// <summary>
        /// Constructor for the OAuthException class.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="innerException"></param>
        public OAuthException(string? response, Exception innerException)
            : base("OAuth Exception", innerException)
        {
            FullResponse = response;

            try
            {
                OAuthErrorPameters = UtilityMethods.StringToDictionary(response);
            }
            catch (Exception)
            {
                throw new FlickrException("Failed to parse OAuth error message: " + FullResponse, innerException);
            }

            Message = "OAuth Exception occurred: " + OAuthErrorPameters["oauth_problem"];
        }

        /// <summary>
        /// Constructor for the OAuthException class.
        /// </summary>
        /// <param name="innerException"></param>
        public OAuthException(Exception innerException)
            : base("OAuth Exception", innerException)
        {
            Message = "OAuth Exception";
            if (innerException is not WebException exception)
            {
                return;
            }

            if (exception.Response is not HttpWebResponse res)
            {
                return;
            }

            using var sr = new StreamReader(res.GetResponseStream());
            var response = sr.ReadToEnd();
            sr.Close();

            FullResponse = response;

            OAuthErrorPameters = UtilityMethods.StringToDictionary(response);
            Message = "OAuth Exception occurred: " + OAuthErrorPameters["oauth_problem"];
        }

    }
}
