
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
            FoundUser user = AuthInstance.UrlsLookupUser("https://www.flickr.com/photos/samjudson");

            Assert.That(user.UserId, Is.EqualTo("41888973@N00"));
            Assert.That(user.UserName, Is.EqualTo("Sam Judson"));
        }

        [Test]
        [Category("AccessTokenRequired")]
        public void UrlsLookupGroup()
        {
            string groupUrl = "https://www.flickr.com/groups/angels_of_the_north/";

            string groupId = AuthInstance.UrlsLookupGroup(groupUrl);

            Assert.That(groupId, Is.EqualTo("71585219@N00"));
        }

        [Test]
        public void UrlsLookupGalleryTest()
        {
            string galleryUrl = "https://www.flickr.com/photos/samjudson/galleries/72157622589312064";

            Flickr f = Instance;

            Gallery gallery = f.UrlsLookupGallery(galleryUrl);

            Assert.That(gallery.GalleryUrl, Is.EqualTo(galleryUrl));

        }

        [Test]
        public void UrlsGetUserPhotosTest()
        {
            string url = Instance.UrlsGetUserPhotos(TestData.TestUserId);

            Assert.That(url, Is.EqualTo("https://www.flickr.com/photos/samjudson/"));
        }

        [Test]
        public void UrlsGetUserProfileTest()
        {
            string url = Instance.UrlsGetUserProfile(TestData.TestUserId);

            Assert.That(url, Is.EqualTo("https://www.flickr.com/people/samjudson/"));
        }

        [Test]
        public void UrlsGetGroupTest()
        {
            string url = Instance.UrlsGetGroup(TestData.GroupId);

            Assert.That(url, Is.EqualTo("https://www.flickr.com/groups/lakedistrict/"));
        }



    }
}
