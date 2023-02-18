
using NUnit.Framework;
using FlickrNet;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for PhotosetCommentsGetListTests
    /// </summary>
    [TestFixture]
    public class PhotosetCommentsGetListTests : BaseTest
    {
       [Test]
        public void PhotosetsCommentsGetListBasicTest()
        {
            Flickr f = Instance;

            PhotosetCommentCollection comments = f.PhotosetsCommentsGetList("1335934");

            Assert.That(comments, Is.Not.Null);

            Assert.That(comments, Has.Count.EqualTo(2));

            Assert.That(comments[0].AuthorUserName, Is.EqualTo("Superchou"));
            Assert.That(comments[0].CommentHtml, Is.EqualTo("LOL... I had no idea this set existed... what a great afternoon we had :)"));
        }
    }
}
