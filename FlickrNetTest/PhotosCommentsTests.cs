using System;

using NUnit.Framework;
using FlickrNet;
using FlickrNet.Classes;
using FlickrNetTest.TestUtilities;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for PhotosCommentsGetListTests
    /// </summary>
    [TestFixture]
    public class PhotosCommentsTests : BaseTest
    {
        [Test]
        public void PhotosCommentsGetListBasicTest()
        {
            Flickr f = Instance;

            PhotoCommentCollection comments = f.PhotosCommentsGetList("3546335765");

            Assert.That(comments, Is.Not.Null, "PhotoCommentCollection should not be null.");

            Assert.That(comments, Has.Count.EqualTo(1), "Count should be one.");

            Assert.That(comments[0].AuthorUserName, Is.EqualTo("ian1001"));
            Assert.That(comments[0].CommentHtml, Is.EqualTo("Sam lucky you NYCis so cool can't wait to go again it's my fav city along with San fran"));
        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void PhotosCommentsGetRecentForContactsBasicTest()
        {
            Flickr f = AuthInstance;

            var photos = f.PhotosCommentsGetRecentForContacts();
            Assert.That(photos, Is.Not.Null, "PhotoCollection should not be null.");
        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void PhotosCommentsGetRecentForContactsFullParamTest()
        {
            Flickr f = AuthInstance;

            var photos = f.PhotosCommentsGetRecentForContacts(DateTime.Now.AddHours(-1), PhotoSearchExtras.All, 1, 20);
            Assert.That(photos, Is.Not.Null, "PhotoCollection should not be null.");
            Assert.That(photos.PerPage, Is.EqualTo(20));
        }
    }
}
