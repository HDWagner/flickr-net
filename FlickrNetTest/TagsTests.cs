using FlickrNet;
using NUnit.Framework;
using Shouldly;

namespace FlickrNetTest
{
    [TestFixture]
    public class TagsTests : BaseTest
    {
        public TagsTests()
        {
            Flickr.CacheDisabled = true;
        }

        [Test]
        public void TagsGetListUserRawAuthenticationTest()
        {
            Flickr f = Instance;
            Should.Throw<SignatureRequiredException>(() => f.TagsGetListUserRaw());
        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void TagsGetListUserRawBasicTest()
        {
            var tags = AuthInstance.TagsGetListUserRaw();

            Assert.That(tags, Is.Not.Empty, "There should be one or more raw tags returned");

            foreach (RawTag tag in tags)
            {
                Assert.That(tag.CleanTag, Is.Not.Null, "Clean tag should not be null");
                Assert.That(tag.CleanTag, Is.Not.Empty, "Clean tag should not be empty string");
                Assert.That(tag.RawTags, Is.Not.Empty, "Should be one or more raw tag for each clean tag");
            }
        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void TagsGetListUserPopularBasicTest()
        {
            TagCollection tags = AuthInstance.TagsGetListUserPopular();

            Assert.That(tags, Is.Not.Null, "TagCollection should not be null.");
            Assert.That(tags, Is.Not.Empty, "TagCollection.Count should not be zero.");

            foreach (Tag tag in tags)
            {
                Assert.That(tag.TagName, Is.Not.Null, "Tag.TagName should not be null.");
                Assert.That(tag.Count, Is.Not.EqualTo(0), "Tag.Count should not be zero.");
            }
        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void TagsGetListUserBasicTest()
        {
            TagCollection tags = AuthInstance.TagsGetListUser();

            Assert.That(tags, Is.Not.Null, "TagCollection should not be null.");
            Assert.That(tags, Is.Not.Empty, "TagCollection.Count should not be zero.");

            foreach (Tag tag in tags)
            {
                Assert.That(tag.TagName, Is.Not.Null, "Tag.TagName should not be null.");
                Assert.That(tag.Count, Is.EqualTo(0), "Tag.Count should be zero. Not ser for this method.");
            }
        }

        [Test]
        public void TagsGetListPhotoBasicTest()
        {
            var tags = Instance.TagsGetListPhoto(TestData.PhotoId);

            Assert.That(tags, Is.Not.Null, "tags should not be null.");
            Assert.That(tags, Is.Not.Empty, "Length should be greater than zero.");

            foreach (var tag in tags)
            {
                Assert.That(tag.TagId, Is.Not.Null, "TagId should not be null.");
                Assert.That(tag.TagText, Is.Not.Null, "TagText should not be null.");
                Assert.That(tag.Raw, Is.Not.Null, "Raw should not be null.");
                Assert.That(tag.IsMachineTag, Is.Not.Null, "IsMachineTag should not be null.");
            }

        }

        [Test]
        public void TagsGetClustersNewcastleTest()
        {
            var col = Instance.TagsGetClusters("newcastle");

            Assert.That(col, Is.Not.Null);

            Assert.That(col, Has.Count.EqualTo(4), "Count should be four.");
            Assert.That(col, Has.Count.EqualTo(col.TotalClusters));
            Assert.That(col.SourceTag, Is.EqualTo("newcastle"));

            Assert.That(col[0].ClusterId, Is.EqualTo("water-ocean-clouds"));

            foreach (var c in col)
            {
                Assert.That(c.TotalTags, Is.Not.EqualTo(0), "TotalTags should not be zero.");
                Assert.That(c.Tags, Is.Not.Null, "Tags should not be null.");
                Assert.That(c.Tags, Has.Count.GreaterThanOrEqualTo(3));
                Assert.That(c.ClusterId, Is.Not.Null);
            }
        }

        [Test]
        public void TagsGetClusterPhotosNewcastleTest()
        {
            Flickr f = Instance;
            var col = f.TagsGetClusters("newcastle");

            foreach (var c in col)
            {
                var ps = f.TagsGetClusterPhotos(c);
                Assert.That(ps, Is.Not.Null);
                Assert.That(ps, Is.Not.Empty);
            }
        }

        [Test]
        [Ignore("Method might require authentication or always returns empty count and no score")]
        public void TagsGetHotListTest()
        {
            var col = Instance.TagsGetHotList();

            Assert.That(col, Is.Not.Empty, "Count should not be zero.");

            foreach (var c in col)
            {
                Assert.That(c, Is.Not.Null);
                Assert.That(c.Tag, Is.Not.Null);
                Assert.That(c.Score, Is.Not.EqualTo(0));
            }
        }

        [Test]
        public void TagsGetListUserTest()
        {
            var col = Instance.TagsGetListUser(TestData.TestUserId);
            Assert.That(col, Is.Not.Null);
        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void TagsGetMostFrequentlyUsedTest()
        {
            Flickr f = AuthInstance;

            var tags = f.TagsGetMostFrequentlyUsed();

            Assert.That(tags, Is.Not.Null);

            Assert.That(tags, Is.Not.Empty);
        }
    }
}
