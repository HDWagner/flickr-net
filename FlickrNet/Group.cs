using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;
using System;

namespace FlickrNet
{
    /// <summary>
    /// Provides details of a particular group.
    /// </summary>
    /// <remarks>Used by <see cref="Flickr.GroupsBrowse()"/> and
    /// <see cref="Flickr.GroupsBrowse(string)"/>.</remarks>
    public class Group: IFlickrParsable
    {
        /// <summary>
        /// The id of the group.
        /// </summary>
        [XmlAttribute("nsid", Form = XmlSchemaForm.Unqualified)]
        public string? GroupId { get; set; }

        /// <summary>
        /// The name of the group
        /// </summary>
        [XmlAttribute("name", Form = XmlSchemaForm.Unqualified)]
        public string? GroupName { get; set; }

        /// <summary>
        /// The number of memebers of the group.
        /// </summary>
        [XmlAttribute("members", Form = XmlSchemaForm.Unqualified)]
        public int Members { get; set; }

        void IFlickrParsable.Load(XmlReader reader)
        {
            while (reader.MoveToNextAttribute())
            {
                switch (reader.LocalName)
                {
                    case "nsid":
                    case "id":
                        GroupId = reader.Value;
                        break;
                    case "eighteenplus":
                    case "invitation_only":
                        // TODO: define corresponding properties
                        break;
                    default:
                        UtilityMethods.CheckParsingException(reader);
                        break;
                }
            }

            reader.Read();

            while (reader.LocalName != "group")
            {
                switch (reader.LocalName)
                {
                    case "name":
                        GroupName = reader.ReadElementContentAsString();
                        break;
                    case "members":
                        Members = reader.ReadElementContentAsInt();
                        break;
                    case "datecreate":
                    case "dateactivity":
                        // TODO: define corresponding properties for // <datecreate>2005-01-09 09:10:32</datecreate>
                        reader.Skip();
                        break;
                    default:
                        UtilityMethods.CheckParsingException(reader);
                        reader.Skip();
                        break;

                }
            }

            reader.Read();
        }

    }


}
