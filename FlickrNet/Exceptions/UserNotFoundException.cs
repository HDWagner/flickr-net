﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using FlickrNet.Exceptions;

namespace FlickrNet.Exceptions
{
    /// <summary>
    /// No user with the user ID supplied to the method could be found.
    /// </summary>
    /// <remarks>
    /// This could mean the user does not exist, or that you do not have permission to view the user.
    /// </remarks>
    [Serializable]
    public class UserNotFoundException : FlickrApiException
    {
        internal UserNotFoundException(int code, string message)
            : base(code, message)
        {
        }

        protected UserNotFoundException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }
    }
}
