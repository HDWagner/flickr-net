﻿using System;
using System.Runtime.Serialization;
using FlickrNet.Exceptions;

namespace FlickrNet.Exceptions
{
    /// <summary>
    /// The specified format (e.g. json) was not found.
    /// </summary>
    /// <remarks>
    /// The FlickrNet library only uses one format, so you should not experience this error.
    /// </remarks>
    [Serializable]
    public sealed class FormatNotFoundException : FlickrApiException
    {
        internal FormatNotFoundException(string message)
            : base(111, message)
        {
        }

        private FormatNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
