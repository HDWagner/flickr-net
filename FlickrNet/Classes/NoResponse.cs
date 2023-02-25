using System;
using System.Collections.Generic;
using System.Text;
using FlickrNet.Internals;

namespace FlickrNet.Classes
{
    /// <summary>
    /// Used by the FlickrNet library when Flickr does not return anything in the body of a response, e.g. for update methods.
    /// </summary>
    public sealed class NoResponse : IFlickrParsable
    {
        void IFlickrParsable.Load(System.Xml.XmlReader reader)
        {
            // no body, no parsing.
        }
    }
}
