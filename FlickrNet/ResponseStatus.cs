using System;
using System.Xml.Serialization;

namespace FlickrNet
{
    /// <summary>
    /// The status of the response, either ok or fail.
    /// </summary>
    [Serializable]
    public enum ResponseStatus
    {
        /// <summary>
        /// An unknown status, and the default value if not set.
        /// </summary>
        [XmlEnum("unknown")]
        Unknown,

        /// <summary>
        /// The response returns "ok" on a successful execution of the method.
        /// </summary>
        [XmlEnum("ok")]
        Ok,
        /// <summary>
        /// The response returns "fail" if there is an error, such as invalid API key or login failure.
        /// </summary>
        [XmlEnum("fail")]
        Failed
    }

}
