using System;
using System.Collections.Generic;

using NUnit.Framework;
using FlickrNet;
using Shouldly;
using System.Linq;
using FlickrNet.Classes;
using FlickrNetTest.TestUtilities;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for GalleriesTests
    /// </summary>
    [TestFixture]
    public class GalleriesTests : BaseTest
    {

        [Test]
        public void GalleriesGetListUserIdTest()
        {
            Flickr f = Instance;

            GalleryCollection galleries = f.GalleriesGetList(TestData.TestUserId);

            Assert.That(galleries, Is.Not.Null, "GalleryCollection should not be null.");
            Assert.That(galleries, Is.Not.Empty, "Count should not be zero.");

            foreach (var g in galleries)
            {
                Assert.That(g, Is.Not.Null);
                Assert.That(g.Title, Is.Not.Null, "Title should not be null.");
                Assert.That(g.GalleryId, Is.Not.Null, "GalleryId should not be null.");
                Assert.That(g.GalleryUrl, Is.Not.Null, "GalleryUrl should not be null.");
            }
        }

        [Test]
        public void GalleriesGetListForPhotoTest()
        {
            string photoId = "2891347068";

            var galleries = Instance.GalleriesGetListForPhoto(photoId);

            Assert.That(galleries, Is.Not.Null, "GalleryCollection should not be null.");
            Assert.That(galleries, Is.Not.Empty, "Count should not be zero.");

            foreach (var g in galleries)
            {
                Assert.That(g, Is.Not.Null);
                Assert.That(g.Title, Is.Not.Null, "Title should not be null.");
                Assert.That(g.GalleryId, Is.Not.Null, "GalleryId should not be null.");
                Assert.That(g.GalleryUrl, Is.Not.Null, "GalleryUrl should not be null.");
            }
        }

        [Test]
        public void GalleriesGetPhotos()
        {
            // Dogs + Tennis Balls
            // https://www.flickr.com/photos/lesliescarter/galleries/72157622656415345
            string galleryId = "13834290-72157622656415345";

            Flickr f = Instance;

            GalleryPhotoCollection photos = f.GalleriesGetPhotos(galleryId, PhotoSearchExtras.All);

            Console.WriteLine(f.LastRequest);

            Assert.That(photos, Is.Not.Null);
            Assert.That(photos, Has.Count.EqualTo(15), "Count should be fifteen.");

            foreach (var photo in photos)
            {
                //This gallery has a comment on each photo.
                Assert.That(photo.Comment, Is.Not.Null, "GalleryPhoto.Comment shoult not be null.");
            }
        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void GalleriesEditPhotosTest()
        {
            Flickr.FlushCache();
            Flickr.CacheDisabled = true;

            Flickr f = AuthInstance;

            string galleryId = "78188-72157622589312064";

            var gallery = f.GalleriesGetInfo(galleryId);

            Console.WriteLine("GalleryUrl = " + gallery.GalleryUrl);

            var photos = f.GalleriesGetPhotos(galleryId);

            var photoIds = new List<string>();

            foreach (var photo in photos)
            {
                photoIds.Add(photo.PhotoId);
            }

            f.GalleriesEditPhotos(galleryId, gallery.PrimaryPhotoId, photoIds);

            var photos2 = f.GalleriesGetPhotos(gallery.GalleryId);

            Assert.That(photos2, Has.Count.EqualTo(photos.Count));

            for (int i = 0; i < photos.Count; i++)
            {
                Assert.That(photos2[i].PhotoId, Is.EqualTo(photos[i].PhotoId));
            }
        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void GalleriesEditMetaTest()
        {
            Flickr.FlushCache();
            Flickr.CacheDisabled = true;

            Flickr f = AuthInstance;

            string galleryId = "78188-72157622589312064";

            string title = "Great Entrances to Hell";
            string description = "A guide to what makes a great photo for the Entrances to Hell group: " +
                                 "<a href=\"https://www.flickr.com/groups/entrancetohell\">www.flickr.com/groups/entrancetohell</a>\n\n";
            description += DateTime.Now.ToString();

            f.GalleriesEditMeta(galleryId, title, description);

            Gallery gallery = f.GalleriesGetInfo(galleryId);

            Assert.That(gallery.Title, Is.EqualTo(title));
            Assert.That(gallery.Description, Is.EqualTo(description));
        }

        [Test, Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void GalleriesAddRemovePhoto()
        {
            string photoId = "18841298081";
            string galleryId = "78188-72157622589312064";
            string comment = "no comment";

            Flickr f = AuthInstance;
            f.GalleriesAddPhoto(galleryId, photoId, comment);

            var photos = f.GalleriesGetPhotos(galleryId);
            photos.ShouldContain(p => p.PhotoId == photoId);

            f.GalleriesRemovePhoto(galleryId, photoId, "");

            photos = f.GalleriesGetPhotos(galleryId);
            photos.ShouldNotContain(p => p.PhotoId == photoId);
        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void GalleriesEditPhotoTest()
        {
            Flickr.FlushCache();
            Flickr.CacheDisabled = true;

            string photoId = "486875512";
            string galleryId = "78188-72157622589312064";

            string comment = "You don't get much better than this for the best Entrance to Hell.\n\n" + DateTime.Now.ToString();

            Flickr f = AuthInstance;
            f.GalleriesEditPhoto(galleryId, photoId, comment);

            var photos = f.GalleriesGetPhotos(galleryId);

            bool found = false;

            foreach (var photo in photos)
            {
                if (photo.PhotoId == photoId)
                {
                    Assert.That(photo.Comment, Is.EqualTo(comment), "Comment should have been updated.");
                    found = true;
                    break;
                }
            }

            Assert.That(found, Is.True, "Should have found the photo in the gallery.");
        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void GalleriesEditComplexTest()
        {
            Flickr.CacheDisabled = true;
            Flickr.FlushCache();

            string primaryPhotoId = "486875512";
            string galleryId = "78188-72157622589312064";

            Flickr f = AuthInstance;

            // Get photos
            var photos = f.GalleriesGetPhotos(galleryId);

            var photoIds = new List<string>();
            foreach (var p in photos)
            {
                photoIds.Add(p.PhotoId);
            }

            // Remove the last one.
            GalleryPhoto photo = photos.Last(p => p.PhotoId != primaryPhotoId);
            photoIds.Remove(photo.PhotoId);

            // Update the gallery
            f.GalleriesEditPhotos(galleryId, primaryPhotoId, photoIds);

            // Check removed photo no longer returned.
            var photos2 = f.GalleriesGetPhotos(galleryId);

            Assert.That(photos2, Has.Count.EqualTo(photos.Count - 1), "Should be one less photo.");

            bool found = false;
            foreach (var p in photos2)
            {
                if (p.PhotoId == photo.PhotoId)
                {
                    found = true;
                    break;
                }
            }
            Assert.That(found, Is.False, "Should not have found the photo in the gallery.");

            // Add photo back in
            f.GalleriesAddPhoto(galleryId, photo.PhotoId, photo.Comment);

            var photos3 = f.GalleriesGetPhotos(galleryId);
            Assert.That(photos3, Has.Count.EqualTo(photos.Count), "Count should match now photo added back in.");

            found = false;
            foreach (var p in photos3)
            {
                if (p.PhotoId == photo.PhotoId)
                {
                    Assert.That(p.Comment, Is.EqualTo(photo.Comment), "Comment should have been updated.");
                    found = true;
                    break;
                }
            }

            Assert.That(found, Is.True, "Should have found the photo in the gallery.");
        }

    }
}
