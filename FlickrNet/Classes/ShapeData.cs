﻿using FlickrNet.Exceptions;
using FlickrNet.Internals;
using System;
using System.Collections.ObjectModel;

namespace FlickrNet.Classes
{
    /// <summary>
    /// The shape data supplied by <see cref="Flickr.PlacesGetInfo"/>.
    /// </summary>
    /// <remarks>
    /// See <a href="http://code.flickr.com/blog/2008/10/30/the-shape-of-alpha/">http://code.flickr.com/blog/2008/10/30/the-shape-of-alpha/</a> for more details.
    /// </remarks>
    public sealed class ShapeData : IFlickrParsable
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public ShapeData()
        {
            PolyLines = new Collection<Collection<PointD>>();
            Urls = new Collection<string>();
        }

        /// <summary>
        /// The date the shapedata was created.
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// The alpha for the shape data.
        /// </summary>
        public double Alpha { get; set; }

        /// <summary>
        /// The number of points in the shapefile.
        /// </summary>
        public int PointCount { get; set; }

        /// <summary>
        /// The number of edge in the shapefile.
        /// </summary>
        public int EdgeCount { get; set; }

        /// <summary>
        /// Does the shape have a donut hole.
        /// </summary>
        public bool HasDonutHole { get; set; }

        /// <summary>
        /// Is the shape a donut hole.
        /// </summary>
        public bool IsDonutHole { get; set; }

        /// <summary>
        /// A list of polylines making up the shape. Each polyline is itself a list of points.
        /// </summary>
        public Collection<Collection<PointD>> PolyLines { get; set; }

        /// <summary>
        /// A list of urls for the shapefiles.
        /// </summary>
        public Collection<string> Urls { get; set; }

        void IFlickrParsable.Load(System.Xml.XmlReader reader)
        {
            while (reader.MoveToNextAttribute())
            {
                switch (reader.LocalName)
                {
                    case "created":
                        DateCreated = UtilityMethods.UnixTimestampToDate(reader.Value);
                        break;
                    case "alpha":
                        Alpha = reader.ReadContentAsDouble();
                        break;
                    case "count_points":
                        PointCount = reader.ReadContentAsInt();
                        break;
                    case "count_edges":
                        EdgeCount = reader.ReadContentAsInt();
                        break;
                    case "has_donuthole":
                        HasDonutHole = reader.Value == "1";
                        break;
                    case "is_donuthole":
                        IsDonutHole = reader.Value == "1";
                        break;
                    default:
                        UtilityMethods.CheckParsingException(reader);
                        break;
                }
            }

            reader.Read();

            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                switch (reader.LocalName)
                {
                    case "polylines":
                        reader.Read();
                        while (reader.LocalName == "polyline")
                        {
                            var polyline = new Collection<PointD>();
                            string polystring = reader.ReadElementContentAsString();
                            string[] points = polystring.Split(' ');
                            foreach (string point in points)
                            {
                                string[] xy = point.Split(',');
                                if (xy.Length != 2)
                                {
                                    throw new ParsingException("Invalid polypoint found in polyline : '" + polystring +
                                                               "'");
                                }

                                polyline.Add(
                                    new PointD(
                                        double.Parse(xy[0], System.Globalization.NumberFormatInfo.InvariantInfo),
                                        double.Parse(xy[1], System.Globalization.NumberFormatInfo.InvariantInfo)));
                            }
                            PolyLines.Add(polyline);
                        }
                        reader.Read();
                        break;
                    case "urls":
                        reader.Skip();
                        break;

                }
            }

            reader.Read();
        }
    }

    /// <summary>
    /// A point structure for holding double-floating points precision data.
    /// </summary>
    public struct PointD
    {
        /// <summary>
        /// The X position of the point.
        /// </summary>
        public double X { get; }

        /// <summary>
        /// The Y position of the point.
        /// </summary>
        public double Y { get; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public PointD(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// For predefined value types, the equality operator (==) returns true if the values of its operands are equal, false otherwise.
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        public static bool operator ==(PointD point1, PointD point2)
        {
            return Math.Abs(point1.X - point2.X) < 0.001 && Math.Abs(point1.Y - point2.Y) < 0.001;
        }

        /// <summary>
        /// For predefined value types, the equality operator (!=) returns false if the values of its operands are equal, true otherwise.
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        public static bool operator !=(PointD point1, PointD point2)
        {
            return Math.Abs(point1.X - point2.X) > 0.001 || Math.Abs(point1.Y - point2.Y) > 0.001;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj is PointD p)
            {
                return this == p;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode();
        }
    }
}


