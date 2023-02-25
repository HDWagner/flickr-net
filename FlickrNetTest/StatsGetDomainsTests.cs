using System;

using NUnit.Framework;
using FlickrNet;
using FlickrNet.Classes;
using FlickrNetTest.TestUtilities;

namespace FlickrNetTest
{
    [TestFixture]
    [Category("AccessTokenRequired")]
    public class StatsGetDomainsTests : BaseTest
    {
        readonly string collectionId = "78188-72157600072356354";
        readonly string photoId = "5890800";
        readonly string photosetId = "1493109";

        [Test]
        [Ignore("Method requires authentication")]
        public void StatsGetCollectionDomainsBasicTest()
        {
            Flickr f = AuthInstance;

            var domains = f.StatsGetCollectionDomains(DateTime.Today.AddDays(-2));

            Assert.That(domains, Is.Not.Null, "StatDomains should not be null.");
            Assert.That(domains, Has.Count.EqualTo(domains.Total), "StatDomains.Count should be the same as StatDomains.Total");

            // Overloads
            domains = f.StatsGetCollectionDomains(DateTime.Today.AddDays(-2), collectionId);
            Assert.That(domains, Is.Not.Null);

            domains = f.StatsGetCollectionDomains(DateTime.Today.AddDays(-2), 1, 10);
            Assert.That(domains, Is.Not.Null);

            domains = f.StatsGetCollectionDomains(DateTime.Today.AddDays(-2), collectionId, 1, 10);
            Assert.That(domains, Is.Not.Null);
        }

        [Test]
        [Ignore("Method requires authentication")]
        public void StatsGetCollectionStatsTest()
        {
            Flickr f = AuthInstance;

            Stats stats = f.StatsGetCollectionStats(DateTime.Today.AddDays(-2), collectionId);

            Assert.That(stats, Is.Not.Null, "Stats should not be null.");
        }

        [Test]
        [Ignore("Method requires authentication")]
        public void StatsGetPhotoDomainsTests()
        {
            Flickr f = AuthInstance;

            var domains = f.StatsGetPhotoDomains(DateTime.Today.AddDays(-2));
            Assert.That(domains, Is.Not.Null, "StatDomains should not be null.");
            Assert.That(domains, Is.Not.Empty, "StatDomains.Count should not be zero.");

            foreach (StatDomain domain in domains)
            {
                Assert.That(domain.Name, Is.Not.Null, "StatDomain.Name should not be null.");
                Assert.That(domain.Views, Is.Not.EqualTo(0), "StatDomain.Views should not be zero.");
            }

            // Overloads
            domains = f.StatsGetPhotoDomains(DateTime.Today.AddDays(-2), photoId);
            Assert.That(domains, Is.Not.Null, "StatDomains should not be null.");

            domains = f.StatsGetPhotoDomains(DateTime.Today.AddDays(-2), photoId, 1, 10);
            Assert.That(domains, Is.Not.Null, "StatDomains should not be null.");

            domains = f.StatsGetPhotoDomains(DateTime.Today.AddDays(-2), 1, 10);
            Assert.That(domains, Is.Not.Null, "StatDomains should not be null.");

        }

        [Test]
        [Ignore("Method requires authentication")]
        public void StatsGetPhotoStatsTest()
        {
            Flickr f = AuthInstance;

            var stats = f.StatsGetPhotoStats(DateTime.Today.AddDays(-5), photoId);

            Assert.That(stats, Is.Not.Null);
        }

        [Test]
        [Ignore("Method requires authentication")]
        public void StatsGetPhotosetDomainsBasic()
        {
            Flickr f = AuthInstance;

            var domains = f.StatsGetPhotosetDomains(DateTime.Today.AddDays(-2));
            Assert.That(domains, Is.Not.Null, "StatDomains should not be null.");

            foreach (StatDomain domain in domains)
            {
                Assert.That(domain.Name, Is.Not.Null, "StatDomain.Name should not be null.");
                Assert.That(domain.Views, Is.Not.EqualTo(0), "StatDomain.Views should not be zero.");
            }

            // Overloads
            domains = f.StatsGetPhotosetDomains(DateTime.Today.AddDays(-2), 1, 10);
            Assert.That(domains, Is.Not.Null, "StatDomains should not be null.");

            domains = f.StatsGetPhotosetDomains(DateTime.Today.AddDays(-2), photosetId);
            Assert.That(domains, Is.Not.Null, "StatDomains should not be null.");

            domains = f.StatsGetPhotosetDomains(DateTime.Today.AddDays(-2), photosetId, 1, 10);
            Assert.That(domains, Is.Not.Null, "StatDomains should not be null.");


        }

        [Test]
        [Ignore("Method requires authentication")]
        public void StatsGetPhotosetStatsTest()
        {
            Flickr f = AuthInstance;

            var stats = f.StatsGetPhotosetStats(DateTime.Today.AddDays(-5), photosetId);

            Assert.That(stats, Is.Not.Null);
        }

        [Test]
        [Ignore("Method requires authentication")]
        public void StatsGetPhotostreamDomainsBasic()
        {
            Flickr f = AuthInstance;

            var domains = f.StatsGetPhotostreamDomains(DateTime.Today.AddDays(-2));
            Assert.That(domains, Is.Not.Null, "StatDomains should not be null.");

            foreach (StatDomain domain in domains)
            {
                Assert.That(domain.Name, Is.Not.Null, "StatDomain.Name should not be null.");
                Assert.That(domain.Views, Is.Not.EqualTo(0), "StatDomain.Views should not be zero.");
            }

            // Overload
            domains = f.StatsGetPhotostreamDomains(DateTime.Today.AddDays(-2), 1, 10);
            Assert.That(domains, Is.Not.Null, "StatDomains should not be null.");
        }

        [Test]
        [Ignore("Method requires authentication")]
        public void StatsGetPhotostreamStatsTest()
        {
            Flickr f = AuthInstance;

            var stats = f.StatsGetPhotostreamStats(DateTime.Today.AddDays(-5));

            Assert.That(stats, Is.Not.Null);
        }


    }
}
