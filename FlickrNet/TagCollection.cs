using System.Xml;
using System.Collections.ObjectModel;

namespace FlickrNet
{
    /// <summary>
    /// List containing <see cref="Tag"/> items.
    /// </summary>
    public sealed class TagCollection : Collection<Tag>, IFlickrParsable
    {
        void IFlickrParsable.Load(XmlReader reader)
        {
            reader.ReadToDescendant("tag");

            while (reader.LocalName == "tag")
            {
                var member = new Tag();
                ((IFlickrParsable)member).Load(reader);
                Add(member);
            }

            reader.Skip();
        }
    }
}
