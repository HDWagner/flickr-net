using FlickrNet;
using FlickrNet.Classes;
using FlickrNetTest.TestUtilities;
using NUnit.Framework;

namespace FlickrNetTest
{

    [TestFixture]
    public class PhotosGetContactsPublicPhotosTests : BaseTest
    {

        [Test]
        public void PhotosGetContactsPublicPhotosUserIdExtrasTest()
        {
            Flickr f = Instance;

            string userId = TestData.TestUserId;
            PhotoSearchExtras extras = PhotoSearchExtras.All;
            var photos = f.PhotosGetContactsPublicPhotos(userId, extras);

            Assert.That(photos, Is.Not.Null);
            Assert.That(photos, Is.Not.Empty, "Should have returned more than 0 photos");
        }

        [Test]
        public void PhotosGetContactsPublicPhotosAllParamsTest()
        {
            Flickr f = Instance;

            string userId = TestData.TestUserId;

            int count = 4; // TODO: Initialize to an appropriate value
            bool justFriends = true; // TODO: Initialize to an appropriate value
            bool singlePhoto = true; // TODO: Initialize to an appropriate value
            bool includeSelf = false; // TODO: Initialize to an appropriate value
            PhotoSearchExtras extras = PhotoSearchExtras.None;

            var photos = f.PhotosGetContactsPublicPhotos(userId, count, justFriends, singlePhoto, includeSelf, extras);

            Assert.That(photos, Is.Not.Null);
            Assert.That(photos, Is.Not.Empty, "Should have returned more than 0 photos");
        }

        [Test]
        public void PhotosGetContactsPublicPhotosExceptExtrasTest()
        {
            Flickr f = Instance;

            string userId = TestData.TestUserId;

            int count = 4; 
            bool justFriends = true; 
            bool singlePhoto = true; 
            bool includeSelf = false; 

            var photos = f.PhotosGetContactsPublicPhotos(userId, count, justFriends, singlePhoto, includeSelf);

            Assert.That(photos, Is.Not.Null);
            Assert.That(photos, Is.Not.Empty, "Should have returned more than 0 photos");
        }

        [Test]
        public void PhotosGetContactsPublicPhotosUserIdTest()
        {
            Flickr f = Instance;

            string userId = TestData.TestUserId;

            var photos = f.PhotosGetContactsPublicPhotos(userId);

            Assert.That(photos, Is.Not.Null);
            Assert.That(photos, Is.Not.Empty, "Should have returned more than 0 photos");
        }

        [Test]
        public void PhotosGetContactsPublicPhotosUserIdCountExtrasTest()
        {
            Flickr f = Instance;

            string userId = TestData.TestUserId;

            int count = 5; 
            PhotoSearchExtras extras = PhotoSearchExtras.None;

            var photos = f.PhotosGetContactsPublicPhotos(userId, count, extras);

            Assert.That(photos, Is.Not.Null);
            Assert.That(photos, Is.Not.Empty, "Should have returned more than 0 photos");
        }

        [Test]
        public void PhotosGetContactsPublicPhotosUserIdCountTest()
        {
            Flickr f = Instance;

            string userId = TestData.TestUserId;

            int count = 5;

            var photos = f.PhotosGetContactsPublicPhotos(userId, count);

            Assert.That(photos, Is.Not.Null);
            Assert.That(photos, Is.Not.Empty, "Should have returned more than 0 photos");
        }
    }
}
