using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace FlickrNet.Exceptions
{
    /// <summary>
    /// Error: 105: Service currently unavailable
    /// </summary>
    /// <remarks>
    /// The requested service is temporarily unavailable.
    /// </remarks>
    [Serializable]
    public class ServiceUnavailableException : FlickrApiException
    {
        internal ServiceUnavailableException(string message)
            : base(105, message)
        {
        }

        protected ServiceUnavailableException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
