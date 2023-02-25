using FlickrNet.Classes;
using FlickrNetTest.TestUtilities;
using NUnit.Framework;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for FavouritesGetPublicListTests
    /// </summary>
    [TestFixture]
    public class FavoritesTests : BaseTest
    {
        [Test]
        public void FavoritesGetPublicListBasicTest()
        {
            const string userId = "77788903@N00";

            var p = Instance.FavoritesGetPublicList(userId);

            Assert.That(p, Is.Not.Null, "PhotoCollection should not be null instance.");
            Assert.That(p, Is.Not.Empty, "PhotoCollection.Count should be greater than zero.");
        }

        [Test]
        public void FavouritesGetPublicListWithDates()
        {
            var allFavourites = Instance.FavoritesGetPublicList(TestData.TestUserId);

            var firstFiveFavourites = allFavourites.OrderBy(p => p.DateFavorited).Take(5).ToList();

            var minDate = firstFiveFavourites.Min(p => p.DateFavorited).GetValueOrDefault();
            var maxDate = firstFiveFavourites.Max(p => p.DateFavorited).GetValueOrDefault();

            var subsetOfFavourites = Instance.FavoritesGetPublicList(TestData.TestUserId, minDate, maxDate,
                                                                     PhotoSearchExtras.None, 0, 0);

            Assert.That(subsetOfFavourites, Has.Count.EqualTo(5), "Should be 5 favourites in subset");
        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void FavoritesGetListBasicTest()
        {
            var photos = AuthInstance.FavoritesGetList();
            Assert.That(photos, Is.Not.Null, "PhotoCollection should not be null instance.");
            Assert.That(photos, Is.Not.Empty, "PhotoCollection.Count should be greater than zero.");
        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void FavoritesGetListFullParamTest()
        {
            var photos = AuthInstance.FavoritesGetList(TestData.TestUserId, DateTime.Now.AddYears(-4), DateTime.Now, PhotoSearchExtras.All, 1, 10);
            Assert.That(photos, Is.Not.Null, "PhotoCollection should not be null.");

            Assert.That(photos, Is.Not.Empty, "Count should be greater than zero.");

        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void FavoritesGetListPartialParamTest()
        {
            PhotoCollection photos = AuthInstance.FavoritesGetList(TestData.TestUserId, 2, 20);
            Assert.That(photos, Is.Not.Null, "PhotoCollection should not be null instance.");
            Assert.That(photos, Is.Not.Empty, "PhotoCollection.Count should be greater than zero.");
            Assert.That(photos.Page, Is.EqualTo(2));
            Assert.That(photos.PerPage, Is.EqualTo(20));
            Assert.That(photos, Has.Count.EqualTo(20));
        }

        [Test]
        public void FavoritesGetContext()
        {
            const string photoId = "2502963121";
            const string userId = "41888973@N00";

            var context = Instance.FavoritesGetContext(photoId, userId);

            Assert.That(context, Is.Not.Null);
            Assert.That(context.Count, Is.Not.EqualTo(0), "Count should be greater than zero");
            Assert.That(context.PreviousPhotos, Has.Count.EqualTo(1), "Should be 1 previous photo.");
            Assert.That(context.NextPhotos, Has.Count.EqualTo(1), "Should be 1 next photo.");
        }

        [Test]
        public void FavoritesGetContextMorePrevious()
        {
            const string photoId = "2502963121";
            const string userId = "41888973@N00";

            var context = Instance.FavoritesGetContext(photoId, userId, 3, 4, PhotoSearchExtras.Description);

            Assert.That(context, Is.Not.Null);
            Assert.That(context.Count, Is.Not.EqualTo(0), "Count should be greater than zero");
            Assert.That(context.PreviousPhotos, Has.Count.EqualTo(3), "Should be 3 previous photo.");
            Assert.That(context.NextPhotos, Has.Count.EqualTo(4), "Should be 4 next photo.");
        }
    }
}
