using System;
using System.Runtime.Serialization;

namespace FlickrNet.Exceptions
{
    /// <summary>
    /// Error: 96: Invalid signature
    /// </summary>
    /// <remarks>
    /// The passed signature was invalid.
    /// </remarks>
    [Serializable]
    public class InvalidSignatureException : FlickrApiException
    {
        internal InvalidSignatureException(string message)
            : base(96, message)
        {
        }

        protected InvalidSignatureException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
