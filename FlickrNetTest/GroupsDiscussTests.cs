using FlickrNet.Classes;
using FlickrNetTest.TestUtilities;
using NUnit.Framework;

namespace FlickrNetTest
{
    [TestFixture]
    public class GroupsDiscussTests : BaseTest
    {

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void GroupsDiscussRepliesAddTest()
        {
            var topicId = "72157630982877126";
            var message = "Test message reply\n" + DateTime.Now.ToString("o");
            var newMessage = "New Message reply\n" + DateTime.Now.ToString("o");

            TopicReply? reply = null;
            TopicReplyCollection topicReplies;
            try
            {
                AuthInstance.GroupsDiscussRepliesAdd(topicId, message);

                Thread.Sleep(1000);

                topicReplies = AuthInstance.GroupsDiscussRepliesGetList(topicId, 1, 100);

                reply = topicReplies.FirstOrDefault(r => r.Message == message);

                Assert.That(reply, Is.Not.Null, "Cannot find matching message.");

                AuthInstance.GroupsDiscussRepliesEdit(topicId, reply.ReplyId, newMessage);

                var reply2 = AuthInstance.GroupsDiscussRepliesGetInfo(topicId, reply.ReplyId);

                Assert.That(reply2.Message, Is.EqualTo(newMessage), "Message should have been updated.");

            }
            finally
            {
                if (reply != null)
                {
                    AuthInstance.GroupsDiscussRepliesDelete(topicId, reply.ReplyId);
                    topicReplies = AuthInstance.GroupsDiscussRepliesGetList(topicId, 1, 100);
                    var reply3 = topicReplies.FirstOrDefault(r => r.ReplyId == reply.ReplyId);
                    Assert.That(reply3, Is.Null, "Reply should not exist anymore.");
                }
            }

        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void GroupsDiscussRepliesGetListTest()
        {
            var topics = AuthInstance.GroupsDiscussTopicsGetList(TestData.GroupId, 1, 100);

            Assert.That(topics, Is.Not.Null, "Topics should not be null.");

            Assert.That(topics, Is.Not.Empty, "Should be more than one topics return.");

            var firstTopic = topics.First(t => t.RepliesCount > 0);

            var replies = AuthInstance.GroupsDiscussRepliesGetList(firstTopic.TopicId, 1, 10);
            Assert.That(replies.TopicId, Is.EqualTo(firstTopic.TopicId), "TopicId's should be the same.");
            Assert.That(replies.Subject, Is.EqualTo(firstTopic.Subject), "Subject's should be the same.");
            Assert.That(replies.Message, Is.EqualTo(firstTopic.Message), "Message's should be the same.");
            Assert.That(replies.DateCreated, Is.EqualTo(firstTopic.DateCreated), "DateCreated's should be the same.");
            Assert.That(replies.DateLastPost, Is.EqualTo(firstTopic.DateLastPost), "DateLastPost's should be the same.");

            Assert.That(replies, Is.Not.Null, "Replies should not be null.");

            var firstReply = replies.First();

            Assert.That(firstReply.ReplyId, Is.Not.Null, "ReplyId should not be null.");

            var reply = AuthInstance.GroupsDiscussRepliesGetInfo(firstTopic.TopicId, firstReply.ReplyId);
            Assert.That(reply, Is.Not.Null, "Reply should not be null.");
            Assert.That(reply.Message, Is.EqualTo(firstReply.Message), "TopicReply.Message should be the same.");
        }

        [Test]
        [Ignore("Got this working, now ignore as there is no way to delete topics!")]
        [Category("AccessTokenRequired")]
        public void GroupsDiscussTopicsAddTest()
        {
            var groupId = TestData.FlickrNetTestGroupId;

            var subject = "Test subject line: " + DateTime.Now.ToString("o");
            var message = "Subject message line.";

            AuthInstance.GroupsDiscussTopicsAdd(groupId, subject, message);

            var topics = AuthInstance.GroupsDiscussTopicsGetList(groupId, 1, 5);

            var topic = topics.SingleOrDefault(t => t.Subject == subject);

            Assert.That(topic, Is.Not.Null, "Unable to find topic with matching subject line.");

            Assert.That(topic.Message, Is.EqualTo(message), "Message should be the same.");
        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void GroupsDiscussTopicsGetListTest()
        {
            var topics = AuthInstance.GroupsDiscussTopicsGetList(TestData.GroupId, 1, 10);

            Assert.That(topics, Is.Not.Null, "Topics should not be null.");

            Assert.That(topics.GroupId, Is.EqualTo(TestData.GroupId), "GroupId should be the same.");
            Assert.That(topics, Is.Not.Empty, "Should be more than one topics return.");
            Assert.That(topics, Has.Count.EqualTo(10), "Count should be 10.");

            foreach (var topic in topics)
            {
                Assert.That(topic.TopicId, Is.Not.Null, "TopicId should not be null.");
                Assert.That(topic.Subject, Is.Not.Null, "Subject should not be null.");
                Assert.That(topic.Message, Is.Not.Null, "Message should not be null.");
            }

            var firstTopic = topics.First();

            var secondTopic = AuthInstance.GroupsDiscussTopicsGetInfo(firstTopic.TopicId);
            Assert.That(secondTopic.TopicId, Is.EqualTo(firstTopic.TopicId), "TopicId's should be the same.");
            Assert.That(secondTopic.Subject, Is.EqualTo(firstTopic.Subject), "Subject's should be the same.");
            Assert.That(secondTopic.Message, Is.EqualTo(firstTopic.Message), "Message's should be the same.");
            Assert.That(secondTopic.DateCreated, Is.EqualTo(firstTopic.DateCreated), "DateCreated's should be the same.");
            Assert.That(secondTopic.DateLastPost, Is.EqualTo(firstTopic.DateLastPost), "DateLastPost's should be the same.");

        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void GroupsDiscussTopicsGetListEditableTest()
        {
            var groupId = "51035612836@N01"; // Flickr API group

            var topics = AuthInstance.GroupsDiscussTopicsGetList(groupId, 1, 20);

            Assert.That(topics, Is.Not.Empty);

            foreach (var topic in topics)
            {
                Assert.That(topic.CanEdit, Is.True, "CanEdit should be true.");
                if (!topic.IsLocked)
                {
                    Assert.That(topic.CanReply, Is.True, "CanReply should be true.");
                }

                Assert.That(topic.CanDelete, Is.True, "CanDelete should be true.");
            }
        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void GroupsDiscussTopicsGetInfoStickyTest()
        {
            const string topicId = "72157630982967152";
            var topic = AuthInstance.GroupsDiscussTopicsGetInfo(topicId);

            Assert.That(topic.IsSticky, Is.True, "This topic should be marked as sticky.");
            Assert.That(topic.IsLocked, Is.False, "This topic should not be marked as locked.");

            // topic.CanReply should be true, but for some reason isn't, so we cannot test it.
        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void GroupsDiscussTopicsGetInfoLockedTest()
        {
            const string topicId = "72157630982969782";

            var topic = AuthInstance.GroupsDiscussTopicsGetInfo(topicId);

            Assert.That(topic.IsLocked, Is.True, "This topic should be marked as locked.");
            Assert.That(topic.IsSticky, Is.False, "This topic should not be marked as sticky.");

            Assert.That(topic.CanReply, Is.False, "CanReply should be false as the topic is locked.");
        }

    }
}
