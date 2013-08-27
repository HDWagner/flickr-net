﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using FlickrNet;

namespace FlickrNetTest
{
    [TestClass]
    public class PhotosSearchTests
    {
        Flickr f = new Flickr();

        [TestInitialize]
        public void TestInitialize()
        {
            Flickr.CacheDisabled = true;
            f.ApiKey = TestData.ApiKey;
        }

        [TestMethod]
        public void PhotosSearchBasicSearch()
        {
            PhotoSearchOptions o = new PhotoSearchOptions();
            o.Tags = "Test";
            PhotoCollection photos = f.PhotosSearch(o);

            Assert.IsTrue(photos.Total > 0, "Total Photos should be greater than zero.");
            Assert.IsTrue(photos.Pages > 0, "Pages should be greaters than zero.");
            Assert.AreEqual(100, photos.PerPage, "PhotosPerPage should be 100.");
            Assert.AreEqual(1, photos.Page, "Page should be 1.");

            Assert.IsTrue(photos.Count > 0, "Photos.Count should be greater than 0.");
            Assert.AreEqual(photos.PerPage, photos.Count);
        }

        [TestMethod]
        public void PhotosSearchSignedTest()
        {
            Flickr f = TestData.GetSignedInstance();
            PhotoSearchOptions o = new PhotoSearchOptions();
            o.Tags = "Test";
            o.PerPage = 5;
            PhotoCollection photos = f.PhotosSearch(o);

            Assert.AreEqual(5, photos.PerPage, "PerPage should equal 5.");
        }

        [TestMethod]
        public void PhotosSearchFavorites()
        {
            Flickr f = TestData.GetAuthInstance();
            PhotoSearchOptions o = new PhotoSearchOptions();
            o.UserId = "me";
            o.Faves = true;
            o.Tags = "cat";

            PhotoCollection p = f.PhotosSearch(o);

            Assert.IsTrue(p.Count > 10, "Should have returned more than 10 result returned. Count = " + p.Count);
            Assert.IsTrue(p.Count < 100, "Should be less than 100 results returned. Count = " + p.Count);
        }

        [TestMethod]
        public void PhotosSearchCameraIphone()
        {
            Flickr f = TestData.GetAuthInstance();

            PhotoSearchOptions o = new PhotoSearchOptions();
            o.Camera = "iPhone 4";
            o.MinUploadDate = DateTime.Now.AddDays(-2);
            o.MaxUploadDate = DateTime.Now;
            o.Extras = PhotoSearchExtras.Tags;

            var ps = f.PhotosSearch(o);

            foreach (Photo p in ps)
            {
                Console.WriteLine(p.WebUrl);
            }
        }

        [TestMethod]
        public void PhotosSearchPerPage()
        {
            PhotoSearchOptions o = new PhotoSearchOptions();
            o.PerPage = 10;
            o.Tags = "Test";
            PhotoCollection photos = f.PhotosSearch(o);

            Assert.IsTrue(photos.Total > 0, "TotalPhotos should be greater than 0.");
            Assert.IsTrue(photos.Pages > 0, "TotalPages should be greater than 0.");
            Assert.AreEqual(10, photos.PerPage, "PhotosPerPage should be 10.");
            Assert.AreEqual(1, photos.Page, "PageNumber should be 1.");
            Assert.AreEqual(10, photos.Count, "Count should be 10.");
            Assert.AreEqual(photos.PerPage, photos.Count);
        }

        [TestMethod]
        public void PhotosSearchUserIdTest()
        {
            PhotoSearchOptions o = new PhotoSearchOptions();
            o.UserId = TestData.TestUserId;

            PhotoCollection photos = f.PhotosSearch(o);

            foreach (Photo photo in photos)
            {
                Assert.AreEqual(TestData.TestUserId, photo.UserId);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ApiKeyRequiredException))]
        public void PhotosSearchNoApiKey()
        {
            f.ApiKey = "";
            f.PhotosSearch(new PhotoSearchOptions());

            Assert.Fail("Shouldn't get here");
        }

        [TestMethod]
        [Ignore()]
        public void PhotosSearchSortDataTakenAscending()
        {
            PhotoSearchOptions o = new PhotoSearchOptions();
            o.Tags = "microsoft";
            o.SortOrder = PhotoSearchSortOrder.DateTakenAscending;
            o.Extras = PhotoSearchExtras.DateTaken;

            PhotoCollection p = f.PhotosSearch(o);

            for (int i = 1; i < p.Count; i++)
            {
                Assert.AreNotEqual(default(DateTime), p[i].DateTaken);
                Assert.IsTrue(p[i].DateTaken >= p[i - 1].DateTaken, "Date taken should increase");
            }
        }

        [TestMethod]
        [Ignore()]
        public void PhotosSearchSortDataTakenDescending()
        {
            PhotoSearchOptions o = new PhotoSearchOptions();
            o.Tags = "microsoft";
            o.SortOrder = PhotoSearchSortOrder.DateTakenDescending;
            o.Extras = PhotoSearchExtras.DateTaken;

            PhotoCollection p = f.PhotosSearch(o);

            for (int i = 1; i < p.Count; i++)
            {
                Assert.AreNotEqual(default(DateTime), p[i].DateTaken);
                Assert.IsTrue(p[i].DateTaken <= p[i - 1].DateTaken, "Date taken should decrease.");
            }
        }

        [TestMethod]
        [Ignore()]
        public void PhotosSearchSortDataPostedAscending()
        {
            PhotoSearchOptions o = new PhotoSearchOptions();
            o.Tags = "microsoft";
            o.SortOrder = PhotoSearchSortOrder.DatePostedAscending;
            o.Extras = PhotoSearchExtras.DateUploaded;

            PhotoCollection p = f.PhotosSearch(o);

            for (int i = 1; i < p.Count; i++)
            {
                Assert.AreNotEqual(default(DateTime), p[i].DateUploaded);
                Assert.IsTrue(p[i].DateUploaded >= p[i - 1].DateUploaded, "Date taken should increase.");
            }
        }

        [TestMethod]
        [Ignore()]
        public void PhotosSearchSortDataPostedDescending()
        {
            PhotoSearchOptions o = new PhotoSearchOptions();
            o.Tags = "microsoft";
            o.SortOrder = PhotoSearchSortOrder.DatePostedDescending;
            o.Extras = PhotoSearchExtras.DateUploaded;

            PhotoCollection p = f.PhotosSearch(o);

            for (int i = 1; i < p.Count; i++)
            {
                Assert.AreNotEqual(default(DateTime), p[i].DateUploaded);
                Assert.IsTrue(p[i].DateUploaded <= p[i - 1].DateUploaded, "Date taken should increase.");
            }
        }

        [TestMethod]
        public void PhotosSearchGetLicenseNotNull()
        {
            PhotoSearchOptions o = new PhotoSearchOptions();
            o.Tags = "microsoft";
            o.SortOrder = PhotoSearchSortOrder.DatePostedDescending;
            o.Extras = PhotoSearchExtras.License;

            PhotoCollection photos = f.PhotosSearch(o);

            foreach (Photo photo in photos)
            {
                Assert.IsNotNull(photo.License);
            }
        }

        [TestMethod]
        public void PhotosSearchGetLicenseAttributionNoDerivs()
        {
            PhotoSearchOptions o = new PhotoSearchOptions();
            o.Tags = "microsoft";
            o.SortOrder = PhotoSearchSortOrder.DatePostedDescending;
            o.Licenses.Add(LicenseType.AttributionNoDerivativesCC);
            o.Extras = PhotoSearchExtras.License;

            PhotoCollection photos = f.PhotosSearch(o);

            foreach (Photo photo in photos)
            {
                Assert.AreEqual(LicenseType.AttributionNoDerivativesCC, photo.License);
            }
        }

        [TestMethod]
        public void PhotosSearchGetMultipleLicenses()
        {
            PhotoSearchOptions o = new PhotoSearchOptions();
            o.Tags = "microsoft";
            o.PerPage = 500;
            o.SortOrder = PhotoSearchSortOrder.DatePostedDescending;
            o.Licenses.Add(LicenseType.AttributionNoDerivativesCC);
            o.Licenses.Add(LicenseType.AttributionNoncommercialNoDerivativesCC);
            o.Extras = PhotoSearchExtras.License | PhotoSearchExtras.OwnerName;

            PhotoCollection photos = f.PhotosSearch(o);

            Console.WriteLine(f.LastRequest);

            foreach (Photo photo in photos)
            {
                if (photo.License != LicenseType.AttributionNoDerivativesCC &&
                    photo.License != LicenseType.AttributionNoncommercialNoDerivativesCC)
                {
                    Assert.Fail("License not one of selected licenses: " + photo.License.ToString());
                }
            }
        }

        [TestMethod]
        public void PhotosSearchGetLicenseNoKnownCopright()
        {
            PhotoSearchOptions o = new PhotoSearchOptions();
            o.Tags = "microsoft";
            o.SortOrder = PhotoSearchSortOrder.DatePostedDescending;
            o.Licenses.Add(LicenseType.NoKnownCopyrightRestrictions);
            o.Extras = PhotoSearchExtras.License;

            PhotoCollection photos = f.PhotosSearch(o);

            foreach (Photo photo in photos)
            {
                Assert.AreEqual(LicenseType.NoKnownCopyrightRestrictions, photo.License);
            }
        }

        [TestMethod]
        public void PhotosSearchSearchTwice()
        {
            PhotoSearchOptions o = new PhotoSearchOptions();
            o.Tags = "microsoft";
            o.PerPage = 10;

            PhotoCollection photos = f.PhotosSearch(o);

            Assert.AreEqual(10, photos.PerPage, "Per page is not 10");

            o.PerPage = 50;
            photos = f.PhotosSearch(o);
            Assert.AreEqual(50, photos.PerPage, "Per page has not changed?");

            photos = f.PhotosSearch(o);
            Assert.AreEqual(50, photos.PerPage, "Per page has changed!");
        }

        [TestMethod]
        public void PhotosSearchPageTest()
        {
            PhotoSearchOptions o = new PhotoSearchOptions();
            o.Tags = "colorful";
            o.PerPage = 10;
            o.Page = 3;

            PhotoCollection photos = f.PhotosSearch(o);

            Assert.AreEqual(3, photos.Page);
        }

        [TestMethod()]
        public void PhotosSearchIsCommons()
        {
            PhotoSearchOptions o = new PhotoSearchOptions();
            o.IsCommons = true;
            o.Tags = "newyork";
            o.PerPage = 10;
            o.Extras = PhotoSearchExtras.License;

            PhotoCollection photos = f.PhotosSearch(o);

            foreach (Photo photo in photos)
            {
                Assert.AreEqual(LicenseType.NoKnownCopyrightRestrictions, photo.License);
            }
        }

        [TestMethod]
        public void PhotosSearchDateTakenGranualityTest()
        {
            Flickr f = TestData.GetInstance();
            PhotoSearchOptions o = new PhotoSearchOptions();
            o.UserId = "8748614@N05";
            o.Tags = "primavera";
            o.PerPage = 500;
            o.Extras = PhotoSearchExtras.DateTaken;

            PhotoCollection photos = f.PhotosSearch(o);
        }

        [TestMethod]
        public void PhotosSearchDetailedTest()
        {
            PhotoSearchOptions o = new PhotoSearchOptions();
            o.Tags = "applestore";
            o.UserId = "41888973@N00";
            //o.Text = "Apple Store";
            o.Extras = PhotoSearchExtras.All;
            PhotoCollection photos = f.PhotosSearch(o);

            Assert.AreEqual(100, photos.PerPage);
            Assert.AreEqual(1, photos.Page);

            Assert.AreNotEqual(0, photos.Count, "PhotoCollection.Count should not be zero.");

            Assert.AreEqual("3547139066", photos[0].PhotoId);
            Assert.AreEqual("41888973@N00", photos[0].UserId);
            Assert.AreEqual("38560d3e1d", photos[0].Secret);
            Assert.AreEqual("3311", photos[0].Server);
            Assert.AreEqual("Apple Store!", photos[0].Title);
            Assert.AreEqual("4", photos[0].Farm);
            Assert.AreEqual(false, photos[0].IsFamily);
            Assert.AreEqual(true, photos[0].IsPublic);
            Assert.AreEqual(false, photos[0].IsFriend);

            DateTime dateTaken = new DateTime(2009, 5, 19, 22, 21, 46);
            DateTime dateUploaded = new DateTime(2009, 5, 19, 21, 21, 46);
            Assert.IsTrue(photos[0].LastUpdated > dateTaken, "Last updated date was not correct.");
            Assert.AreEqual(dateTaken, photos[0].DateTaken, "Date taken date was not correct.");
            Assert.AreEqual(dateUploaded, photos[0].DateUploaded, "Date uploaded date was not correct.");

            Assert.AreEqual("jpg", photos[0].OriginalFormat, "OriginalFormat should be JPG");
            Assert.AreEqual("JjXZOYpUV7IbeGVOUQ", photos[0].PlaceId, "PlaceID not set correctly.");

            Assert.IsNotNull(photos[0].Description, "Description should not be null.");

            foreach (Photo photo in photos)
            {
                Assert.IsNotNull(photo.PhotoId);
                Assert.IsTrue(photo.IsPublic);
                Assert.IsFalse(photo.IsFamily);
                Assert.IsFalse(photo.IsFriend);
            }
        }

        [TestMethod]
        public void PhotosSearchTagsTest()
        {
            PhotoSearchOptions o = new PhotoSearchOptions();
            o.PerPage = 10;
            o.Tags = "test";
            o.Extras = PhotoSearchExtras.Tags;

            PhotoCollection photos = f.PhotosSearch(o);

            Assert.IsTrue(photos.Total > 0);
            Assert.IsTrue(photos.Pages > 0);
            Assert.AreEqual(10, photos.PerPage, "PerPage should be 10.");
            Assert.AreEqual(10, photos.Count, "Count should be 10.");
            Assert.AreEqual(1, photos.Page, "Page should be 1.");

            foreach (Photo photo in photos)
            {
                Assert.IsTrue(photo.Tags.Count > 0, "Should be some tags");
                Assert.IsTrue(photo.Tags.Contains("test"), "At least one should be 'test'");
            }
        }

        // Flickr sometimes returns different totals for the same search when a different perPage value is used.
        // As I have no control over this, and I am correctly setting the properties as returned I am ignoring this test.
        [TestMethod]
        [Ignore]
        public void PhotosSearchPerPageMultipleTest()
        {
            PhotoSearchOptions o = new PhotoSearchOptions();
            o.Tags = "microsoft";
            o.Licenses.Add(LicenseType.AttributionCC);
            o.Licenses.Add(LicenseType.AttributionNoDerivativesCC);
            o.Licenses.Add(LicenseType.AttributionNoncommercialCC);
            o.Licenses.Add(LicenseType.AttributionNoncommercialNoDerivativesCC);
            o.Licenses.Add(LicenseType.AttributionNoncommercialShareAlikeCC);
            o.Licenses.Add(LicenseType.AttributionShareAlikeCC);

            o.MinUploadDate = DateTime.Today.AddDays(-4);
            o.MaxUploadDate = DateTime.Today.AddDays(-2);

            o.PerPage = 1;

            PhotoCollection photos = f.PhotosSearch(o);

            int totalPhotos1 = photos.Total;

            o.PerPage = 10;

            photos = f.PhotosSearch(o);

            int totalPhotos2 = photos.Total;

            o.PerPage = 100;

            photos = f.PhotosSearch(o);

            int totalPhotos3 = photos.Total;

            Assert.AreEqual(totalPhotos1, totalPhotos2, "Total Photos 1 & 2 should be equal");
            Assert.AreEqual(totalPhotos2, totalPhotos3, "Total Photos 2 & 3 should be equal");
        }

        [TestMethod]
        public void PhotosSearchPhotoSearchBoundaryBox()
        {
            BoundaryBox b = new BoundaryBox(103.675997, 1.339811, 103.689456, 1.357764, GeoAccuracy.World);
            PhotoSearchOptions o = new PhotoSearchOptions();
            o.HasGeo = true;
            o.BoundaryBox = b;
            o.Accuracy = b.Accuracy;
            o.MinUploadDate = DateTime.Now.AddYears(-1);
            o.MaxUploadDate = DateTime.Now;
            o.Extras = PhotoSearchExtras.Geo;

            PhotoCollection ps = f.PhotosSearch(o);

            foreach (Photo p in ps)
            {
                // Annoying, but sometimes Flickr doesn't return the geo properties for a photo even for this type of search.
                if (p.Latitude == 0 && p.Longitude == 0) continue;

                Assert.IsTrue(p.Latitude > b.MinimumLatitude && b.MaximumLatitude > p.Latitude, "Latitude is not within the boundary box.");
                Assert.IsTrue(p.Longitude > b.MinimumLongitude && b.MaximumLongitude > p.Longitude, "Longitude is not within the boundary box.");
            }

        }

        [TestMethod]
        public void PhotosSearchLatCultureTest()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("nb-NO");

            PhotoSearchOptions o = new PhotoSearchOptions();
            o.HasGeo = true;
            o.Extras |= PhotoSearchExtras.Geo;
            o.Tags = "colorful";
            o.TagMode = TagMode.AllTags;
            o.PerPage = 10;

            PhotoCollection photos = f.PhotosSearch(o);
        }


        [TestMethod]
        public void PhotosSearchTagCollectionTest()
        {
            PhotoSearchOptions o = new PhotoSearchOptions();
            o.UserId = TestData.TestUserId;
            o.PerPage = 10;
            o.Extras = PhotoSearchExtras.Tags;

            PhotoCollection photos = f.PhotosSearch(o);

            foreach (Photo p in photos)
            {
                Assert.IsNotNull(p.Tags, "Tag Collection should not be null");
                Assert.IsTrue(p.Tags.Count > 0, "Should be more than one tag for all photos");
                Assert.IsNotNull(p.Tags[0]);
            }
        }

        [TestMethod]
        public void PhotosSearchMultipleTagsTest()
        {
            PhotoSearchOptions o = new PhotoSearchOptions();
            o.Tags = "art,collection";
            o.TagMode = TagMode.AllTags;
            o.PerPage = 10;
            o.Extras = PhotoSearchExtras.Tags;

            PhotoCollection photos = f.PhotosSearch(o);

            foreach (Photo p in photos)
            {
                Assert.IsNotNull(p.Tags, "Tag Collection should not be null");
                Assert.IsTrue(p.Tags.Count > 0, "Should be more than one tag for all photos");
                Assert.IsTrue(p.Tags.Contains("art"), "Should contain 'art' tag.");
                Assert.IsTrue(p.Tags.Contains("collection"), "Should contain 'collection' tag.");
            }
        }

        [TestMethod]
        public void PhotosSearchInterestingnessBasicTest()
        {
            var flickr = TestData.GetInstance();
            var o = new PhotoSearchOptions
                        {
                            SortOrder = PhotoSearchSortOrder.InterestingnessDescending,
                            Tags = "colorful",
                            PerPage = 500
                        };

            var ps = flickr.PhotosSearch(o);

            Assert.IsNotNull(ps, "Photos should not be null");
            Assert.AreEqual(500, ps.PerPage, "PhotosPerPage should be 500");
            // Due to instabilities in the Flickr API at the moment the following assert would likely fail.
            //Assert.AreEqual(500, ps.Count, "Count should be 500 as well");
            Assert.AreNotEqual(0, ps.Count, "Count should be greater than zero.");
        }

        [TestMethod]
        public void PhotosSearchGroupIdTest()
        {
            PhotoSearchOptions o = new PhotoSearchOptions();
            o.GroupId = TestData.GroupId;
            o.PerPage = 10;

            var photos = TestData.GetInstance().PhotosSearch(o);

            Assert.IsNotNull(photos);
            Assert.AreNotEqual<int>(0, photos.Count);

            foreach (var photo in photos)
            {
                Assert.IsNotNull(photo.PhotoId);
            }
        }

        [TestMethod]
        public void PhotosSearchGoeContext()
        {
            Flickr f = TestData.GetInstance();

            var o = new PhotoSearchOptions
                        {
                            HasGeo = true,
                            GeoContext = GeoContext.Outdoors,
                            Tags = "landscape"
                        };
                
            o.Extras |= PhotoSearchExtras.Geo;

            var col = f.PhotosSearch(o);

            foreach (var p in col)
            {
                Assert.AreEqual(GeoContext.Outdoors, p.GeoContext);
            }
        }

        [TestMethod]
        public void PhotosSearchLatLongGeoRadiusTest()
        {
            Flickr f = TestData.GetInstance();

            PhotoSearchOptions o = new PhotoSearchOptions();
            o.HasGeo = true;
            o.MinTakenDate = DateTime.Today.AddYears(-3);
            o.PerPage = 10;
            o.Latitude = 61.600447;
            o.Longitude = 5.035064;
            o.Radius = 4.75f;
            o.RadiusUnits = RadiusUnit.Kilometers;
            o.Extras |= PhotoSearchExtras.Geo;

            var photos = f.PhotosSearch(o);

            Assert.IsNotNull(photos);
            Assert.AreNotEqual<int>(0, photos.Count, "No photos returned by search.");

            foreach (var photo in photos)
            {
                Assert.IsNotNull(photo.PhotoId);
                Assert.AreNotEqual<double>(0, photo.Longitude, "Longitude should not be zero.");
                Assert.AreNotEqual<double>(0, photo.Latitude, "Latitude should not be zero.");
            }
        }

        [TestMethod]
        public void PhotosSearchLargeRadiusTest()
        {
            double lat = 61.600447;
            double lon = 5.035064;
            Flickr f = TestData.GetInstance();

            PhotoSearchOptions o = new PhotoSearchOptions();
            o.PerPage = 100;
            o.HasGeo = true;
            o.MinTakenDate = DateTime.Today.AddYears(-3);
            o.Latitude = lat;
            o.Longitude = lon;
            o.Radius = 5.432123456f;
            o.RadiusUnits = RadiusUnit.Kilometers;
            o.Extras |= PhotoSearchExtras.Geo;

            var photos = f.PhotosSearch(o);

            Assert.IsNotNull(photos);
            Assert.AreNotEqual<int>(0, photos.Count, "No photos returned by search.");

            foreach (var photo in photos)
            {
                Assert.IsNotNull(photo.PhotoId);
                Assert.AreNotEqual<double>(0, photo.Longitude, "Longitude should not be zero.");
                Assert.AreNotEqual<double>(0, photo.Latitude, "Latitude should not be zero.");

                Console.WriteLine("Photo {0}, Lat={1}, Long={2}", photo.PhotoId, photo.Latitude, photo.Longitude);

                // Note: +/-1 is not an exact match to 5.4km, but anything outside of these bounds is definitely wrong.
                Assert.IsTrue(photo.Latitude > lat - 1 && photo.Latitude < lat + 1, "Latitude not within acceptable range.");
                Assert.IsTrue(photo.Longitude > lon - 1 && photo.Longitude < lon + 1, "Latitude not within acceptable range.");

            }
        }

        [TestMethod]
        public void PhotosSearchFullParamTest()
        {
            PhotoSearchOptions o = new PhotoSearchOptions();
            o.UserId = TestData.TestUserId;
            o.Tags = "microsoft";
            o.TagMode = TagMode.AllTags;
            o.Text = "microsoft";
            o.MachineTagMode = MachineTagMode.AllTags;
            o.MachineTags = "dc:author=*";
            o.MinTakenDate = DateTime.Today.AddYears(-1);
            o.MaxTakenDate = DateTime.Today;
            o.PrivacyFilter = PrivacyFilter.PublicPhotos;
            o.SafeSearch = SafetyLevel.Safe;
            o.ContentType = ContentTypeSearch.PhotosOnly;
            o.HasGeo = false;
            o.WoeId = "30079";
            o.PlaceId = "X9sTR3BSUrqorQ";

            var photos = TestData.GetInstance().PhotosSearch(o);

            Assert.IsNotNull(photos);
            Assert.AreEqual(0, photos.Count);

        }

        [TestMethod]
        public void PhotosSearchGalleryPhotos()
        {
            PhotoSearchOptions o = new PhotoSearchOptions();
            o.UserId = TestData.TestUserId;
            o.InGallery = true;
            o.Tags = "art";
            PhotoCollection photos = TestData.GetInstance().PhotosSearch(o);

            Assert.AreEqual(1, photos.Count, "Only one photo should have been returned.");
        }

        [TestMethod]
        public void PhotosSearchUrlLimitTest()
        {
            PhotoSearchOptions o = new PhotoSearchOptions();
            o.Extras = PhotoSearchExtras.All;
            o.TagMode = TagMode.AnyTag;
            StringBuilder sb = new StringBuilder();
            for (int i = 1; i < 200; i++) sb.Append("tagnumber" + i);
            o.Tags = sb.ToString();

            PhotoCollection photos = TestData.GetInstance().PhotosSearch(o);
        }

        [TestMethod]
        public void PhotosSearchRussianCharacters()
        {
            Flickr f = TestData.GetInstance();

            PhotoSearchOptions o = new PhotoSearchOptions();
            o.Tags = "снег";

            var photos = f.PhotosSearch(o);

            Console.WriteLine(f.LastRequest);

            Assert.AreNotEqual(0, photos.Count, "Search should return some results.");
        }

        [TestMethod]
        public void PhotosSearchRussianTagsReturned()
        {
            Flickr f = TestData.GetInstance();

            var o = new PhotoSearchOptions();
            o.UserId = "31828860@N08";
            o.Extras = PhotoSearchExtras.Tags;

            var photos = f.PhotosSearch(o);

            foreach (var photo in photos)
            {
                Console.WriteLine(photo.Title + " = " + String.Join(",", photo.Tags));
            }

        }

        [TestMethod]
        public void PhotosSearchAuthRussianCharacters()
        {
            Flickr f = TestData.GetAuthInstance();

            PhotoSearchOptions o = new PhotoSearchOptions();
            o.Tags = "снег";

            var photos = f.PhotosSearch(o);

            Console.WriteLine(f.LastRequest);

            Assert.AreNotEqual(0, photos.Count, "Search should return some results.");
        }

        [TestMethod]
        public void PhotosSearchRotation()
        {
            Flickr f = TestData.GetInstance();
            PhotoSearchOptions o = new PhotoSearchOptions() { Extras = PhotoSearchExtras.Rotation, UserId = TestData.TestUserId, PerPage = 100 };
            var photos = f.PhotosSearch(o);
            foreach (var photo in photos)
            {
                Assert.IsTrue(photo.Rotation.HasValue, "Rotation should be set.");
                if (photo.PhotoId == "6861439677")
                    Assert.AreEqual(90, photo.Rotation, "Rotation should be 90 for this photo.");
                if (photo.PhotoId == "6790104907")
                    Assert.AreEqual(0, photo.Rotation, "Rotation should be 0 for this photo.");
            }
        }

    }
}
