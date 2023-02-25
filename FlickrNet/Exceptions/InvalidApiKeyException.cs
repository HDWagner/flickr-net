using System;
using System.Runtime.Serialization;
using FlickrNet.Exceptions;

namespace FlickrNet.Exceptions
{
    /// <summary>
    /// Error: 100: Invalid API Key
    /// </summary>
    /// <remarks>
    /// The API key passed was not valid or has expired.
    /// </remarks>
    [Serializable]
    public class InvalidApiKeyException : FlickrApiException
    {
        internal InvalidApiKeyException(string message)
            : base(100, message)
        {
        }

        protected InvalidApiKeyException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
