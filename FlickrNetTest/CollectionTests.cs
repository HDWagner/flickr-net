
using NUnit.Framework;
using FlickrNet;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for CollectionGetTreeTest
    /// </summary>
    [TestFixture]
    public class CollectionTests : BaseTest
    {
        
        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void CollectionGetInfoBasicTest()
        {
            string id = "78188-72157618817175751";

            Flickr f = AuthInstance;

            CollectionInfo info = f.CollectionsGetInfo(id);

            Assert.That(info.CollectionId, Is.EqualTo(id), "CollectionId should be correct.");
            Assert.That(info.ChildCount, Is.EqualTo(1), "ChildCount should be 1.");
            Assert.That(info.Title, Is.EqualTo("Global Collection"), "Title should be 'Global Collection'.");
            Assert.That(info.Description, Is.EqualTo("My global collection."), "Description should be set correctly.");
            Assert.That(info.Server, Is.EqualTo("3629"), "Server should be 3629.");

            Assert.That(info.IconPhotos, Has.Count.EqualTo(12), "IconPhotos.Length should be 12.");

            Assert.That(info.IconPhotos[0].Title, Is.EqualTo("Tires"), "The first IconPhoto Title should be 'Tires'.");
        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void CollectionGetTreeRootTest()
        {
            Flickr f = AuthInstance;
            CollectionCollection tree = f.CollectionsGetTree();

            Assert.That(tree, Is.Not.Null, "CollectionList should not be null.");
            Assert.That(tree, Is.Not.Empty, "CollectionList.Count should not be zero.");

            foreach (Collection coll in tree)
            {
                Assert.That(coll.CollectionId, Is.Not.Null, "CollectionId should not be null.");
                Assert.That(coll.Title, Is.Not.Null, "Title should not be null.");
                Assert.That(coll.Description, Is.Not.Null, "Description should not be null.");
                Assert.That(coll.IconSmall, Is.Not.Null, "IconSmall should not be null.");
                Assert.That(coll.IconLarge, Is.Not.Null, "IconLarge should not be null.");

                Assert.That(coll.Sets.Count + coll.Collections.Count, Is.Not.EqualTo(0), "Should be either some sets or some collections.");

                foreach (CollectionSet set in coll.Sets)
                {
                    Assert.That(set.SetId, Is.Not.Null, "SetId should not be null.");
                }
            }
        }

        [Test]
        public void CollectionGetTreeRootForSpecificUser()
        {
            Flickr f = Instance;
            CollectionCollection tree = f.CollectionsGetTree(null, TestData.TestUserId);

            Assert.That(tree, Is.Not.Null, "CollectionList should not be null.");
            Assert.That(tree, Is.Not.Empty, "CollectionList.Count should not be zero.");

            foreach (Collection coll in tree)
            {
                Assert.That(coll.CollectionId, Is.Not.Null, "CollectionId should not be null.");
                Assert.That(coll.Title, Is.Not.Null, "Title should not be null.");
                Assert.That(coll.Description, Is.Not.Null, "Description should not be null.");
                Assert.That(coll.IconSmall, Is.Not.Null, "IconSmall should not be null.");
                Assert.That(coll.IconLarge, Is.Not.Null, "IconLarge should not be null.");

                Assert.That(coll.Sets.Count + coll.Collections.Count, Is.Not.EqualTo(0), "Should be either some sets or some collections.");

                foreach (CollectionSet set in coll.Sets)
                {
                    Assert.That(set.SetId, Is.Not.Null, "SetId should not be null.");
                }
            }
        }

        [Test]
        public void CollectionGetSubTreeForSpecificUser()
        {
            string id = "78188-72157618817175751";
            Flickr f = Instance;
            CollectionCollection tree = f.CollectionsGetTree(id, TestData.TestUserId);

            Assert.That(tree, Is.Not.Null, "CollectionList should not be null.");
            Assert.That(tree, Is.Not.Empty, "CollectionList.Count should not be zero.");

            foreach (Collection coll in tree)
            {
                Assert.That(coll.CollectionId, Is.Not.Null, "CollectionId should not be null.");
                Assert.That(coll.Title, Is.Not.Null, "Title should not be null.");
                Assert.That(coll.Description, Is.Not.Null, "Description should not be null.");
                Assert.That(coll.IconSmall, Is.Not.Null, "IconSmall should not be null.");
                Assert.That(coll.IconLarge, Is.Not.Null, "IconLarge should not be null.");

                Assert.That(coll.Sets.Count + coll.Collections.Count, Is.Not.EqualTo(0), "Should be either some sets or some collections.");

                foreach (CollectionSet set in coll.Sets)
                {
                    Assert.That(set.SetId, Is.Not.Null, "SetId should not be null.");
                }
            }
        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void CollectionsEditMetaTest()
        {
            string id = "78188-72157618817175751";

            Flickr.CacheDisabled = true;
            Flickr f = AuthInstance;

            CollectionInfo info = f.CollectionsGetInfo(id);

            f.CollectionsEditMeta(id, info.Title, info.Description + "TEST");

            var info2 = f.CollectionsGetInfo(id);

            Assert.That(info2.Description, Is.Not.EqualTo(info.Description));

            // Revert description
            f.CollectionsEditMeta(id, info.Title, info.Description);

        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void CollectionsEmptyCollection()
        {
            Flickr f = AuthInstance;

            // Get global collection
            CollectionCollection collections = f.CollectionsGetTree("78188-72157618817175751", null);

            Assert.That(collections, Is.Not.Null);
            Assert.That(collections, Is.Not.Empty, "Global collection should be greater than zero.");

            var col = collections[0];

            Assert.That(col.Title, Is.EqualTo("Global Collection"), "Global Collection title should be correct.");

            Assert.That(col.Collections, Is.Not.Null, "Child collections property should not be null.");
            Assert.That(col.Collections, Is.Not.Empty, "Global collection should have child collections.");

            var subsol = col.Collections[0];

            Assert.That(subsol.Collections, Is.Not.Null, "Child collection Collections property should ne null.");
            Assert.That(subsol.Collections, Is.Empty, "Child collection should not have and sub collections.");

        }
    }
}
