using System;
using System.IO;
using System.Linq;
using FlickrNet.Classes;
using FlickrNetTest.TestUtilities;
using NUnit.Framework;
using Shouldly;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for FlickrPhotosetsGetList
    /// </summary>
    [TestFixture]
    public class PhotosetsTests : BaseTest
    {
        [Test]
        public void GetContextTest()
        {
            const string photosetId = "72157594532130119";

            var photos = Instance.PhotosetsGetPhotos(photosetId);

            var firstPhoto = photos.First();
            var lastPhoto = photos.Last();

            var context1 = Instance.PhotosetsGetContext(firstPhoto.PhotoId, photosetId);

            Assert.That(context1, Is.Not.Null, "Context should not be null.");
            Assert.That(context1.PreviousPhoto, Is.Null, "PreviousPhoto should be null for first photo.");
            Assert.That(context1.NextPhoto, Is.Not.Null, "NextPhoto should not be null.");

            if (firstPhoto.PhotoId != lastPhoto.PhotoId)
            {
                Assert.That(context1.NextPhoto.PhotoId, Is.EqualTo(photos[1].PhotoId), "NextPhoto should be the second photo in photoset.");
            }

            var context2 = Instance.PhotosetsGetContext(lastPhoto.PhotoId, photosetId);

            Assert.That(context2, Is.Not.Null, "Last photo context should not be null.");
            Assert.That(context2.PreviousPhoto, Is.Not.Null, "PreviousPhoto should not be null for first photo.");
            Assert.That(context2.NextPhoto, Is.Null, "NextPhoto should be null.");

            if (firstPhoto.PhotoId != lastPhoto.PhotoId)
            {
                Assert.That(context2.PreviousPhoto.PhotoId, Is.EqualTo(photos[photos.Count - 2].PhotoId), "PreviousPhoto should be the last but one photo in photoset.");
            }
        }


        [Test]
        public void PhotosetsGetInfoBasicTest()
        {
            const string photosetId = "72157594532130119";

            var p = Instance.PhotosetsGetInfo(photosetId);

            Assert.That(p, Is.Not.Null);
            Assert.That(p.PhotosetId, Is.EqualTo(photosetId));
            Assert.That(p.Title, Is.EqualTo("Places: Derwent Walk, Gateshead"));
            Assert.That(p.Description, Is.EqualTo("It's near work, so I go quite a bit..."));
        }

        [Test]
        public void PhotosetsGetListBasicTest()
        {
            PhotosetCollection photosets = Instance.PhotosetsGetList(TestData.TestUserId);

            Assert.That(photosets, Is.Not.Empty, "Should be at least one photoset");
            Assert.That(photosets, Has.Count.GreaterThan(100), "Should be greater than 100 photosets. (" + photosets.Count + " returned)");

            foreach (Photoset set in photosets)
            {
                Assert.That(set.OwnerId, Is.Not.Null, "OwnerId should not be null");
                Assert.That(set.NumberOfPhotos, Is.GreaterThan(0), "NumberOfPhotos should be greater than zero");
                Assert.That(set.Title, Is.Not.Null, "Title should not be null");
                Assert.That(set.Description, Is.Not.Null, "Description should not be null");
                Assert.That(set.OwnerId, Is.EqualTo(TestData.TestUserId));
            }
        }

        [Test]
        [Category("AccessTokenRequired")]
        public void PhotosetsGetListWithExtras()
        {
            var testUserPhotoSets = AuthInstance.PhotosetsGetList(TestData.TestUserId, 1, 5, PhotoSearchExtras.All);

            testUserPhotoSets.Count.ShouldBeGreaterThan(0, "Should have returned at least 1 set for the authenticated user.");

            var firstPhotoSet = testUserPhotoSets.First();

            firstPhotoSet.PrimaryPhoto.ShouldNotBeNull("Primary Photo should not be null.");
            firstPhotoSet.PrimaryPhoto.LargeSquareThumbnailUrl.ShouldNotBeNullOrEmpty("LargeSquareThumbnailUrl should not be empty.");
        }

        [Test]
        public void PhotosetsGetListWebUrlTest()
        {
            PhotosetCollection photosets = Instance.PhotosetsGetList(TestData.TestUserId);

            Assert.That(photosets, Is.Not.Empty, "Should be at least one photoset");

            foreach (Photoset set in photosets)
            {
                Assert.That(set.Url, Is.Not.Null);
                string expectedUrl = "https://www.flickr.com/photos/" + TestData.TestUserId + "/sets/" + set.PhotosetId + "/";
                Assert.That(set.Url, Is.EqualTo(expectedUrl));
            }
        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void PhotosetsCreateAddPhotosTest()
        {
            byte[] imageBytes = TestData.TestImageBytes;
            var s = new MemoryStream(imageBytes);

            const string initialPhotoTitle = "Test Title";
            const string updatedPhotoTitle = "New Test Title";
            const string initialPhotoDescription = "Test Description\nSecond Line";
            const string updatedPhotoDescription = "New Test Description";
            const string initialTags = "testtag1,testtag2";

            s.Position = 0;
            // Upload photo once
            var photoId1 = AuthInstance.UploadPicture(s, "Test1.jpg", initialPhotoTitle, initialPhotoDescription, initialTags, false, false, false, ContentType.Other, SafetyLevel.Safe, HiddenFromSearch.Visible);
            Assert.That(photoId1, Is.Not.Null);

            s.Position = 0;
            // Upload photo a second time
            var photoId2 = AuthInstance.UploadPicture(s, "Test2.jpg", initialPhotoTitle, initialPhotoDescription, initialTags, false, false, false, ContentType.Other, SafetyLevel.Safe, HiddenFromSearch.Visible);
            Assert.That(photoId2, Is.Not.Null);

            // Creat photoset
            Photoset photoset = AuthInstance.PhotosetsCreate("Test photoset", photoId1);

            try
            {
                var photos = AuthInstance.PhotosetsGetPhotos(photoset.PhotosetId, PhotoSearchExtras.OriginalFormat | PhotoSearchExtras.Media, PrivacyFilter.None, 1, 30, MediaType.None);

                photos.Count.ShouldBe(1, "Photoset should contain 1 photo");
                photos[0].IsPublic.ShouldBe(false, "Photo 1 should be private");

                // Add second photo to photoset.
                AuthInstance.PhotosetsAddPhoto(photoset.PhotosetId, photoId2);

                // Remove second photo from photoset
                AuthInstance.PhotosetsRemovePhoto(photoset.PhotosetId, photoId2);

                AuthInstance.PhotosetsEditMeta(photoset.PhotosetId, updatedPhotoTitle, updatedPhotoDescription);

                photoset = AuthInstance.PhotosetsGetInfo(photoset.PhotosetId);

                photoset.Title.ShouldBe(updatedPhotoTitle, "New Title should be set.");
                photoset.Description.ShouldBe(updatedPhotoDescription, "New description should be set");

                AuthInstance.PhotosetsEditPhotos(photoset.PhotosetId, photoId1, new[] { photoId2, photoId1 });

                AuthInstance.PhotosetsRemovePhoto(photoset.PhotosetId, photoId2);
            }
            finally
            {
                // Delete photoset completely
                AuthInstance.PhotosetsDelete(photoset.PhotosetId);

                // Delete both photos.
                AuthInstance.PhotosDelete(photoId1);
                AuthInstance.PhotosDelete(photoId2);
            }
        }

        [Test]
        public void PhotosetsGetInfoEncodingCorrect()
        {
            Photoset pset = Instance.PhotosetsGetInfo("72157627650627399");

            Assert.That(pset.Title, Is.EqualTo("Sítio em Arujá - 14/08/2011"));
        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void PhotosetGetInfoGetList()
        {
            const string photosetId = "72157660633195178";

            var photosetInfo = AuthInstance.PhotosetsGetInfo(photosetId);

            photosetInfo.NumberOfVideos.ShouldBe(2);
            photosetInfo.NumberOfPhotos.ShouldBe(72);
            photosetInfo.Total.ShouldBe(74);

            var photosetList = AuthInstance.PhotosetsGetList();

            var photosetListInfo = photosetList.First(s => s.PhotosetId == photosetId);
            photosetListInfo.NumberOfVideos.ShouldBe(2);
            photosetListInfo.NumberOfPhotos.ShouldBe(72);
            photosetListInfo.Total.ShouldBe(74);
        }
    }
}
