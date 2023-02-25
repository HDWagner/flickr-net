using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using FlickrNet.Exceptions;

namespace FlickrNet.Exceptions
{
    /// <summary>
    /// Error 97: Missing signature exception.
    /// </summary>
    /// <remarks>
    /// The call required signing but no signature was sent.
    /// </remarks>
    [Serializable]
    public class MissingSignatureException : FlickrApiException
    {
        internal MissingSignatureException(string message)
            : base(97, message)
        {
        }

        protected MissingSignatureException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
