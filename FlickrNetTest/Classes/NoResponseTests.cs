using FlickrNet.Classes;
using FlickrNet.Internals;
using FlickrNetTest.TestUtilities;
using NUnit.Framework;
using System.Xml;

namespace FlickrNetTest.Classes
{
    [TestFixture]
    internal class NoResponseTests : BaseTest
    {
        [Test]
        public void NoResponse_Load()
        {
            var cut = new NoResponse();
            ((IFlickrParsable)cut).Load(XmlReader.Create(TextReader.Null));

            Assert.That(cut, Is.Not.Null);
        }
    }
}
