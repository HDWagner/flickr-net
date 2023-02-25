using System;
using System.Linq;
using NUnit.Framework;
using FlickrNet;
using System.Reactive.Subjects;
using System.Reactive.Linq;
using Shouldly;
using FlickrNet.Classes;
using FlickrNetTest.TestUtilities;

namespace FlickrNetTest.Async
{
    [TestFixture]
    public class PhotosAsyncTests : BaseTest
    {
        [Test]
        public void PhotosSearchRussianAsync()
        {
            var o = new PhotoSearchOptions();
            o.Extras = PhotoSearchExtras.Tags;
            o.Tags = "фото";
            o.PerPage = 100;

            Flickr f = Instance;

            var w = new AsyncSubject<FlickrResult<PhotoCollection>>();
            f.PhotosSearchAsync(o, r => { w.OnNext(r); w.OnCompleted(); });
            var result = w.Next().First();

            Assert.That(result.HasError, Is.False);
            Assert.That(result.Result, Is.Not.Null);

            result.Result.Count.ShouldBeGreaterThan(0);

            var photos = result.Result;
            foreach (var photo in photos)
            {
                Console.WriteLine(photo.Title + " = " + string.Join(",", photo.Tags));
            }

        }
        [Test]
        public void PhotosGetContactsPublicPhotosAsyncTest()
        {
            Flickr f = Instance;

            var w = new AsyncSubject<FlickrResult<PhotoCollection>>();
            f.PhotosGetContactsPublicPhotosAsync(TestData.TestUserId, 5, true, true, true, PhotoSearchExtras.All, r => { w.OnNext(r); w.OnCompleted(); });
            var result = w.Next().First();

            Assert.That(result.HasError, Is.False);
            Assert.That(result.Result, Is.Not.Null);

            Assert.That(result.Result, Is.Not.Empty, "Should return some photos.");
        }

        [Ignore("Method requires authentication")]
        [Test]
        [Category("AccessTokenRequired")]
        public void PhotosGetCountsAsyncTest()
        {
            DateTime date1 = DateTime.Today.AddMonths(-12);
            DateTime date2 = DateTime.Today.AddMonths(-6);
            DateTime date3 = DateTime.Today;

            DateTime[] uploadDates = { date1, date2, date3 };

            Flickr f = AuthInstance;

            var w = new AsyncSubject<FlickrResult<PhotoCountCollection>>();
            f.PhotosGetCountsAsync(uploadDates, false, r => { w.OnNext(r); w.OnCompleted(); });
            var result = w.Next().First();

            Assert.That(result.HasError, Is.False);

            var counts = result.Result;

            Assert.That(counts, Has.Count.EqualTo(2), "Should be two counts returned.");

            var count1 = counts[0];

            Assert.That(count1.FromDate, Is.EqualTo(date1));
            Assert.That(count1.ToDate, Is.EqualTo(date2));

            var count2 = counts[1];
            Assert.That(count2.FromDate, Is.EqualTo(date2));
            Assert.That(count2.ToDate, Is.EqualTo(date3));

        }

        [Test]
        public void PhotosGetExifAsyncTest()
        {
            Flickr f = Instance;

            var w = new AsyncSubject<FlickrResult<ExifTagCollection>>();
            f.PhotosGetExifAsync(TestData.PhotoId, r => { w.OnNext(r); w.OnCompleted(); });
            var result = w.Next().First();

            Assert.That(result.HasError, Is.False);

        }

        [Test]
        public void PhotosGetRecentAsyncTest()
        {
            Flickr f = Instance;
            var w = new AsyncSubject<FlickrResult<PhotoCollection>>();
            f.PhotosGetRecentAsync(1, 50, PhotoSearchExtras.All, r => { w.OnNext(r); w.OnCompleted(); });
            var result = w.Next().First();

            Assert.That(result.HasError, Is.False);
            Assert.That(result.Result, Is.Not.Null);

            Assert.That(result.Result, Is.Not.Empty, "Should return some photos.");

        }


    }
}
