using System.Xml;
using System.Collections.ObjectModel;

namespace FlickrNet
{
    /// <summary>
    /// List containing <see cref="RawTag"/> items.
    /// </summary>
    public sealed class RawTagCollection : Collection<RawTag>, IFlickrParsable
    {
        void IFlickrParsable.Load(XmlReader reader)
        {
            reader.ReadToDescendant("tag");

            while (reader.LocalName == "tag")
            {
                var member = new RawTag();
                ((IFlickrParsable)member).Load(reader);
                Add(member);
            }

            reader.Skip();
        }
    }
}
