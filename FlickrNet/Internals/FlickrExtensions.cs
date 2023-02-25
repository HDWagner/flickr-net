using FlickrNet.Classes;
using FlickrNet.Exceptions;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace FlickrNet.Internals
{
    internal static class FlickrExtensions
    {
        public static void Load(this IFlickrParsable item, string originalXml)
        {
            try
            {
                var reader = XmlReader.Create(new StringReader(originalXml), new XmlReaderSettings
                {
                    IgnoreWhitespace = true
                });

                if (!reader.ReadToDescendant("rsp"))
                {
                    throw new FlickrException("Unable to find response element 'rsp' in Flickr response");
                }
                while (reader.MoveToNextAttribute())
                {
                    if (reader.LocalName == "stat" && reader.Value == "fail")
                    {
                        throw ExceptionHandler.CreateResponseException(reader);
                    }
                }

                reader.MoveToElement();
                reader.Read();

                item.Load(reader);
            }
            catch (XmlException)
            {
                var newReader = XmlReader.Create(new StringReader(SanitizeXmlString(originalXml)), new XmlReaderSettings
                {
                    IgnoreWhitespace = true
                });

                if (!newReader.ReadToDescendant("rsp"))
                {
                    throw new FlickrException("Unable to find response element 'rsp' in Flickr response");
                }

                while (newReader.MoveToNextAttribute())
                {
                    if (newReader.LocalName == "stat" && newReader.Value == "fail")
                    {
                        throw ExceptionHandler.CreateResponseException(newReader);
                    }
                }

                newReader.MoveToElement();
                newReader.Read();

                item.Load(newReader);
            }
        }

        private static string SanitizeXmlString(string xml)
        {
            if (xml == null)
            {
                throw new ArgumentNullException(nameof(xml));
            }

            var buffer = new StringBuilder(xml.Length);
            var xmlWithLegalCharacters = xml.Where(c => IsLegalXmlChar(c));
            foreach (var c in xmlWithLegalCharacters)
            {
                buffer.Append(c);
            }

            return buffer.ToString();
        }

        /// <summary>
        /// Whether a given character is allowed by XML 1.0.
        /// </summary>
        private static bool IsLegalXmlChar(int character)
        {
            return

                 character == 0x9 /* == '\t' == 9   */          ||
                 character == 0xA /* == '\n' == 10  */          ||
                 character == 0xD /* == '\r' == 13  */          ||
                character >= 0x20 && character <= 0xD7FF ||
                character >= 0xE000 && character <= 0xFFFD ||
                character >= 0x10000 && character <= 0x10FFFF
            ;
        }
    }
}
