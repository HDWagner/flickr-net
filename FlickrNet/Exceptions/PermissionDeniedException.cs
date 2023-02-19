using System;
using System.Runtime.Serialization;

namespace FlickrNet.Exceptions
{
    /// <summary>
    /// Error: Permission Denied.
    /// </summary>
    /// <remarks>
    /// The owner of the photo does not want to share the data wih you.
    /// </remarks>
    [Serializable]
    public class PermissionDeniedException : FlickrApiException
    {
        internal PermissionDeniedException(int code, string message)
            : base(code, message)
        {
        }

        protected PermissionDeniedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
