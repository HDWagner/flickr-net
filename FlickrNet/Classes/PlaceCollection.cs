﻿using System;
using System.Collections.Generic;
using System.Text;
using FlickrNet.Internals;

namespace FlickrNet.Classes
{
    /// <summary>
    /// A list of <see cref="Place"/> items.
    /// </summary>
    public sealed class PlaceCollection : System.Collections.ObjectModel.Collection<Place>, IFlickrParsable
    {
        void IFlickrParsable.Load(System.Xml.XmlReader reader)
        {
            reader.Read();

            while (reader.LocalName == "place")
            {
                var place = new Place();
                ((IFlickrParsable)place).Load(reader);
                Add(place);
            }

            reader.Skip();
        }
    }
}
