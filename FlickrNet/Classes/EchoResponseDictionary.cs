using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using FlickrNet;
using FlickrNet.Internals;

namespace FlickrNet.Classes
{
    /// <summary>
    /// The response returned by the <see cref="Flickr.TestEcho"/> method.
    /// </summary>
    [Serializable]
    public sealed class EchoResponseDictionary : Dictionary<string, string>, IFlickrParsable
    {
        public EchoResponseDictionary()
            : base()
        {
        }
        private EchoResponseDictionary(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        void IFlickrParsable.Load(System.Xml.XmlReader reader)
        {
            while (reader.NodeType != System.Xml.XmlNodeType.None && reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                Add(reader.Name, reader.ReadElementContentAsString());
            }
        }
    }
}
