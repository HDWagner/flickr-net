using System.Collections.Generic;

using NUnit.Framework;
using FlickrNet;
using FlickrNet.Classes;
using FlickrNetTest.TestUtilities;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for TestTests
    /// </summary>
    [TestFixture]
    public class TestTests : BaseTest
    {
        [Test]
        public void TestGenericGroupSearch()
        {
            Flickr f = Instance;

            var parameters = new Dictionary<string, string>();
            parameters.Add("text", "Flowers");
            UnknownResponse response = f.TestGeneric("flickr.groups.search", parameters);

            Assert.That(response, Is.Not.Null, "UnknownResponse should not be null.");
            Assert.That(response.ResponseXml, Is.Not.Null, "ResponseXml should not be null.");

        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void TestGenericTestNull()
        {
            Flickr f = AuthInstance;

            UnknownResponse response = f.TestGeneric("flickr.test.null", null);

            Assert.That(response, Is.Not.Null, "UnknownResponse should not be null.");
            Assert.That(response.ResponseXml, Is.Not.Null, "ResponseXml should not be null.");
        }

        [Test]
        public void TestEcho()
        {
            Flickr f = Instance;
            var parameters = new Dictionary<string, string>();
            parameters.Add("test1", "testvalue");

            Dictionary<string, string> returns = f.TestEcho(parameters);

            Assert.That(returns, Is.Not.Null);

            // Was 3, now 11 because of extra oauth parameter used by default.
            Assert.That(returns, Has.Count.EqualTo(11));

            Assert.That(returns["method"], Is.EqualTo("flickr.test.echo"));
            Assert.That(returns["test1"], Is.EqualTo("testvalue"));

        }
    }
}
