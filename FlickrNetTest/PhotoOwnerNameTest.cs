
using NUnit.Framework;
using FlickrNet;
using FlickrNet.Classes;
using FlickrNetTest.TestUtilities;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for PhotoOwnerNameTest
    /// </summary>
    [TestFixture]
    public class PhotoOwnerNameTest : BaseTest
    {
        [Test]
        public void PhotosSearchOwnerNameTest()
        {
            var o = new PhotoSearchOptions();

            o.UserId = TestData.TestUserId;
            o.PerPage = 10;
            o.Extras = PhotoSearchExtras.OwnerName;

            Flickr f = Instance;
            PhotoCollection photos = f.PhotosSearch(o);

            Assert.That(photos[0].OwnerName, Is.Not.Null);
           
        }

        [Test]
        public void PhotosGetContactsPublicPhotosOwnerNameTest()
        {
            Flickr f = Instance;
            PhotoCollection photos = f.PhotosGetContactsPublicPhotos(TestData.TestUserId, PhotoSearchExtras.OwnerName);

            Assert.That(photos[0].OwnerName, Is.Not.Null);
        }

    }
}
