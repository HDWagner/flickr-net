﻿
using FlickrNet;
using NUnit.Framework;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for PhotosSearchAsyncTests
    /// </summary>
    [TestFixture]
    public class PhotosSearchAsyncTests : BaseTest
    {

        [Test]
        [Ignore("Method requires authentication")]
        public void PhotosSearchAsyncBasicTest()
        {
            var o = new PhotoSearchOptions();
            o.Tags = "microsoft";

            var w = new AsyncSubject<FlickrResult<PhotoCollection>>();

            Instance.PhotosSearchAsync(o, r => { w.OnNext(r); w.OnCompleted(); });
            var result = w.Next().First();

            Assert.IsTrue(result.Result.Total > 0);

        }

        [Test]
        [Category("AccessTokenRequired")]
        public void PhotosAddTagTest()
        {
            string photoId = "4499284951";
            string tag = "testx";

            var w = new AsyncSubject<FlickrResult<NoResponse>>();

            AuthInstance.PhotosAddTagsAsync(photoId, tag, r => { w.OnNext(r); w.OnCompleted(); });

            var result = w.Next().First();
            Assert.IsNotNull(result.Result);
        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void PhotosSearchAsyncShowerTest()
        {
            var o = new PhotoSearchOptions();
            o.UserId = "78507951@N00";
            o.Tags = "shower";
            o.SortOrder = PhotoSearchSortOrder.DatePostedDescending;
            o.PerPage = 1000;
            o.TagMode = TagMode.AllTags;
            o.Extras = PhotoSearchExtras.All;

            var w = new AsyncSubject<FlickrResult<PhotoCollection>>();

            AuthInstance.PhotosSearchAsync(o, r => { w.OnNext(r); w.OnCompleted(); });
            var result = w.Next().First();

            Assert.IsTrue(result.Result.Total > 0);
        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void PhotosGetContactsPhotosAsyncTest()
        {
            var w = new AsyncSubject<FlickrResult<PhotoCollection>>();
            AuthInstance.PhotosGetContactsPhotosAsync(50, true, true, true, PhotoSearchExtras.All, r => { w.OnNext(r); w.OnCompleted(); });
            var result = w.Next().First();

            Assert.IsFalse(result.HasError);
            Assert.IsNotNull(result.Result);

            Assert.IsTrue(result.Result.Count > 0, "Should return some photos.");

        }


    }
}
