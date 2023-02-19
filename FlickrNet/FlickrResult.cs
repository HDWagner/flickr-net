using System;
using System.Net;

namespace FlickrNet
{
    /// <summary>
    /// Contains details of the result from Flickr, or the error if an error occurred.
    /// </summary>
    /// <typeparam name="T">The type of the result returned from Flickr.</typeparam>
    public class FlickrResult<T>
    {
        private Exception? error;
        /// <summary>
        /// True if the result returned an error.
        /// </summary>
        public bool HasError { get; set; }

        /// <summary>
        /// If the call was successful then this contains the result.
        /// </summary>
        public T? Result { get; set; }

        /// <summary>
        /// If the call was unsuccessful then this contains the exception.
        /// </summary>
        public Exception? Error
        {
            get
            {
                return error;
            }
            set
            {
                error = value;
                if (value == null)
                {
                    HasError = false;
                    return;
                }
                HasError = true;
                var flickrApiException = value as FlickrApiException;
                if (flickrApiException != null)
                {
                    ErrorCode = flickrApiException.Code;
                    ErrorMessage = flickrApiException.OriginalMessage;
                }
            }
        }

        /// <summary>
        /// If an error was returned by the Flickr API then this will contain the error code.
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// If an error was returned by the Flickr API then this will contain the error message.
        /// </summary>
        public string? ErrorMessage { get; set; }
    }
}
