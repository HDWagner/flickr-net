using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace FlickrNet
{
    /// <summary>
    /// If an error occurs then Flickr returns this object.
    /// </summary>
    [Serializable]
    public class ResponseError
    {
        /// <summary>
        /// The code or number of the error.
        /// </summary>
        /// <remarks>
        /// 100 - Invalid Api Key.
        /// 99  - User not logged in.
        /// Other codes are specific to a method.
        /// </remarks>
        [XmlAttribute("code", Form = XmlSchemaForm.Unqualified)]
        public int Code { get; set; }

        /// <summary>
        /// The verbose message matching the error code.
        /// </summary>
        [XmlAttribute("msg", Form = XmlSchemaForm.Unqualified)]
        public string? Message { get; set; }

        /// <summary>
        /// The default constructor for a response error.
        /// </summary>
        public ResponseError()
        {
        }

        /// <summary>
        /// Constructor for a response error using the error code and message returned by Flickr.
        /// </summary>
        /// <param name="code">The error code returned by Flickr.</param>
        /// <param name="message">The error message returned by Flickr.</param>
        public ResponseError(int code, string message)
        {
            Code = code;
            Message = message;
        }
    }

}
