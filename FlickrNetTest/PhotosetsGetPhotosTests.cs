using System;
using System.Linq;
using NUnit.Framework;
using FlickrNet;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for FlickrPhotoSetGetPhotos
    /// </summary>
    [TestFixture]
    public class PhotosetsGetPhotosTests : BaseTest
    {
        [Test]
        public void PhotosetsGetPhotosBasicTest()
        {
            PhotosetPhotoCollection set = Instance.PhotosetsGetPhotos("72157618515066456", PhotoSearchExtras.All, PrivacyFilter.None, 1, 10);

            Assert.That(set.Total, Is.EqualTo(8), "NumberOfPhotos should be 8.");
            Assert.That(set, Has.Count.EqualTo(8), "Should be 8 photos returned.");
        }

        [Test]
        public void PhotosetsGetPhotosMachineTagsTest()
        {
            var set = Instance.PhotosetsGetPhotos("72157594218885767", PhotoSearchExtras.MachineTags, PrivacyFilter.None, 1, 10);

            var machineTagsFound = set.Any(p => !string.IsNullOrEmpty(p.MachineTags));

            Assert.That(machineTagsFound, Is.True, "No machine tags were found in the photoset");
        }

        [Test]
        public void PhotosetsGetPhotosFilterMediaTest()
        {
            // https://www.flickr.com/photos/sgoralnick/sets/72157600283870192/
            // Set contains videos and photos
            var theset = Instance.PhotosetsGetPhotos("72157600283870192", PhotoSearchExtras.Media, PrivacyFilter.None, 1, 100, MediaType.Videos);

            Assert.That(theset.Title, Is.EqualTo("Canon 5D"));

            foreach (var p in theset)
            {
                Assert.That(p.Media, Is.EqualTo("video"), "Should be video.");
            }

            var theset2 = Instance.PhotosetsGetPhotos("72157600283870192", PhotoSearchExtras.Media, PrivacyFilter.None, 1, 100, MediaType.Photos);
            foreach (var p in theset2)
            {
                Assert.That(p.Media, Is.EqualTo("photo"), "Should be photo.");
            }

        }

        [Test]
        public void PhotosetsGetPhotosWebUrlTest()
        {
            var theset = Instance.PhotosetsGetPhotos("72157618515066456");

            foreach(var p in theset)
            {
                Assert.That(p.UserId, Is.Not.Null, "UserId should not be null.");
                Assert.That(p.UserId, Is.Not.EqualTo(string.Empty), "UserId should not be an empty string.");
                var url = "https://www.flickr.com/photos/" + p.UserId + "/" + p.PhotoId + "/";
                Assert.That(p.WebUrl, Is.EqualTo(url));
            }
        }

        [Test]
        public void PhotosetsGetPhotosPrimaryPhotoTest()
        {
            var theset = Instance.PhotosetsGetPhotos("72157618515066456", 1, 100);

            Assert.That(theset.PrimaryPhotoId, Is.Not.Null, "PrimaryPhotoId should not be null.");

            if (theset.Total >= theset.PerPage)
            {
                return;
            }

            var primary = theset.FirstOrDefault(p => p.PhotoId == theset.PrimaryPhotoId);

            Assert.That(primary, Is.Not.Null, "Primary photo should have been found.");
        }

        [Test]
        [Category("AccessTokenRequired")]
        public void PhotosetsGetPhotosOrignalTest()
        {
            var photos = AuthInstance.PhotosetsGetPhotos("72157623027759445", PhotoSearchExtras.AllUrls);

            foreach (var photo in photos)
            {
                Assert.That(photo.OriginalUrl, Is.Not.Null, "Original URL should not be null.");
            }
        }

        [Test]
        public void ShouldReturnDateTakenWhenAsked()
        {
            var theset = Instance.PhotosetsGetPhotos("72157618515066456", PhotoSearchExtras.DateTaken | PhotoSearchExtras.DateUploaded, 1, 10);

            var firstInvalid = theset.FirstOrDefault(p => p.DateTaken == DateTime.MinValue || p.DateUploaded == DateTime.MinValue);

            Assert.That(firstInvalid, Is.Null, "There should not be a photo with not date taken or date uploaded");

            theset = Instance.PhotosetsGetPhotos("72157618515066456", PhotoSearchExtras.All, 1, 10);

            firstInvalid = theset.FirstOrDefault(p => p.DateTaken == DateTime.MinValue || p.DateUploaded == DateTime.MinValue);

            Assert.That(firstInvalid, Is.Null, "There should not be a photo with not date taken or date uploaded");

            theset = Instance.PhotosetsGetPhotos("72157618515066456", PhotoSearchExtras.None, 1, 10);

            var noDateCount = theset.Count(p => p.DateTaken == DateTime.MinValue || p.DateUploaded == DateTime.MinValue);

            Assert.That(noDateCount, Is.EqualTo(theset.Count), "All photos should have no date taken set.");
        }
    }
}
