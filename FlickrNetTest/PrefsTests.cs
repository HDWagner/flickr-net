
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

            Assert.IsNotNull(s);
            Assert.AreNotEqual(ContentType.None, s);
        }

        [Test]
        [Ignore("Method requires authentication")]
        public void PrefsGetGeoPermsTest()
        {
            var p = AuthInstance.PrefsGetGeoPerms();

            Assert.IsNotNull(p);
            Assert.IsTrue(p.ImportGeoExif);
            Assert.AreEqual(GeoPermissionType.Public, p.GeoPermissions);
        }

        [Test]
        [Ignore("Method requires authentication")]
        public void PrefsGetHiddenTest()
        {
            var s = AuthInstance.PrefsGetHidden();

            Assert.IsNotNull(s);
            Assert.AreNotEqual(HiddenFromSearch.None, s);
        }

        [Test]
        [Ignore("Method requires authentication")]
        public void PrefsGetPrivacyTest()
        {
            var p = AuthInstance.PrefsGetPrivacy();

            Assert.IsNotNull(p);
            Assert.AreEqual(PrivacyFilter.PublicPhotos, p);
        }

        [Test]
        [Ignore("Method requires authentication")]
        public void PrefsGetSafetyLevelTest()
        {
            var s = AuthInstance.PrefsGetSafetyLevel();

            Assert.IsNotNull(s);
            Assert.AreEqual(SafetyLevel.Safe, s);
        }


    }
}
