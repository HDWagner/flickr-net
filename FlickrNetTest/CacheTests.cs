﻿using System;

using NUnit.Framework;
using FlickrNet;
using System.IO;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for CacheTests
    /// </summary>
    [TestFixture]
    public class CacheTests : BaseTest
    {
        [Test]
        public void CacheLocationTest()
        {
            string origLocation = Flickr.CacheLocation;

            Console.WriteLine(origLocation);

            string newLocation = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            Flickr.CacheLocation = newLocation;

            Assert.That(newLocation, Is.EqualTo(Flickr.CacheLocation));

            Flickr.CacheLocation = origLocation;

            Assert.That(origLocation, Is.EqualTo(Flickr.CacheLocation));

        }

        [Test]
        public void CacheHitTest()
        {
            if (Directory.Exists(Flickr.CacheLocation))
            {
                Directory.Delete(Flickr.CacheLocation, true);
            }

            Flickr f = Instance;
            Flickr.FlushCache();
            f.InstanceCacheDisabled = false;

            f.PeopleGetPublicPhotos(TestData.TestUserId);

            string lastUrl = f.LastRequest;

            var item = Cache.Responses.Get(lastUrl, TimeSpan.MaxValue, false);

            Assert.That(item, Is.Not.Null, "Cache should now contain the item.");
            Assert.IsInstanceOf<ResponseCacheItem>(item);

            var response = item as ResponseCacheItem;

            Assert.That(response?.Url, Is.Not.Null, "Url should not be null.");
            Assert.That(response?.Url.AbsoluteUri, Is.EqualTo(lastUrl), "Url should match the url requested from the cache.");
        }
    }
}
