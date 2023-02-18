using NUnit.Framework;
using FlickrNet;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for PrefsTest
    /// </summary>
    [TestFixture]
    [Category("AccessTokenRequired")]
    public class PrefsTests : BaseTest
    {
        [Test]
        [Ignore("Method requires authentication")]
        public void PrefsGetContentTypeTest()
        {
            var s = AuthInstance.PrefsGetContentType();

            //Assert.That(s, Is.Not.Null);
            Assert.That(s, Is.Not.EqualTo(ContentType.None));
        }

        [Test]
        [Ignore("Method requires authentication")]
        public void PrefsGetGeoPermsTest()
        {
            var p = AuthInstance.PrefsGetGeoPerms();

            Assert.That(p, Is.Not.Null);
            Assert.That(p.ImportGeoExif, Is.True);
            Assert.That(p.GeoPermissions, Is.EqualTo(GeoPermissionType.Public));
        }

        [Test]
        [Ignore("Method requires authentication")]
        public void PrefsGetHiddenTest()
        {
            var s = AuthInstance.PrefsGetHidden();

            //Assert.That(s, Is.Not.Null);
            Assert.That(s, Is.Not.EqualTo(HiddenFromSearch.None));
        }

        [Test]
        [Ignore("Method requires authentication")]
        public void PrefsGetPrivacyTest()
        {
            var p = AuthInstance.PrefsGetPrivacy();

            //Assert.That(p, Is.Not.Null);
            Assert.That(p, Is.EqualTo(PrivacyFilter.PublicPhotos));
        }

        [Test]
        [Ignore("Method requires authentication")]
        public void PrefsGetSafetyLevelTest()
        {
            var s = AuthInstance.PrefsGetSafetyLevel();

            //Assert.That(s, Is.Not.Null);
            Assert.That(s, Is.EqualTo(SafetyLevel.Safe));
        }


    }
}
