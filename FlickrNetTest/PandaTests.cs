
using NUnit.Framework;
using FlickrNet;
using FlickrNetTest.TestUtilities;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for PandaGetListTest
    /// </summary>
    [TestFixture]
    public class PandaTests : BaseTest
    {
        [Test]
        public void PandaGetListBasicTest()
        {
            string[] pandas = Instance.PandaGetList();

            Assert.That(pandas, Is.Not.Null, "Should return string array");
            Assert.That(pandas, Is.Not.Empty, "Should not return empty array");

            Assert.That(pandas[0], Is.EqualTo("ling ling"));
            Assert.That(pandas[1], Is.EqualTo("hsing hsing"));
            Assert.That(pandas[2], Is.EqualTo("wang wang"));
        }

        [Test]
        public void PandaGetPhotosLingLingTest()
        {
            var photos = Instance.PandaGetPhotos("ling ling");

            Assert.That(photos, Is.Not.Null, "PandaPhotos should not be null.");
            Assert.That(photos.Total, Is.EqualTo(photos.Count), "PandaPhotos.Count should equal PandaPhotos.Total.");
            Assert.That(photos.PandaName, Is.EqualTo("ling ling"), "PandaPhotos.Panda should be 'ling ling'");
        }
    }
}
