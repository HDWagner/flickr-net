using FlickrNet.Classes;
using FlickrNet.Exceptions;
using System;

namespace FlickrNet.Internals
{
    internal class FlickrResultEventArgs<T> : EventArgs where T : IFlickrParsable
    {
        internal FlickrResultEventArgs(FlickrResult<T> result)
        {
            Error = result.Error;
            HasError = result.HasError;
            Result = result.Result;
        }

        private Exception? error;
        public bool HasError { get; set; }
        public T? Result { get; set; }
        public Exception? Error
        {
            get
            {
                return error;
            }
            set
            {
                error = value;
                var flickrApiException = value as FlickrApiException;

                if (flickrApiException != null)
                {
                    ErrorCode = flickrApiException.Code;
                    ErrorMessage = flickrApiException.OriginalMessage;
                    HasError = true;
                }
            }
        }

        public int ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
