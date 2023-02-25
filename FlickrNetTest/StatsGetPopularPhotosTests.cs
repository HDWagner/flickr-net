using System;

using NUnit.Framework;
using FlickrNet;
using FlickrNet.Classes;
using FlickrNetTest.TestUtilities;

namespace FlickrNetTest
{
    [TestFixture]
    [Category("AccessTokenRequired")]
    public class StatsGetPopularPhotosTests : BaseTest
    {
        [Test]
        [Ignore("Method requires authentication")]
        public void StatsGetPopularPhotosBasic()
        {
            PopularPhotoCollection photos = AuthInstance.StatsGetPopularPhotos(DateTime.MinValue, PopularitySort.None, 0, 0);

            Assert.That(photos, Is.Not.Null, "PopularPhotos should not be null.");

            Assert.That(photos.Total, Is.Not.EqualTo(0), "PopularPhotos.Total should not be zero.");
            Assert.That(photos, Is.Not.Empty, "PopularPhotos.Count should not be zero.");
            Assert.That(Math.Min(photos.Total, photos.PerPage), Is.EqualTo(photos.Count), "PopularPhotos.Count should equal either PopularPhotos.Total or PopularPhotos.PerPage.");

            foreach (Photo p in photos)
            {
                Assert.That(p.PhotoId, Is.Not.Null, "Photo.PhotoId should not be null.");
            }

            foreach (PopularPhoto p in photos)
            {
                Assert.That(p.PhotoId, Is.Not.Null, "PopularPhoto.PhotoId should not be null.");
                Assert.That(p.StatViews, Is.Not.EqualTo(0), "PopularPhoto.StatViews should not be zero.");
            }
        }

        [Test]
        [Ignore("Method requires authentication")]
        public void StatsGetPopularPhotosNoParamsTest()
        {
            Flickr f = AuthInstance;

            PopularPhotoCollection photos = f.StatsGetPopularPhotos();

            Assert.That(photos, Is.Not.Null, "PopularPhotos should not be null.");

            Assert.That(photos.Total, Is.Not.EqualTo(0), "PopularPhotos.Total should not be zero.");
            Assert.That(photos, Is.Not.Empty, "PopularPhotos.Count should not be zero.");
            Assert.That(Math.Min(photos.Total, photos.PerPage), Is.EqualTo(photos.Count), "PopularPhotos.Count should equal either PopularPhotos.Total or PopularPhotos.PerPage.");

            foreach (Photo p in photos)
            {
                Assert.That(p.PhotoId, Is.Not.Null, "Photo.PhotoId should not be null.");
            }

            foreach (PopularPhoto p in photos)
            {
                Assert.That(p.PhotoId, Is.Not.Null, "PopularPhoto.PhotoId should not be null.");
                Assert.That(p.StatViews, Is.Not.EqualTo(0), "PopularPhoto.StatViews should not be zero.");
            }
        }

        [Test]
        [Ignore("Method requires authentication")]
        public void StatsGetPopularPhotosOtherTest()
        {
            var lastWeek = DateTime.Today.AddDays(-7);

            var photos = AuthInstance.StatsGetPopularPhotos(lastWeek);
            Assert.That(photos, Is.Not.Null, "PopularPhotos should not be null.");

            photos = AuthInstance.StatsGetPopularPhotos(PopularitySort.Favorites);
            Assert.That(photos, Is.Not.Null, "PopularPhotos should not be null.");

            photos = AuthInstance.StatsGetPopularPhotos(lastWeek, 1, 10);
            Assert.That(photos, Is.Not.Null, "PopularPhotos should not be null.");
            Assert.That(photos, Has.Count.EqualTo(10), "Date search popular photos should return 10 photos.");

            photos = AuthInstance.StatsGetPopularPhotos(PopularitySort.Favorites, 1, 10);
            Assert.That(photos, Is.Not.Null, "PopularPhotos should not be null.");
            Assert.That(photos, Has.Count.EqualTo(10), "Favorite search popular photos should return 10 photos.");

        }
    }
}
