﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using FlickrNet;
using FlickrNet.Internals;

namespace FlickrNet.Classes
{
    /// <summary>
    /// A list of the blog services that Flickr aupports. 
    /// </summary>
    public sealed class BlogServiceCollection : System.Collections.ObjectModel.Collection<BlogService>, IFlickrParsable
    {
        void IFlickrParsable.Load(XmlReader reader)
        {
            if (reader.LocalName != "services")
            {
                UtilityMethods.CheckParsingException(reader);
            }

            reader.Read();

            while (reader.LocalName == "service")
            {
                var service = new BlogService();
                ((IFlickrParsable)service).Load(reader);
                Add(service);
            }

            reader.Skip();

        }
    }
}
