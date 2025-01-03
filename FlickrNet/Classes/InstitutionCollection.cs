﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using FlickrNet.Internals;

namespace FlickrNet.Classes
{
    /// <summary>
    /// A collection of <see cref="Institution"/> instances.
    /// </summary>
    public sealed class InstitutionCollection : Collection<Institution>, IFlickrParsable
    {
        void IFlickrParsable.Load(System.Xml.XmlReader reader)
        {
            reader.Read();

            while (reader.LocalName == "institution")
            {
                var service = new Institution();
                ((IFlickrParsable)service).Load(reader);
                Add(service);
            }

        }
    }
}
