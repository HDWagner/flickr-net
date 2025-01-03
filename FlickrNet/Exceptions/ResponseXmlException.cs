using System;
using System.Runtime.Serialization;

namespace FlickrNet.Exceptions
{
    /// <summary>
    /// Exception thrown when an error parsing the returned XML.
    /// </summary>
    [Serializable]
    public class ResponseXmlException : FlickrException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseXmlException"/> class.
        /// </summary>
        public ResponseXmlException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseXmlException"/> class with a specified error message.
        /// </summary>
        /// <param name="message"></param>
        public ResponseXmlException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseXmlException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public ResponseXmlException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ResponseXmlException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

    }
}
