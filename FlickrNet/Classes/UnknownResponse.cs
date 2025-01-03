﻿using System.Collections.Generic;
using System.Xml;
using FlickrNet;
using FlickrNet.Internals;

namespace FlickrNet.Classes
{
    /// <summary>
    /// Contains the raw response from Flickr when an unknown method has been called. Used by <see cref="Flickr.TestGeneric"/>.
    /// </summary>
    public sealed class UnknownResponse : IFlickrParsable
    {
        /// <summary>
        /// The response from Flickr.
        /// </summary>
        public string? ResponseXml { get; set; }

        /// <summary>
        /// Gets a <see cref="XmlDocument"/> containing the response XML.
        /// </summary>
        /// <returns></returns>
        public XmlDocument GetXmlDocument()
        {
            var document = new XmlDocument();
            if (ResponseXml != null)
            {
                document.LoadXml(ResponseXml);
            }

            return document;
        }

        /// <summary>
        /// Gets an attribute value from the given response.
        /// </summary>
        /// <param name="element">The element name to find.</param>
        /// <param name="attribute">The attribute of the element to return.</param>
        /// <returns>The string value of the given attribute, if found.</returns>
        public string? GetAttributeValue(string element, string attribute)
        {
            var doc = GetXmlDocument();
            var node = doc.SelectSingleNode("//" + element + "/@" + attribute);
            return node?.Value;
        }

        /// <summary>
        /// Gets an text value of an element from the given response.
        /// </summary>
        /// <param name="element">The element name to find.</param>
        /// <returns>The string value of the given element, if found.</returns>
        public string? GetElementValue(string element)
        {
            var doc = GetXmlDocument();
            var node = doc.SelectSingleNode("//" + element + "[1]");
            return node?.InnerText;
        }



        void IFlickrParsable.Load(XmlReader reader)
        {
            ResponseXml = reader.ReadOuterXml();
        }

        /// <summary>
        /// Gets an array of text values of an element from the given response.
        /// </summary>
        /// <param name="elementName">The element name to find.</param>
        /// <returns>An array of string values.</returns>
        public string[] GetElementArray(string elementName)
        {
            var array = new List<string>();
            var nodelist = GetXmlDocument().SelectNodes("//" + elementName);
            if (nodelist == null)
            {
                return array.ToArray();
            }
            foreach (XmlNode n in nodelist)
            {
                array.Add(n.InnerText);
            }
            return array.ToArray();
        }

        /// <summary>
        /// Gets an array of text values of an element's attribute from the given response.
        /// </summary>
        /// <param name="elementName">The element name to find.</param>
        /// <param name="attributeName">The attribute name to find on the matching element.</param>
        /// <returns>An array of string values.</returns>
        public string[] GetElementArray(string elementName, string attributeName)
        {
            var array = new List<string>();
            var nodelist = GetXmlDocument().SelectNodes("//" + elementName + "/@" + attributeName);
            if (nodelist == null)
            {
                return array.ToArray();
            }
            foreach (XmlNode n in nodelist)
            {
                array.Add(n.InnerText);
            }
            return array.ToArray();
        }
    }
}
