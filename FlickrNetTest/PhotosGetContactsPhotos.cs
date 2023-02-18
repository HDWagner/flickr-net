using System;
using FlickrNet;
using NUnit.Framework;
using Shouldly;

namespace FlickrNetTest
{
    [TestFixture]
    [Category("AccessTokenRequired")]
    public class PhotosGetContactsPhotos : BaseTest
    {
        [Test]
        public void PhotosGetContactsPhotosSignatureRequiredTest()
        {
            Should.Throw<SignatureRequiredException>(() => Instance.PhotosGetContactsPhotos());
        }

        [Test]
        [Ignore("Method requires authentication")]
        public void PhotosGetContactsPhotosIncorrectCountTest()
        {
            Should.Throw<ArgumentOutOfRangeException>(() => AuthInstance.PhotosGetContactsPhotos(51));
        }

        [Test]
        [Ignore("Method requires authentication")]
        public void PhotosGetContactsPhotosBasicTest()
        {
            PhotoCollection photos = AuthInstance.PhotosGetContactsPhotos(10);

            photos.Count.ShouldBeInRange(9, 10, "Should return 9-10 photos");

        }

        [Test]
        [Ignore("Method requires authentication")]
        public void PhotosGetContactsPhotosExtrasTest()
        {
            PhotoCollection photos = AuthInstance.PhotosGetContactsPhotos(10, false, false, false, PhotoSearchExtras.All);

            photos.Count.ShouldBeInRange(9, 10, "Should return 9-10 photos");

            foreach (Photo p in photos)
            {
                Assert.That(p.OwnerName, Is.Not.Null, "OwnerName should not be null");
                Assert.That(p.DateTaken, Is.Not.EqualTo(default(DateTime)), "DateTaken should not be default DateTime");
            }
        }
    }
}
