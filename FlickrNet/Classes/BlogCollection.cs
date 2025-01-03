using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using FlickrNet;
using FlickrNet.Internals;

namespace FlickrNet.Classes
{
    /// <summary>
    /// Contains a list of <see cref="Blog"/> items for the user.
    /// </summary>
    public sealed class BlogCollection : Collection<Blog>, IFlickrParsable
    {
        void IFlickrParsable.Load(System.Xml.XmlReader reader)
        {
            if (reader.LocalName != "blogs")
            {
                UtilityMethods.CheckParsingException(reader);
            }

            reader.Read();

            while (reader.LocalName == "blog")
            {
                var b = new Blog();
                ((IFlickrParsable)b).Load(reader);
                Add(b);
            }

            // Skip to next element (if any)
            reader.Skip();
        }
    }
}