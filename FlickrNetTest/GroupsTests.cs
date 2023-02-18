
using NUnit.Framework;
using FlickrNet;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for GroupsBrowseTests
    /// </summary>
    [TestFixture]
    public class GroupsTests : BaseTest
    {
        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void GroupsBrowseBasicTest()
        {
            Flickr f = AuthInstance;
            GroupCategory cat = f.GroupsBrowse();

            Assert.That(cat, Is.Not.Null, "GroupCategory should not be null.");
            Assert.That(cat.CategoryName, Is.EqualTo("/"), "CategoryName should be '/'.");
            Assert.That(cat.Path, Is.EqualTo("/"), "Path should be '/'.");
            Assert.That(cat.PathIds, Is.EqualTo(""), "PathIds should be empty string.");
            Assert.That(cat.Subcategories, Is.Empty, "No sub categories should be returned.");
            Assert.That(cat.Groups, Is.Empty, "No groups should be returned.");
        }

        [Test]
        public void GroupsSearchBasicTest()
        {
            Flickr f = Instance;

            GroupSearchResultCollection results = f.GroupsSearch("Buses");

            Assert.That(results, Is.Not.Null, "GroupSearchResults should not be null.");
            Assert.That(results, Is.Not.Empty, "Count should not be zero.");
            Assert.That(results.Total, Is.Not.EqualTo(0), "Total should not be zero.");
            Assert.That(results.PerPage, Is.Not.EqualTo(0), "PerPage should not be zero.");
            Assert.That(results.Page, Is.EqualTo(1), "Page should be 1.");

            foreach (GroupSearchResult result in results)
            {
                Assert.That(result.GroupId, Is.Not.Null, "GroupId should not be null.");
                Assert.That(result.GroupName, Is.Not.Null, "GroupName should not be null.");
            }
        }

        [Test]
        public void GroupsGetInfoBasicTest()
        {
            Flickr f = Instance;

            GroupFullInfo info = f.GroupsGetInfo(TestData.GroupId);

            Assert.That(info, Is.Not.Null, "GroupFullInfo should not be null");
            Assert.That(info.GroupId, Is.EqualTo(TestData.GroupId));
            Assert.That(info.GroupName, Is.EqualTo("The Lake District UK"));

            Assert.That(info.IconServer, Is.EqualTo("5128"));
            Assert.That(info.IconFarm, Is.EqualTo("6"));

            Assert.That(info.GroupIconUrl, Is.EqualTo("https://farm6.staticflickr.com/5128/buddyicons/53837206@N00.jpg"));

            Assert.That(info.ThrottleInfo.Count, Is.EqualTo(2));
            Assert.That(info.ThrottleInfo.Mode, Is.EqualTo(GroupThrottleMode.PerDay));

            Assert.That(info.Restrictions.PhotosAccepted, Is.True, "PhotosAccepted should be true.");
            Assert.That(info.Restrictions.VideosAccepted, Is.False, "VideosAccepted should be false.");
        }

        [Test]
        public void GroupsGetInfoNoGroupIconTest()
        {
            string groupId = "562176@N20";
            Flickr f = Instance;

            GroupFullInfo info = f.GroupsGetInfo(groupId);

            Assert.That(info, Is.Not.Null, "GroupFullInfo should not be null");
            Assert.That(info.IconServer, Is.EqualTo("0"), "Icon Server should be zero");
            Assert.That(info.GroupIconUrl, Is.EqualTo("https://www.flickr.com/images/buddyicon.jpg"));

        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void GroupsMembersGetListBasicTest()
        {
            var ms = AuthInstance.GroupsMembersGetList(TestData.GroupId);

            Assert.That(ms, Is.Not.Null);
            Assert.That(ms, Is.Not.Empty, "Count should not be zero.");
            Assert.That(ms.Total, Is.Not.EqualTo(0), "Total should not be zero.");
            Assert.That(ms.Page, Is.EqualTo(1), "Page should be one.");
            Assert.That(ms.PerPage, Is.Not.EqualTo(0), "PerPage should not be zero.");
            Assert.That(ms.Pages, Is.Not.EqualTo(0), "Pages should not be zero.");

        }
    }
}
