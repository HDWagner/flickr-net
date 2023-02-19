using System;
using System.Runtime.Serialization;

namespace FlickrNet.Exceptions
{
    /// <summary>
    /// No photo with the photo ID supplied to the method could be found.
    /// </summary>
    /// <remarks>
    /// This could mean the photo does not exist, or that you do not have permission to view the photo.
    /// </remarks>
    [Serializable]
    public class PhotoNotFoundException : FlickrApiException
    {
        internal PhotoNotFoundException(int code, string message)
            : base(code, message)
        {
        }

        protected PhotoNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
