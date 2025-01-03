﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using FlickrNet;
using FlickrNet.Internals;

namespace FlickrNet.Classes
{
    /// <summary>
    /// A collection of <see cref="Subscription"/> instances for the calling user.
    /// </summary>
    public sealed class SubscriptionCollection : Collection<Subscription>, IFlickrParsable
    {
        void IFlickrParsable.Load(System.Xml.XmlReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            if (reader.LocalName != "subscriptions") { UtilityMethods.CheckParsingException(reader); return; }

            reader.Read();

            while (reader.LocalName == "subscription")
            {
                var item = new Subscription();
                ((IFlickrParsable)item).Load(reader);
                Add(item);
            }

            reader.Skip();
        }
    }
}
