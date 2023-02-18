using System;

using NUnit.Framework;
using FlickrNet;
using System.IO;
using System.Net;
using System.Linq;
using System.Reactive.Subjects;
using System.Reactive.Linq;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for PhotosUploadTests
    /// </summary>
    [TestFixture]
    [Category("AccessTokenRequired")]
    public class PhotosUploadTests : BaseTest
    {
        [Test]
        [Ignore("Method requires authentication")]
        public void UploadPictureAsyncBasicTest()
        {
            Flickr f = AuthInstance;

            var w = new AsyncSubject<FlickrResult<string>>();

            byte[] imageBytes = TestData.TestImageBytes;
            var s = new MemoryStream(imageBytes);
            s.Position = 0;

            string title = "Test Title";
            string desc = "Test Description\nSecond Line";
            string tags = "testtag1,testtag2";

            f.UploadPictureAsync(s, "Test.jpg", title, desc, tags, false, false, false, ContentType.Other, SafetyLevel.Safe, HiddenFromSearch.Visible,
                r => { w.OnNext(r); w.OnCompleted(); });

            var result = w.Next().First();

            if (result.HasError)
            {
                throw result.Error;
            }

            Assert.That(result.Result, Is.Not.Null);
            Console.WriteLine(result.Result);

            // Clean up photo
            f.PhotosDelete(result.Result);
        }

        [Test]
        [Ignore("Method requires authentication")]
        public void UploadPictureBasicTest()
        {
            Flickr f = AuthInstance;

            f.OnUploadProgress += (sender, args) =>
            {
                // Do nothing
            };

            byte[] imageBytes = TestData.TestImageBytes;
            var s = new MemoryStream(imageBytes);
            s.Position = 0;

            string title = "Test Title";
            string desc = "Test Description\nSecond Line";
            string tags = "testtag1,testtag2";
            string photoId = f.UploadPicture(s, "Test.jpg", title, desc, tags, false, false, false, ContentType.Other, SafetyLevel.Safe, HiddenFromSearch.Visible);

            try
            {
                PhotoInfo info = f.PhotosGetInfo(photoId);

                Assert.That(info.Title, Is.EqualTo(title));
                Assert.That(info.Description, Is.EqualTo(desc));
                Assert.That(info.Tags, Has.Count.EqualTo(2));
                Assert.That(info.Tags[0].Raw, Is.EqualTo("testtag1"));
                Assert.That(info.Tags[1].Raw, Is.EqualTo("testtag2"));

                Assert.That(info.IsPublic, Is.False);
                Assert.That(info.IsFamily, Is.False);
                Assert.That(info.IsFriend, Is.False);

                SizeCollection sizes = f.PhotosGetSizes(photoId);

                string url = sizes[sizes.Count - 1].Source;
                using (WebClient client = new WebClient())
                {
                    byte[] downloadBytes = client.DownloadData(url);
                    string downloadBase64 = Convert.ToBase64String(downloadBytes);

                    Assert.That(downloadBase64, Is.EqualTo(TestData.TestImageBase64));
                }
            }
            finally
            {
                f.PhotosDelete(photoId);
            }
        }

        [Test]
        [Ignore("Method requires authentication")]
        public void DownloadAndUploadImage()
        {
            var photos = AuthInstance.PeopleGetPhotos(PhotoSearchExtras.Small320Url);

            var photo = photos.First();
            var url = photo.Small320Url;

            using var client = new WebClient();
            var data = client.DownloadData(url);

            using var ms = new MemoryStream(data) { Position = 0 };

            var photoId = AuthInstance.UploadPicture(ms, "test.jpg", "Test Photo", "Test Description", "", false, false, false, ContentType.Photo, SafetyLevel.Safe, HiddenFromSearch.Hidden);
            Assert.That(photoId, Is.Not.Null, "PhotoId should not be null");

            // Cleanup
            AuthInstance.PhotosDelete(photoId);
        }

        [Test]
        [Ignore("Method requires authentication")]
        public void ReplacePictureBasicTest()
        {
            Flickr f = AuthInstance;

            byte[] imageBytes = TestData.TestImageBytes;
            var s = new MemoryStream(imageBytes);
            s.Position = 0;

            string title = "Test Title";
            string desc = "Test Description\nSecond Line";
            string tags = "testtag1,testtag2";
            string photoId = f.UploadPicture(s, "Test.jpg", title, desc, tags, false, false, false, ContentType.Other, SafetyLevel.Safe, HiddenFromSearch.Visible);

            try
            {
                s.Position = 0;
                f.ReplacePicture(s, "Test.jpg", photoId);
            }
            finally
            {
                f.PhotosDelete(photoId);
            }
        }

        [Test]
        [Ignore("Method requires authentication")]
        public void UploadPictureFromUrl()
        {
            string url = "http://www.google.co.uk/intl/en_com/images/srpr/logo1w.png";
            Flickr f = AuthInstance;

            using (WebClient client = new WebClient())
            {
                using (Stream s = client.OpenRead(url))
                {
                    string photoId = f.UploadPicture(s, "google.png", "Google Image", "Google", "", false, false, false, ContentType.Photo, SafetyLevel.None, HiddenFromSearch.None);
                    f.PhotosDelete(photoId);
                }
            }
        }

        [Test, Ignore("Long running test")]
        public void UploadLargeVideoFromUrl()
        {
            string url = "http://www.sample-videos.com/video/mp4/720/big_buck_bunny_720p_50mb.mp4";
            Flickr f = AuthInstance;

            using (WebClient client = new WebClient())
            {
                using (Stream s = client.OpenRead(url))
                {
                    string photoId = f.UploadPicture(s, "bunny.mp4", "Big Buck Bunny", "Sample Video", "", false, false, false, ContentType.Photo, SafetyLevel.None, HiddenFromSearch.None);
                    f.PhotosDelete(photoId);
                }
            }
        }
        // 

        [Test]
        [Ignore("Large time consuming uploads")]
        public void UploadPictureVideoTests()
        {
            // Samples downloaded from http://support.apple.com/kb/HT1425
            // sample_mpeg2.m2v does not upload
            string[] filenames = { "sample_mpeg4.mp4", "sample_sorenson.mov", "sample_iTunes.mov", "sample_iPod.m4v", "sample.3gp", "sample_3GPP2.3g2" };
            // Copy files to this directory.
            string directory = @"Z:\Code Projects\FlickrNet\Samples\";

            foreach (string file in filenames)
            {
                try
                {
                    using (Stream s = new FileStream(Path.Combine(directory, file), FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        Flickr f = AuthInstance;
                        string photoId = f.UploadPicture(s, file, "Video Upload Test", file, "video, test", false, false, false, ContentType.Other, SafetyLevel.Safe, HiddenFromSearch.None);
                        f.PhotosDelete(photoId);
                    }
                }
                catch (Exception ex)
                {
                    Assert.Fail("Failed during upload of " + file + " with exception: " + ex.ToString());
                }
            }
        }
    }
}
