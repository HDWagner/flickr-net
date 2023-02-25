using FlickrNet.Classes;
using FlickrNet.Exceptions;
using FlickrNetTest.TestUtilities;
using NUnit.Framework;
using Shouldly;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for GroupsPoolsGetGroupsTests
    /// </summary>
    [TestFixture]
    public class GroupsPoolsTests : BaseTest
    {
        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void GroupsPoolsAddBasicTest()
        {
            var f = AuthInstance;

            var imageBytes = TestData.TestImageBytes;
            var s = new MemoryStream(imageBytes);
            s.Position = 0;

            var title = "Test Title";
            var desc = "Test Description\nSecond Line";
            var tags = "testtag1,testtag2";
            var photoId = f.UploadPicture(s, "Test.jpg", title, desc, tags, false, false, false, ContentType.Other, SafetyLevel.Safe, HiddenFromSearch.Visible);

            Assert.That(photoId, Is.Not.Null, "photoId should not be null");

            try
            {
                f.GroupsPoolsAdd(photoId, TestData.FlickrNetTestGroupId);
            }
            finally
            {
                f.PhotosDelete(photoId);
            }
        }

        [Test]
        [Category("AccessTokenRequired")]
        public void GroupsPoolsAddNotAuthTestTest()
        {
            var photoId = "12345";

            Should.Throw<SignatureRequiredException>(() => Instance.GroupsPoolsAdd(photoId, TestData.FlickrNetTestGroupId));
        }

        [Test]
        public void GroupsPoolGetPhotosFullParamTest()
        {
            var f = Instance;

            var photos = f.GroupsPoolsGetPhotos(TestData.GroupId, null, TestData.TestUserId, PhotoSearchExtras.All, 1, 20);

            Assert.That(photos, Is.Not.Null, "Photos should not be null");
            Assert.That(photos, Is.Not.Empty, "Should be more than 0 photos returned");
            Assert.That(photos.PerPage, Is.EqualTo(20));
            Assert.That(photos.Page, Is.EqualTo(1));

            foreach (Photo p in photos)
            {
                Assert.That(p.DateAddedToGroup, Is.Not.EqualTo(default(DateTime)), "DateAddedToGroup should not be default value");
                Assert.That(p.DateAddedToGroup, Is.LessThan(DateTime.Now), "DateAddedToGroup should be in the past");
            }

        }

        [Test]
        public void GroupsPoolGetPhotosDateAddedTest()
        {
            var f = Instance;

            var photos = f.GroupsPoolsGetPhotos(TestData.GroupId);

            Assert.That(photos, Is.Not.Null, "Photos should not be null");
            Assert.That(photos, Is.Not.Empty, "Should be more than 0 photos returned");

            foreach (Photo p in photos)
            {
                Assert.That(p.DateAddedToGroup, Is.Not.EqualTo(default(DateTime)), "DateAddedToGroup should not be default value");
                Assert.That(p.DateAddedToGroup, Is.LessThan(DateTime.Now), "DateAddedToGroup should be in the past");
            }

        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void GroupsPoolsGetGroupsBasicTest()
        {
            var groups = AuthInstance.GroupsPoolsGetGroups();

            Assert.That(groups, Is.Not.Null, "MemberGroupInfoCollection should not be null.");
            Assert.That(groups, Is.Not.Empty, "MemberGroupInfoCollection.Count should not be zero.");
            Assert.That(groups, Has.Count.GreaterThan(1), "Count should be greater than one.");

            Assert.That(groups.PerPage, Is.EqualTo(400), "PerPage should be 400.");
            Assert.That(groups.Page, Is.EqualTo(1), "Page should be 1.");
            Assert.That(groups.Total, Is.GreaterThan(1), "Total chould be greater than one");
        }
    }
}
