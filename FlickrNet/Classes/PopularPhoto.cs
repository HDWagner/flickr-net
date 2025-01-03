﻿using System;
using System.Collections.Generic;
using System.Text;
using FlickrNet;
using FlickrNet.Internals;

namespace FlickrNet.Classes
{
    /// <summary>
    /// Details about a popular photo, including the statistics for its views, comments and favourites for the date.
    /// </summary>
    public class PopularPhoto : Photo, IFlickrParsable
    {
        /// <summary>
        /// The number of views this photo has received in the context of the calling time period.
        /// </summary>
        public int StatViews { get; set; }
        /// <summary>
        /// The number of comments this photo has received in the context of the calling time period.
        /// </summary>
        public int StatComments { get; set; }
        /// <summary>
        /// The number of favorites this photo has received in the context of the calling time period.
        /// </summary>
        public int StatFavorites { get; set; }
        /// <summary>
        /// The number of views this photo has received in total.
        /// </summary>
        public int TotalViews { get; set; }
        /// <summary>
        /// The number of comments this photo has received in total.
        /// </summary>
        public int TotalComments { get; set; }
        /// <summary>
        /// The number of favorites this photo has received in total.
        /// </summary>
        public int TotalFavorites { get; set; }

        void IFlickrParsable.Load(System.Xml.XmlReader reader)
        {
            Load(reader, false);

            if (reader.LocalName == "photo")
            {
                return;
            }

            if (reader.LocalName != "stats")
            {
                UtilityMethods.CheckParsingException(reader);
            }

            while (reader.MoveToNextAttribute())
            {
                switch (reader.LocalName)
                {
                    case "views":
                        StatViews = int.Parse(reader.Value, System.Globalization.NumberFormatInfo.InvariantInfo);
                        break;
                    case "comments":
                        StatComments = int.Parse(reader.Value, System.Globalization.NumberFormatInfo.InvariantInfo);
                        break;
                    case "favorites":
                        StatFavorites = int.Parse(reader.Value, System.Globalization.NumberFormatInfo.InvariantInfo);
                        break;
                    case "total_views":
                        TotalViews = reader.ReadContentAsInt();
                        break;
                    case "total_comments":
                        TotalComments = reader.ReadContentAsInt();
                        break;
                    case "total_favorites":
                        TotalFavorites = reader.ReadContentAsInt();
                        break;
                    default:
                        UtilityMethods.CheckParsingException(reader);
                        break;
                }
            }

            reader.Read();

            if (reader.LocalName == "description")
            {
                Description = reader.ReadElementContentAsString();
            }

            reader.Skip();
        }
    }
}
