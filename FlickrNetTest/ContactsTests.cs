using System;

using NUnit.Framework;
using FlickrNet;
using FlickrNet.Classes;
using FlickrNetTest.TestUtilities;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for ContactsTests
    /// </summary>
    [TestFixture]
    public class ContactsTests : BaseTest
    {
        
        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void ContactsGetListTestBasicTest()
        {
            Flickr f = AuthInstance;
            var contacts = f.ContactsGetList();

            Assert.That(contacts, Is.Not.Null);

            foreach (var contact in contacts)
            {
                Assert.That(contact.UserId, Is.Not.Null, "UserId should not be null.");
                Assert.That(contact.UserName, Is.Not.Null, "UserName should not be null.");
                Assert.That(contact.BuddyIconUrl, Is.Not.Null, "BuddyIconUrl should not be null.");
            }
        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void ContactsGetListFullParamTest()
        {
            Flickr f = AuthInstance;

            ContactCollection contacts = f.ContactsGetList(null, 0, 0);

            Assert.That(contacts, Is.Not.Null, "Contacts should not be null.");
        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void ContactsGetListFilteredTest()
        {
            Flickr f = AuthInstance;
            var contacts = f.ContactsGetList("friends");

            Assert.That(contacts, Is.Not.Null);

            foreach (var contact in contacts)
            {
                Assert.That(contact.UserId, Is.Not.Null, "UserId should not be null.");
                Assert.That(contact.UserName, Is.Not.Null, "UserName should not be null.");
                Assert.That(contact.BuddyIconUrl, Is.Not.Null, "BuddyIconUrl should not be null.");
                Assert.That(contact.IsFriend, Is.Not.Null, "IsFriend should not be null.");
                Assert.That(contact.IsFriend.Value, Is.True);
            }
        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void ContactsGetListPagedTest()
        {
            Flickr f = AuthInstance;
            var contacts = f.ContactsGetList(2, 20);

            Assert.That(contacts, Is.Not.Null);
            Assert.That(contacts.Page, Is.EqualTo(2));
            Assert.That(contacts.PerPage, Is.EqualTo(20));
            Assert.That(contacts, Has.Count.EqualTo(20));

            foreach (var contact in contacts)
            {
                Assert.That(contact.UserId, Is.Not.Null, "UserId should not be null.");
                Assert.That(contact.UserName, Is.Not.Null, "UserName should not be null.");
                Assert.That(contact.BuddyIconUrl, Is.Not.Null, "BuddyIconUrl should not be null.");
            }
        }

        [Test]
        public void ContactsGetPublicListTest()
        {
            Flickr f = Instance;

            ContactCollection contacts = f.ContactsGetPublicList(TestData.TestUserId);

            Assert.That(contacts, Is.Not.Null, "Contacts should not be null.");

            Assert.That(contacts.Total, Is.Not.EqualTo(0), "Total should not be zero.");
            Assert.That(contacts.PerPage, Is.Not.EqualTo(0), "PerPage should not be zero.");
        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void ContactsGetRecentlyUpdatedTest()
        {
            Flickr f = AuthInstance;

            ContactCollection contacts = f.ContactsGetListRecentlyUploaded(DateTime.Now.AddDays(-1), null);

            Assert.That(contacts, Is.Not.Null, "Contacts should not be null.");
        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void ContactsGetTaggingSuggestions()
        {
            Flickr f = AuthInstance;

            var contacts = f.ContactsGetTaggingSuggestions();

            Assert.That(contacts, Is.Not.Null);
        }

    }
}
