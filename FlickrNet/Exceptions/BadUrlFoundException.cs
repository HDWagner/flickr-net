using System;
using System.Runtime.Serialization;

namespace FlickrNet.Exceptions
{
    /// <summary>
    /// A user was included in a description or comment which Flickr rejected.
    /// </summary>
    [Serializable]
    public sealed class BadUrlFoundException : FlickrApiException
    {
        internal BadUrlFoundException(string message)
            : base(111, message)
        {
        }

        private BadUrlFoundException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }
    }
}
