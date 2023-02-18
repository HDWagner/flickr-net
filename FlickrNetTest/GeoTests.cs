
using NUnit.Framework;
using FlickrNet;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for GeoTests
    /// </summary>
    [TestFixture]
    public class GeoTests : BaseTest
    {
       
        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void PhotosGeoGetPermsBasicTest()
        {
            GeoPermissions perms = AuthInstance.PhotosGeoGetPerms(TestData.PhotoId);

            Assert.That(perms, Is.Not.Null);
            Assert.That(perms.PhotoId, Is.EqualTo(TestData.PhotoId));
            Assert.That(perms.IsPublic, Is.True, "IsPublic should be true.");
        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void PhotosGetWithGeoDataBasicTest()
        {
            PhotoCollection photos = AuthInstance.PhotosGetWithGeoData();

            Assert.That(photos, Is.Not.Null);
            Assert.That(photos, Is.Not.Empty);
            Assert.That(photos.Total, Is.Not.EqualTo(0));
            Assert.That(photos.Page, Is.EqualTo(1));
            Assert.That(photos.PerPage, Is.Not.EqualTo(0));
            Assert.That(photos.Pages, Is.Not.EqualTo(0));

            foreach (var p in photos)
            {
                Assert.That(p.PhotoId, Is.Not.Null);
            }

        }
    }
}
