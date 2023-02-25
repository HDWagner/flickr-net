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
            var f = AuthInstance;

            var w = new AsyncSubject<FlickrResult<string>>();

            var imageBytes = TestData.TestImageBytes;
            var s = new MemoryStream(imageBytes);
            s.Position = 0;

            string title = "Test Title";
            string desc = "Test Description\nSecond Line";
            string tags = "testtag1,testtag2";

            f.UploadPictureAsync(s, "Test.jpg", title, desc, tags, false, false, false, ContentType.Other, SafetyLevel.Safe, HiddenFromSearch.Visible,
                r => { w.OnNext(r); w.OnCompleted(); });

            var result = w.Next().First();

            Assert.That(result.Result, Is.Not.Null);
            Console.WriteLine(result.Result);

            // Clean up photo
            f.PhotosDelete(result.Result);
        }

        [Test]
        [Ignore("Method requires authentication")]
        public async Task UploadPictureBasicTest()
        {
            var f = AuthInstance;

            f.OnUploadProgress += (sender, args) =>
            {
                // Do nothing
            };

            var imageBytes = TestData.TestImageBytes;
            var s = new MemoryStream(imageBytes);
            s.Position = 0;

            var title = "Test Title";
            var desc = "Test Description\nSecond Line";
            var tags = "testtag1,testtag2";
            var photoId = f.UploadPicture(s, "Test.jpg", title, desc, tags, false, false, false, ContentType.Other, SafetyLevel.Safe, HiddenFromSearch.Visible);
            Assert.That(photoId, Is.Not.Null, "PhotoId should not be null");

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

                Assert.That(sizes[sizes.Count - 1].Source, Is.Not.Null);

                string url = sizes[sizes.Count - 1].Source;

                using var httpClient = new HttpClient();
                var downloadBytes = await httpClient.GetByteArrayAsync(url);

                string downloadBase64 = Convert.ToBase64String(downloadBytes);
                Assert.That(downloadBase64, Is.EqualTo(TestData.TestImageBase64));
            }
            finally
            {
                f.PhotosDelete(photoId);
            }
        }

        [Test]
        [Ignore("Method requires authentication")]
        public async Task DownloadAndUploadImage()
        {
            var photos = AuthInstance.PeopleGetPhotos(PhotoSearchExtras.Small320Url);

            var photo = photos.First();
            var url = photo.Small320Url;

            using var httpClient = new HttpClient();
            var data = await httpClient.GetByteArrayAsync(url);

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
            var f = AuthInstance;

            var imageBytes = TestData.TestImageBytes;
            var s = new MemoryStream(imageBytes);
            s.Position = 0;

            var title = "Test Title";
            var desc = "Test Description\nSecond Line";
            var tags = "testtag1,testtag2";
            var photoId = f.UploadPicture(s, "Test.jpg", title, desc, tags, false, false, false, ContentType.Other, SafetyLevel.Safe, HiddenFromSearch.Visible);
            Assert.That(photoId, Is.Not.Null, "PhotoId should not be null");

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
        public async Task UploadPictureFromUrl()
        {
            var url = "http://www.google.co.uk/intl/en_com/images/srpr/logo1w.png";
            var f = AuthInstance;

            using var httpClient = new HttpClient();
            var s = await httpClient.GetStreamAsync(url);

            var photoId = f.UploadPicture(s, "google.png", "Google Image", "Google", "", false, false, false, ContentType.Photo, SafetyLevel.None, HiddenFromSearch.None);
            Assert.That(photoId, Is.Not.Null, "PhotoId should not be null");
            f.PhotosDelete(photoId);
        }

        [Test, Ignore("Long running test")]
        public async Task UploadLargeVideoFromUrl()
        {
            var url = "http://www.sample-videos.com/video/mp4/720/big_buck_bunny_720p_50mb.mp4";
            var f = AuthInstance;

            using var httpClient = new HttpClient();
            using var s = await httpClient.GetStreamAsync(url);

            var photoId = f.UploadPicture(s, "bunny.mp4", "Big Buck Bunny", "Sample Video", "", false, false, false, ContentType.Photo, SafetyLevel.None, HiddenFromSearch.None);
            Assert.That(photoId, Is.Not.Null, "PhotoId should not be null");
            f.PhotosDelete(photoId);
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
            var directory = @"Z:\Code Projects\FlickrNet\Samples\";

            foreach (string file in filenames)
            {
                try
                {
                    using (Stream s = new FileStream(Path.Combine(directory, file), FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        var f = AuthInstance;
                        var photoId = f.UploadPicture(s, file, "Video Upload Test", file, "video, test", false, false, false, ContentType.Other, SafetyLevel.Safe, HiddenFromSearch.None);
                        Assert.That(photoId, Is.Not.Null, "PhotoId should not be null");
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
