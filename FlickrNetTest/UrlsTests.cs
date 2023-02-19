
using NUnit.Framework;
using FlickrNet;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for UrlsTests
    /// </summary>
    [TestFixture]
    public class UrlsTests : BaseTest
    {
        [Test]
        [Category("AccessTokenRequired")]
        public void UrlsLookupUserTest1()
        {
            var user = AuthInstance.UrlsLookupUser("https://www.flickr.com/photos/samjudson");

            Assert.That(user.UserId, Is.EqualTo("41888973@N00"));
            Assert.That(user.UserName, Is.EqualTo("Sam Judson"));
        }

        [Test]
        [Category("AccessTokenRequired")]
        public void UrlsLookupGroup()
        {
            var groupUrl = "https://www.flickr.com/groups/angels_of_the_north/";

            var groupId = AuthInstance.UrlsLookupGroup(groupUrl);

            Assert.That(groupId, Is.EqualTo("71585219@N00"));
        }

        [Test]
        public void UrlsLookupGalleryTest()
        {
            var galleryUrl = "https://www.flickr.com/photos/samjudson/galleries/72157622589312064";

            var f = Instance;

            var gallery = f.UrlsLookupGallery(galleryUrl);

            Assert.That(gallery.GalleryUrl, Is.EqualTo(galleryUrl));

        }

        [Test]
        public void UrlsGetUserPhotosTest()
        {
            var url = Instance.UrlsGetUserPhotos(TestData.TestUserId);

            Assert.That(url, Is.EqualTo("https://www.flickr.com/photos/samjudson/"));
        }

        [Test]
        public void UrlsGetUserProfileTest()
        {
            var url = Instance.UrlsGetUserProfile(TestData.TestUserId);

            Assert.That(url, Is.EqualTo("https://www.flickr.com/people/samjudson/"));
        }

        [Test]
        public void UrlsGetGroupTest()
        {
            var url = Instance.UrlsGetGroup(TestData.GroupId);

            Assert.That(url, Is.EqualTo("https://www.flickr.com/groups/lakedistrict/"));
        }



    }
}
