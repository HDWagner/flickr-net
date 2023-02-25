using FlickrNet.Classes;
using FlickrNetTest.TestUtilities;
using NUnit.Framework;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for ActivityTests
    /// </summary>
    [Ignore("Method requires authentication")]
    [TestFixture]
    public class ActivityTests : BaseTest
    {

        [Test]
        [Category("AccessTokenRequired")]
        public void ActivityUserCommentsBasicTest()
        {
            ActivityItemCollection activity = AuthInstance.ActivityUserComments(0, 0);

            Assert.That(activity, Is.Not.Null, "ActivityItemCollection should not be null.");

            foreach (ActivityItem item in activity)
            {
                Assert.That(item.Id, Is.Not.Null, "Id should not be null.");
            }
        }

        [Test]
        [Category("AccessTokenRequired")]
        public void ActivityUserPhotosBasicTest()
        {
            ActivityItemCollection activity = AuthInstance.ActivityUserPhotos(20, "d");

            Assert.That(activity, Is.Not.Null, "ActivityItemCollection should not be null.");

            foreach (ActivityItem item in activity)
            {
                Assert.That(item.Id, Is.Not.Null, "Id should not be null.");
            }
        }

        [Test]
        [Category("AccessTokenRequired")]
        public void ActivityUserPhotosBasicTests()
        {
            int days = 50;

            // Get last 10 days activity.
            ActivityItemCollection items = AuthInstance.ActivityUserPhotos(days, "d");

            Assert.That(items, Is.Not.Null, "ActivityItemCollection should not be null.");

            if (items.Count == 0)
            {
                Assert.Inconclusive("Unable to continue the test as no recent activity");
                return;
            }

            foreach (ActivityItem item in items)
            {
                Assert.That(item.ItemType, Is.Not.EqualTo(ActivityItemType.Unknown), "ItemType should not be 'Unknown'.");
                Assert.That(item.Id, Is.Not.Null, "Id should not be null.");

                Assert.That(item.Events, Is.Not.Empty, "Events.Count should not be zero.");

                foreach (ActivityEvent e in item.Events)
                {
                    Assert.That(e.EventType, Is.Not.EqualTo(ActivityEventType.Unknown), "EventType should not be 'Unknown'.");
                    Assert.That(e.DateAdded, Is.GreaterThan(DateTime.Today.AddDays(-days)), "DateAdded should be within the last " + days + " days");

                    // For Gallery events the comment is optional.
                    if (e.EventType != ActivityEventType.Gallery)
                    {
                        if (e.EventType == ActivityEventType.Note || e.EventType == ActivityEventType.Comment || e.EventType == ActivityEventType.Tag)
                        {
                            Assert.That(e.Value, Is.Not.Null, "Value should not be null for a non-favorite event.");
                        }
                        else
                        {
                            Assert.That(e.Value, Is.Null, "Value should be null for an event of type " + e.EventType);
                        }
                    }

                    if (e.EventType == ActivityEventType.Comment)
                    {
                        Assert.That(e.CommentId, Is.Not.Null, "CommentId should not be null for a comment event.");
                    }
                    else
                    {
                        Assert.That(e.CommentId, Is.Null, "CommentId should be null for non-comment events.");
                    }

                    if (e.EventType == ActivityEventType.Gallery)
                    {
                        Assert.That(e.GalleryId, Is.Not.Null, "GalleryId should not be null for a gallery event.");
                    }
                    else
                    {
                        Assert.That(e.GalleryId, Is.Null, "GalleryId should be null for non-gallery events.");
                    }
                }
            }
        }
    }
}
