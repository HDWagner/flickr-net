using System;
using FlickrNet;
using NUnit.Framework;

namespace FlickrNetTest
{
    [TestFixture]
    public class PhotosGetFavouritesTests : BaseTest
    {
        [Test]
        public void PhotosGetFavoritesNoFavourites()
        {
            // No favourites
            PhotoFavoriteCollection favs = Instance.PhotosGetFavorites(TestData.PhotoId);

            Assert.That(favs, Is.Empty, "Should have no favourites");

        }

        [Test]
        [Ignore("Method requires authentication")]
        public void PhotosGetFavoritesHasFavourites()
        {
            PhotoFavoriteCollection favs = Instance.PhotosGetFavorites(TestData.FavouritedPhotoId, 500, 1);

            Assert.That(favs, Is.Not.Null, "PhotoFavourites instance should not be null.");

            Assert.That(favs, Is.Not.Empty, "PhotoFavourites.Count should not be zero.");

            Assert.That(favs, Has.Count.EqualTo(50), "Should be 50 favourites listed (maximum returned)");

            foreach (PhotoFavorite p in favs)
            {
                Assert.That(string.IsNullOrEmpty(p.UserId), Is.False, "Should have a user ID.");
                Assert.That(string.IsNullOrEmpty(p.UserName), Is.False, "Should have a user name.");
                Assert.That(p.FavoriteDate, Is.Not.EqualTo(default(DateTime)), "Favourite Date should not be default Date value");
                Assert.That(p.FavoriteDate, Is.LessThan(DateTime.Now), "Favourite Date should be in the past.");
            }
        }

        [Test]
        public void PhotosGetFavoritesPaging()
        {
            PhotoFavoriteCollection favs = Instance.PhotosGetFavorites(TestData.FavouritedPhotoId, 10, 1);

            Assert.That(favs, Has.Count.EqualTo(10), "PhotoFavourites.Count should be 10.");
            Assert.That(favs.PerPage, Is.EqualTo(10), "PhotoFavourites.PerPage should be 10");
            Assert.That(favs.Page, Is.EqualTo(1), "PhotoFavourites.Page should be 1.");
            Assert.That(favs.Total, Is.GreaterThan(100), "PhotoFavourites.Total should be greater than 100.");
            Assert.That(favs.Pages, Is.GreaterThan(10), "PhotoFavourites.Pages should be greater than 10.");
        }

        [Test]
        public void PhotosGetFavoritesPagingTwo()
        {
            PhotoFavoriteCollection favs = Instance.PhotosGetFavorites(TestData.FavouritedPhotoId, 10, 2);

            Assert.That(favs, Has.Count.EqualTo(10), "PhotoFavourites.Count should be 10.");
            Assert.That(favs.PerPage, Is.EqualTo(10), "PhotoFavourites.PerPage should be 10");
            Assert.That(favs.Page, Is.EqualTo(2), "PhotoFavourites.Page should be 2.");
        }
    }
}
