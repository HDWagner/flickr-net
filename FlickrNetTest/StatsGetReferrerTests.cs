using System;

using NUnit.Framework;
using FlickrNet;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for StatsGetReferrerTests
    /// </summary>
    [TestFixture]
    [Category("AccessTokenRequired")]
    public class StatsGetReferrerTests : BaseTest
    {
        readonly string collectionId = "78188-72157600072356354";
        readonly string photoId = "5890800";
        readonly string photosetId = "1493109";
        readonly DateTime lastWeek = DateTime.Today.AddDays(-7);


        [Test]
        [Ignore("Method requires authentication")]
        public void StatsGetPhotoReferrersBasicTest()
        {
            string domain = "flickr.com";

            Flickr f = AuthInstance;

            StatReferrerCollection referrers = f.StatsGetPhotoReferrers(lastWeek, domain, 1, 10);

            Assert.That(referrers, Is.Not.Null, "StatReferrers should not be null.");

            Assert.That(referrers.Total, Is.Not.EqualTo(0), "StatReferrers.Total should not be zero.");

            Assert.That(Math.Min(referrers.Total, referrers.PerPage), Is.EqualTo(referrers.Count), "Count should either be equal to Total or PerPage.");

            Assert.That(referrers.DomainName, Is.EqualTo(domain), "StatReferrers.Domain should be the same as the searched for domain.");

            foreach (StatReferrer referrer in referrers)
            {
                Assert.That(referrer.Url, Is.Not.Null, "StatReferrer.Url should not be null.");
                Assert.That(referrer.Views, Is.Not.EqualTo(0), "StatReferrer.Views should be greater than zero.");
            }

            // Overloads
            referrers = f.StatsGetPhotoReferrers(lastWeek, domain);
            Assert.That(referrers, Is.Not.Null);

            referrers = f.StatsGetPhotoReferrers(lastWeek, domain, photoId);
            Assert.That(referrers, Is.Not.Null);

            referrers = f.StatsGetPhotoReferrers(lastWeek, domain, photoId, 1, 10);
            Assert.That(referrers, Is.Not.Null);

        }

        [Test]
        [Ignore("Method requires authentication")]
        public void StatsGetPhotosetsReferrersBasicTest()
        {
            string domain = "flickr.com";

            Flickr f = AuthInstance;

            StatReferrerCollection referrers = f.StatsGetPhotosetReferrers(lastWeek, domain, 1, 10);

            Assert.That(referrers, Is.Not.Null, "StatReferrers should not be null.");

            // I often get 0 referrers for a particular given date. As this method only works for the previous 28 days I cannot pick a fixed date.
            // Therefore we cannot confirm that regerrers.Total is always greater than zero.

            Assert.That(Math.Min(referrers.Total, referrers.PerPage), Is.EqualTo(referrers.Count), "Count should either be equal to Total or PerPage.");

            if (referrers.Total == 0)
            {
                return;
            }

            Assert.That(referrers.DomainName, Is.EqualTo(domain), "StatReferrers.Domain should be the same as the searched for domain.");

            foreach (StatReferrer referrer in referrers)
            {
                Assert.That(referrer.Url, Is.Not.Null, "StatReferrer.Url should not be null.");
                Assert.That(referrer.Views, Is.Not.EqualTo(0), "StatReferrer.Views should be greater than zero.");
            }

            // Overloads
            referrers = f.StatsGetPhotosetReferrers(lastWeek, domain);
            Assert.That(referrers, Is.Not.Null);

            referrers = f.StatsGetPhotosetReferrers(lastWeek, domain, photosetId);
            Assert.That(referrers, Is.Not.Null);

            referrers = f.StatsGetPhotosetReferrers(lastWeek, domain, photosetId, 1, 10);
            Assert.That(referrers, Is.Not.Null);

        }

        [Test]
        [Ignore("Method requires authentication")]
        public void StatsGetPhotostreamReferrersBasicTest()
        {
            string domain = "flickr.com";

            Flickr f = AuthInstance;

            StatReferrerCollection referrers = f.StatsGetPhotostreamReferrers(lastWeek, domain, 1, 10);

            Assert.That(referrers, Is.Not.Null, "StatReferrers should not be null.");

            // I often get 0 referrers for a particular given date. As this method only works for the previous 28 days I cannot pick a fixed date.
            // Therefore we cannot confirm that regerrers.Total is always greater than zero.

            Assert.That(Math.Min(referrers.Total, referrers.PerPage), Is.EqualTo(referrers.Count), "Count should either be equal to Total or PerPage.");

            if (referrers.Total == 0)
            {
                return;
            }

            Assert.That(referrers.DomainName, Is.EqualTo(domain), "StatReferrers.Domain should be the same as the searched for domain.");

            foreach (StatReferrer referrer in referrers)
            {
                Assert.That(referrer.Url, Is.Not.Null, "StatReferrer.Url should not be null.");
                Assert.That(referrer.Views, Is.Not.EqualTo(0), "StatReferrer.Views should be greater than zero.");
            }

            // Overloads
            referrers = f.StatsGetPhotostreamReferrers(lastWeek, domain);
            Assert.That(referrers, Is.Not.Null);
        }

        [Test]
        [Ignore("Method requires authentication")]
        public void StatsGetCollectionReferrersBasicTest()
        {
            string domain = "flickr.com";

            Flickr f = AuthInstance;

            var referrers = f.StatsGetCollectionReferrers(lastWeek, domain, 1, 10);

            Assert.That(referrers, Is.Not.Null, "StatReferrers should not be null.");

            Assert.That(Math.Min(referrers.Total, referrers.PerPage), Is.EqualTo(referrers.Count), "Count should either be equal to Total or PerPage.");

            if (referrers.Total == 0 && referrers.Pages == 0)
            {
                return;
            }

            Assert.That(referrers.DomainName, Is.EqualTo(domain), "StatReferrers.Domain should be the same as the searched for domain.");

            foreach (StatReferrer referrer in referrers)
            {
                Assert.That(referrer.Url, Is.Not.Null, "StatReferrer.Url should not be null.");
                Assert.That(referrer.Views, Is.Not.EqualTo(0), "StatReferrer.Views should be greater than zero.");
            }
            
            // Overloads
            referrers = f.StatsGetCollectionReferrers(lastWeek, domain);
            Assert.That(referrers, Is.Not.Null);

            referrers = f.StatsGetCollectionReferrers(lastWeek, domain, collectionId);
            Assert.That(referrers, Is.Not.Null);

            referrers = f.StatsGetCollectionReferrers(lastWeek, domain, collectionId, 1, 10);
            Assert.That(referrers, Is.Not.Null);
        }

    }
}
