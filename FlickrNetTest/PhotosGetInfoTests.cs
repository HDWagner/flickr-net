using System;
using System.Linq;
using FlickrNet.Exceptions;
using NUnit.Framework;
using FlickrNet;
using System.Reactive.Subjects;
using System.Reactive.Linq;
using Shouldly;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for PhotosGetInfoTests
    /// </summary>
    [TestFixture]
    public class PhotosGetInfoTests : BaseTest
    {
        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void PhotosGetInfoBasicTest()
        {
            PhotoInfo info = AuthInstance.PhotosGetInfo("4268023123");

            Assert.That(info, Is.Not.Null);

            Assert.That(info.PhotoId, Is.EqualTo("4268023123"));
            Assert.That(info.Secret, Is.EqualTo("a4283bac01"));
            Assert.That(info.Server, Is.EqualTo("2795"));
            Assert.That(info.Farm, Is.EqualTo("3"));
            Assert.That(info.DateUploaded, Is.EqualTo(UtilityMethods.UnixTimestampToDate("1263291891")));
            Assert.That(info.IsFavorite, Is.EqualTo(false));
            Assert.That(info.License, Is.EqualTo(LicenseType.AttributionNoncommercialShareAlikeCC));
            Assert.That(info.Rotation, Is.EqualTo(0));
            Assert.That(info.OriginalSecret, Is.EqualTo("9d3d4bf24a"));
            Assert.That(info.OriginalFormat, Is.EqualTo("jpg"));
            Assert.That(info.ViewCount, Is.GreaterThan(87), "ViewCount should be greater than 87.");
            Assert.That(info.Media, Is.EqualTo(MediaType.Photos));

            Assert.That(info.Title, Is.EqualTo("12. Sudoku"));
            Assert.That(info.Description, Is.EqualTo("It scares me sometimes how much some of my handwriting reminds me of Dad's " +
                            "- in this photo there is one 5 that especially reminds me of his handwriting."));

            //Owner
            Assert.That(info.OwnerUserId, Is.EqualTo("41888973@N00"));

            //Dates
            Assert.That(info.DateTaken, Is.EqualTo(new DateTime(2010, 01, 12, 11, 01, 20)), "DateTaken is not set correctly.");

            //Editability
            Assert.That(info.CanComment, Is.True, "CanComment should be true when authenticated.");
            Assert.That(info.CanAddMeta, Is.True, "CanAddMeta should be true when authenticated.");

            //Permissions
            Assert.That(info.PermissionComment, Is.EqualTo(PermissionComment.Everybody));
            Assert.That(info.PermissionAddMeta, Is.EqualTo(PermissionAddMeta.Everybody));

            //Visibility

            // Notes

            Assert.That(info.Notes, Has.Count.EqualTo(1), "Notes.Count should be one.");
            Assert.That(info.Notes[0].NoteId, Is.EqualTo("72157623069944527"));
            Assert.That(info.Notes[0].AuthorId, Is.EqualTo("41888973@N00"));
            Assert.That(info.Notes[0].AuthorName, Is.EqualTo("Sam Judson"));
            Assert.That(info.Notes[0].XPosition, Is.EqualTo(267));
            Assert.That(info.Notes[0].YPosition, Is.EqualTo(238));

            // Tags

            Assert.That(info.Tags, Has.Count.EqualTo(5));
            Assert.That(info.Tags[0].TagId, Is.EqualTo("78188-4268023123-586"));
            Assert.That(info.Tags[0].Raw, Is.EqualTo("green"));

            // URLs

            Assert.That(info.Urls, Has.Count.EqualTo(1));
            Assert.That(info.Urls[0].UrlType, Is.EqualTo("photopage"));
            Assert.That(info.Urls[0].Url, Is.EqualTo("https://www.flickr.com/photos/samjudson/4268023123/"));

        }

        [Test]
        public void PhotosGetInfoUnauthenticatedTest()
        {
            PhotoInfo info = Instance.PhotosGetInfo("4268023123");

            Assert.That(info, Is.Not.Null);

            Assert.That(info.PhotoId, Is.EqualTo("4268023123"));
            Assert.That(info.Secret, Is.EqualTo("a4283bac01"));
            Assert.That(info.Server, Is.EqualTo("2795"));
            Assert.That(info.Farm, Is.EqualTo("3"));
            Assert.That(info.DateUploaded, Is.EqualTo(UtilityMethods.UnixTimestampToDate("1263291891")));
            Assert.That(info.IsFavorite, Is.EqualTo(false));
            Assert.That(info.License, Is.EqualTo(LicenseType.AttributionNoncommercialShareAlikeCC));
            Assert.That(info.Rotation, Is.EqualTo(0));
            Assert.That(info.OriginalSecret, Is.EqualTo("9d3d4bf24a"));
            Assert.That(info.OriginalFormat, Is.EqualTo("jpg"));
            Assert.That(info.ViewCount, Is.GreaterThan(87), "ViewCount should be greater than 87.");
            Assert.That(info.Media, Is.EqualTo(MediaType.Photos));

            Assert.That(info.Title, Is.EqualTo("12. Sudoku"));
            Assert.That(info.Description, Is.EqualTo("It scares me sometimes how much some of my handwriting reminds me of Dad's " +
                            "- in this photo there is one 5 that especially reminds me of his handwriting."));

            //Owner
            Assert.That(info.OwnerUserId, Is.EqualTo("41888973@N00"));

            //Dates

            //Editability
            Assert.That(info.CanComment, Is.False, "CanComment should be false when not authenticated.");
            Assert.That(info.CanAddMeta, Is.False, "CanAddMeta should be false when not authenticated.");

            //Permissions
            Assert.That(info.PermissionComment, Is.Null, "PermissionComment should be null when not authenticated.");
            Assert.That(info.PermissionAddMeta, Is.Null, "PermissionAddMeta should be null when not authenticated.");

            //Visibility

            // Notes

            Assert.That(info.Notes, Has.Count.EqualTo(1), "Notes.Count should be one.");
            Assert.That(info.Notes[0].NoteId, Is.EqualTo("72157623069944527"));
            Assert.That(info.Notes[0].AuthorId, Is.EqualTo("41888973@N00"));
            Assert.That(info.Notes[0].AuthorName, Is.EqualTo("Sam Judson"));
            Assert.That(info.Notes[0].XPosition, Is.EqualTo(267));
            Assert.That(info.Notes[0].YPosition, Is.EqualTo(238));

            // Tags

            Assert.That(info.Tags, Has.Count.EqualTo(5));
            Assert.That(info.Tags[0].TagId, Is.EqualTo("78188-4268023123-586"));
            Assert.That(info.Tags[0].Raw, Is.EqualTo("green"));

            // URLs

            Assert.That(info.Urls, Has.Count.EqualTo(1));
            Assert.That(info.Urls[0].UrlType, Is.EqualTo("photopage"));
            Assert.That(info.Urls[0].Url, Is.EqualTo("https://www.flickr.com/photos/samjudson/4268023123/"));
        }

        [Test]
        public void PhotosGetInfoTestUserIssue()
        {
            var photoId = "14042679057";
            var info = Instance.PhotosGetInfo(photoId);

            Assert.That(info.PhotoId, Is.EqualTo(photoId));
            Assert.That(info.OwnerUserId, Is.EqualTo("63226137@N02"));
            Assert.That(info.WebUrl, Is.EqualTo("https://www.flickr.com/photos/63226137@N02/14042679057/"));

        }
        [Test]
        [Category("AccessTokenRequired")]
        public void PhotosGetInfoTestLocation()
        {
            const string photoId = "4268756940";

            PhotoInfo info = AuthInstance.PhotosGetInfo(photoId);

            Assert.That(info.Location, Is.Not.Null);
        }

        [Test]
        public void PhotosGetInfoWithPeople()
        {
            const string photoId = "3547137580"; // https://www.flickr.com/photos/samjudson/3547137580/in/photosof-samjudson/

            PhotoInfo info = Instance.PhotosGetInfo(photoId);

            Assert.That(info, Is.Not.Null);
            Assert.That(info.HasPeople, Is.True, "HasPeople should be true.");

        }

        [Test]
        public void PhotosGetInfoCanBlogTest()
        {
            var o = new PhotoSearchOptions();
            o.UserId = TestData.TestUserId;
            o.PerPage = 5;

            PhotoCollection photos = Instance.PhotosSearch(o);
            Assert.That(photos[0].PhotoId, Is.Not.Null);
            PhotoInfo info = Instance.PhotosGetInfo(photos[0].PhotoId);

            Assert.That(info.CanBlog, Is.EqualTo(false));
            Assert.That(info.CanDownload, Is.EqualTo(true));
        }

        [Test]
        public void PhotosGetInfoDataTakenGranularityTest()
        {
            const string photoid = "4386780023";

            PhotoInfo info = Instance.PhotosGetInfo(photoid);

            Assert.That(info.DateTaken, Is.EqualTo(new DateTime(2009, 1, 1)));
            Assert.That(info.DateTakenGranularity, Is.EqualTo(DateGranularity.Circa));

        }

        [Test]
        public void PhotosGetInfoVideoTest()
        {
            const string videoId = "2926486605";

            var info = Instance.PhotosGetInfo(videoId);

            Assert.That(info, Is.Not.Null);
            Assert.That(info.PhotoId, Is.EqualTo(videoId));
        }

        [Test]
        public void TestPhotoNotFound()
        {
            Should.Throw< PhotoNotFoundException>(() => Instance.PhotosGetInfo("abcd"));
        }

        [Test]
        public void TestPhotoNotFoundAsync()
        {
            var w = new AsyncSubject<FlickrResult<PhotoInfo>>();

            Instance.PhotosGetInfoAsync("abcd", r => { w.OnNext(r); w.OnCompleted(); });
            var result = w.Next().First();

            result.HasError.ShouldBeTrue();
            result.Error.ShouldBeOfType<PhotoNotFoundException>();
        }

        [Test]
        public void ShouldReturnPhotoInfoWithGeoData()
        {
            var info = Instance.PhotosGetInfo("54071193");

            Assert.That(info, Is.Not.Null, "PhotoInfo should not be null.");
            Assert.That(info.Location, Is.Not.Null, "Location should not be null.");
            Assert.That(info.Location.Longitude, Is.EqualTo(-180), "Longitude should be -180");
            Assert.That(info.Urls[0].Url, Is.EqualTo("https://www.flickr.com/photos/afdn/54071193/"));
            Assert.That(info.GeoPermissions?.IsPublic, Is.True, "GeoPermissions should be public.");
        }

        [Test]
        public void ShouldReturnPhotoInfoWithValidUrls()
        {
            var info = Instance.PhotosGetInfo("9671143400");

            Assert.That(UrlHelper.Exists(info.Small320Url), Is.True, "Small320Url is not valid url : " + info.Small320Url);
            Assert.That(UrlHelper.Exists(info.Medium640Url), Is.True, "Medium640Url is not valid url : " + info.Medium640Url);
            Assert.That(UrlHelper.Exists(info.Medium800Url), Is.True, "Medium800Url is not valid url : " + info.Medium800Url);
            Assert.That(info.LargeUrl, Is.Not.EqualTo(info.SmallUrl), "URLs should all be different.");
        }

        [Test]
        [Ignore("Photo urls appear to have changed to start with 'live' so test is invalid")]
        public void PhotoInfoUrlsShouldMatchSizes()
        {
            var photos =
                Instance.PhotosSearch(new PhotoSearchOptions
                                          {
                                              UserId = TestData.TestUserId,
                                              PerPage = 1,
                                              Extras = PhotoSearchExtras.AllUrls
                                          });

            var photo = photos.First();
            Assert.That(photo.PhotoId, Is.Not.Null);

            var info = Instance.PhotosGetInfo(photo.PhotoId);

            Assert.That(info.LargeUrl, Is.EqualTo(photo.LargeUrl));
            Assert.That(info.Small320Url, Is.EqualTo(photo.Small320Url));
        }

        [Test]
        [TestCase("46611802@N00", "")]
        public void GetInfoWithInvalidXmlTests(string userId, string location)
        {
            var userInfo = Instance.PeopleGetInfo(userId);
            Assert.That(userInfo.UserId, Is.EqualTo(userId));
            Assert.That(userInfo.Location, Is.EqualTo(location));
        }

    }
}
