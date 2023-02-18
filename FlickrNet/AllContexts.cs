using System.Xml;
using System.Collections.ObjectModel;

namespace FlickrNet
{
    /// <summary>
    /// All contexts that a photo is in.
    /// </summary>
    public sealed class AllContexts : IFlickrParsable
    {
        /// <summary>
        /// An array of <see cref="ContextSet"/> objects for the current photo.
        /// </summary>
        public Collection<ContextSet> Sets { get; set; }

        /// <summary>
        /// An array of <see cref="ContextGroup"/> objects for the current photo.
        /// </summary>
        public Collection<ContextGroup> Groups { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public AllContexts()
        {
            Sets = new Collection<ContextSet>();
            Groups = new Collection<ContextGroup>();
        }

        void IFlickrParsable.Load(XmlReader reader)
        {
            while (reader.NodeType != XmlNodeType.EndElement)
            {
                switch (reader.LocalName)
                {
                    case "set":
                        var set = new ContextSet();
                        set.PhotosetId = reader.GetAttribute("id");
                        set.Title = reader.GetAttribute("title");
                        Sets.Add(set);
                        reader.Read();
                        break;
                    case "pool":
                        var group = new ContextGroup();
                        group.GroupId = reader.GetAttribute("id");
                        group.Title = reader.GetAttribute("title");
                        Groups.Add(group);
                        reader.Read();
                        break;
                }
            }
        }
    }
}
