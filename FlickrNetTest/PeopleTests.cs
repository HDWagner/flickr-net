using System;

using NUnit.Framework;
using FlickrNet;
using Shouldly;
using FlickrNet.Classes;
using FlickrNet.Exceptions;
using FlickrNetTest.TestUtilities;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for PeopleTests
    /// </summary>
    [TestFixture]
    public class PeopleTests : BaseTest
    {
        [Test]
        public void PeopleGetPhotosOfBasicTest()
        {
            PeoplePhotoCollection p = Instance.PeopleGetPhotosOf(TestData.TestUserId);

            Assert.That(p, Is.Not.Null, "PeoplePhotos should not be null.");
            Assert.That(p, Is.Not.Empty, "PeoplePhotos.Count should be greater than zero.");
            Assert.That(p.PerPage, Is.GreaterThanOrEqualTo(p.Count), "PerPage should be the same or greater than the number of photos returned.");
        }

        [Test]
        public void PeopleGetPhotosOfAuthRequired()
        {
            Should.Throw<SignatureRequiredException>(() => Instance.PeopleGetPhotosOf());
        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void PeopleGetPhotosOfMe()
        {
            PeoplePhotoCollection p = AuthInstance.PeopleGetPhotosOf();

            Assert.That(p, Is.Not.Null, "PeoplePhotos should not be null.");
            Assert.That(p, Is.Not.Empty, "PeoplePhotos.Count should be greater than zero.");
            Assert.That(p.PerPage, Is.GreaterThanOrEqualTo(p.Count), "PerPage should be the same or greater than the number of photos returned.");
        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void PeopleGetPhotosBasicTest()
        {
            PhotoCollection photos = AuthInstance.PeopleGetPhotos();

            Assert.That(photos, Is.Not.Null);
            Assert.That(photos, Is.Not.Empty, "Count should not be zero.");
            Assert.That(photos.Total, Is.GreaterThan(1000), "Total should be greater than 1000.");
        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void PeopleGetPhotosFullParamTest()
        {
            PhotoCollection photos = AuthInstance.PeopleGetPhotos(TestData.TestUserId, SafetyLevel.Safe, new DateTime(2010, 1, 1),
                                                       new DateTime(2012, 1, 1), new DateTime(2010, 1, 1),
                                                       new DateTime(2012, 1, 1), ContentTypeSearch.All,
                                                       PrivacyFilter.PublicPhotos, PhotoSearchExtras.All, 1, 20);

            Assert.That(photos, Is.Not.Null);
            Assert.That(photos, Has.Count.EqualTo(20), "Count should be twenty.");
        }

        [Test]
        [Ignore("Method requires authentication")]
        public void PeopleGetInfoBasicTestUnauth()
        {
            Flickr f = Instance;
            Person p = f.PeopleGetInfo(TestData.TestUserId);

            Assert.That(p.UserName, Is.EqualTo("Sam Judson"));
            Assert.That(p.RealName, Is.EqualTo("Sam Judson"));
            Assert.That(p.PathAlias, Is.EqualTo("samjudson"));
            Assert.That(p.IsPro, Is.True, "IsPro should be true.");
            Assert.That(p.Location, Is.EqualTo("Newcastle, UK"));
            Assert.That(p.TimeZoneOffset, Is.EqualTo("+00:00"));
            Assert.That(p.TimeZoneLabel, Is.EqualTo("GMT: Dublin, Edinburgh, Lisbon, London"));
            Assert.That(p.Description, Is.Not.Null, "Description should not be null.");
            Assert.That(p.Description, Is.Not.Empty, "Description should not be empty");
        }

        [Test]
        [Ignore("Method requires authentication")]
        public void PeopleGetInfoGenderNoAuthTest()
        {
            Flickr f = Instance;
            Person p = f.PeopleGetInfo("10973297@N00");

            Assert.That(p, Is.Not.Null, "Person object should be returned");
            Assert.That(p.Gender, Is.Null, "Gender should be null as not authenticated.");

            Assert.That(p.IsReverseContact, Is.Null, "IsReverseContact should not be null.");
            Assert.That(p.IsContact, Is.Null, "IsContact should be null.");
            Assert.That(p.IsIgnored, Is.Null, "IsIgnored should be null.");
            Assert.That(p.IsFriend, Is.Null, "IsFriend should be null.");
        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void PeopleGetInfoGenderTest()
        {
            Flickr f = AuthInstance;
            Person p = f.PeopleGetInfo("10973297@N00");

            Assert.That(p, Is.Not.Null, "Person object should be returned");
            Assert.That(p.Gender, Is.EqualTo("F"), "Gender of M should be returned");

            Assert.That(p.IsReverseContact, Is.Not.Null, "IsReverseContact should not be null.");
            Assert.That(p.IsContact, Is.Not.Null, "IsContact should not be null.");
            Assert.That(p.IsIgnored, Is.Not.Null, "IsIgnored should not be null.");
            Assert.That(p.IsFriend, Is.Not.Null, "IsFriend should not be null.");

            Assert.That(p.PhotosSummary, Is.Not.Null, "PhotosSummary should not be null.");
        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void PeopleGetInfoBuddyIconTest()
        {
            Flickr f = AuthInstance;
            Person p = f.PeopleGetInfo(TestData.TestUserId);
            Assert.That(p.BuddyIconUrl, Does.Contain(".staticflickr.com/"), "Buddy icon doesn't contain correct details.");
        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void PeopleGetInfoSelfTest()
        {
            Flickr f = AuthInstance;

            Person p = f.PeopleGetInfo(TestData.TestUserId);

            Assert.That(p.MailboxSha1Hash, Is.Not.Null, "MailboxSha1Hash should not be null.");
            Assert.That(p.PhotosSummary, Is.Not.Null, "PhotosSummary should not be null.");
            Assert.That(p.PhotosSummary.Views, Is.Not.EqualTo(0), "PhotosSummary.Views should not be zero.");

        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void PeopleGetGroupsTest()
        {
            Flickr f = AuthInstance;

            var groups = f.PeopleGetGroups(TestData.TestUserId);

            Assert.That(groups, Is.Not.Null);
            Assert.That(groups, Is.Not.Empty);
        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void PeopleGetLimitsTest()
        {
            var f = AuthInstance;

            var limits = f.PeopleGetLimits();

            Assert.That(limits, Is.Not.Null);

            Assert.That(limits.MaximumDisplayPixels, Is.EqualTo(0));
            Assert.That(limits.MaximumPhotoUpload, Is.EqualTo(209715200));
            Assert.That(limits.MaximumVideoUpload, Is.EqualTo(1073741824));
            Assert.That(limits.MaximumVideoDuration, Is.EqualTo(180));
            
        }

        [Test]
        [Category("AccessTokenRequired")]
        public void PeopleFindByUsername()
        {
            Flickr f = AuthInstance;

            FoundUser user = f.PeopleFindByUserName("Sam Judson");

            Assert.That(user.UserId, Is.EqualTo("41888973@N00"));
            Assert.That(user.UserName, Is.EqualTo("Sam Judson"));
        }

        [Test]
        [Category("AccessTokenRequired")]
        public void PeopleFindByEmail()
        {
            Flickr f = AuthInstance;

            FoundUser user = f.PeopleFindByEmail("samjudson@gmail.com");

            Assert.That(user.UserId, Is.EqualTo("41888973@N00"));
            Assert.That(user.UserName, Is.EqualTo("Sam Judson"));
        }

        [Test]
        public void PeopleGetPublicPhotosBasicTest()
        {
            var f = Instance;
            var photos = f.PeopleGetPublicPhotos(TestData.TestUserId, 1, 100, SafetyLevel.None, PhotoSearchExtras.OriginalDimensions);

            Assert.That(photos, Is.Not.Null);
            Assert.That(photos, Is.Not.Empty);

            foreach (var photo in photos)
            {
                Assert.That(photo.PhotoId, Is.Not.Null, "PhotoId should not be null.");
                Assert.That(photo.UserId, Is.EqualTo(TestData.TestUserId));
                Assert.That(photo.OriginalWidth, Is.Not.EqualTo(0), "OriginalWidth should not be zero.");
                Assert.That(photo.OriginalHeight, Is.Not.EqualTo(0), "OriginalHeight should not be zero.");
            }
        }

        [Test]
        [Category("AccessTokenRequired")]
        public void PeopleGetPublicGroupsBasicTest()
        {
            Flickr f = AuthInstance;

            GroupInfoCollection groups = f.PeopleGetPublicGroups(TestData.TestUserId);

            Assert.That(groups, Is.Not.Empty, "PublicGroupInfoCollection.Count should not be zero.");

            foreach(GroupInfo group in groups)
            {
                Assert.That(group.GroupId, Is.Not.Null, "GroupId should not be null.");
                Assert.That(group.GroupName, Is.Not.Null, "GroupName should not be null.");
            }
        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void PeopleGetUploadStatusBasicTest()
        {
            var u = AuthInstance.PeopleGetUploadStatus();

            Assert.That(u, Is.Not.Null);
            Assert.That(u.UserId, Is.Not.Null);
            Assert.That(u.UserName, Is.Not.Null);
            Assert.That(u.FileSizeMax, Is.Not.EqualTo(0));
        }

        [Test]
        [Ignore("Method requires authentication")]
        public void PeopleGetInfoBlankDate()
        {
            var p = Instance.PeopleGetInfo("18387778@N00");
        }

        [Test]
        [Ignore("Method requires authentication")]
        public void PeopleGetInfoZeroDate()
        {
            var p = Instance.PeopleGetInfo("47963952@N03");
        }

        [Test]
        [Ignore("Method requires authentication")]
        public void PeopleGetInfoInternationalCharacters()
        {
            var p = Instance.PeopleGetInfo("24754141@N08");

            Assert.That(p.UserId, Is.EqualTo("24754141@N08"), "UserId should match.");
            Assert.That(p.RealName, Is.EqualTo("Pierre Hsiu 脩丕政"), "RealName should match");
        }
    }
}
