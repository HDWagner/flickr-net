using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace FlickrNet
{
    /// <summary>
    /// Information returned by the UploadPicture url.
    /// </summary>
    [XmlRoot("rsp")]
    public class UploadResponse
    {

        /// <summary>
        /// The status of the upload, either "ok" or "fail".
        /// </summary>
        [XmlAttribute("stat", Form = XmlSchemaForm.Unqualified)]
        public ResponseStatus Status { get; set; }

        /// <summary>
        /// If the upload succeeded then this contains the id of the photo. Otherwise it will be zero.
        /// </summary>
        [XmlElement("photoid", Form = XmlSchemaForm.Unqualified)]
        public string? PhotoId { get; set; }

        /// <summary>
        /// The ticket id, if using Asynchronous uploading.
        /// </summary>
        [XmlElement("ticketid", Form = XmlSchemaForm.Unqualified)]
        public string? TicketId { get; set; }

        /// <summary>
        /// Contains the error returned if the upload is unsuccessful.
        /// </summary>
        [XmlElement("err", Form = XmlSchemaForm.Unqualified)]
        public ResponseError? Error { get; set; }
    }

}
