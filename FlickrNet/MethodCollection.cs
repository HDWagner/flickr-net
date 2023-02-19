using System.Xml;

namespace FlickrNet
{
    /// <summary>
    /// Summary description for Methods.
    /// </summary>
    public sealed class MethodCollection : System.Collections.ObjectModel.Collection<string>, IFlickrParsable
    {
        void IFlickrParsable.Load(XmlReader reader)
        {
            reader.Read();

            while (reader.LocalName == "method")
            {
                Add(reader.ReadElementContentAsString());
            }

            reader.Skip();
        }
    }
}
