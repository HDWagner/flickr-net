using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace FlickrNet.Exceptions
{
    /// <summary>
    /// Error: 98: Login failed / Invalid auth token
    /// </summary>
    /// <remarks>
    /// The login details or auth token passed were invalid.
    /// </remarks>
    [Serializable]
    public class LoginFailedInvalidTokenException : FlickrApiException
    {
        internal LoginFailedInvalidTokenException(string message)
            : base(98, message)
        {
        }

        protected LoginFailedInvalidTokenException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
