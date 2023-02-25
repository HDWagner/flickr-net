using FlickrNet;
using FlickrNet.Classes;
using FlickrNet.Exceptions;
using FlickrNetTest.TestUtilities;
using NUnit.Framework;
using Shouldly;
using System.Text;

namespace FlickrNetTest
{
    [TestFixture]
    public class PhotosSearchTests : BaseTest
    {
        [Test]
        public void PhotosSearchBasicSearch()
        {
            var o = new PhotoSearchOptions { Tags = "Test" };
            var photos = Instance.PhotosSearch(o);

            Assert.That(photos.Total, Is.GreaterThan(0), "Total Photos should be greater than zero.");
            Assert.That(photos.Pages, Is.GreaterThan(0), "Pages should be greaters than zero.");
            Assert.That(photos.PerPage, Is.EqualTo(100), "PhotosPerPage should be 100.");
            Assert.That(photos.Page, Is.EqualTo(1), "Page should be 1.");

            Assert.That(photos, Is.Not.Empty, "Photos.Count should be greater than 0.");
            Assert.That(photos, Has.Count.EqualTo(photos.PerPage));
        }

        [Test]
        public void PhotosSearchSignedTest()
        {
            Flickr f = TestData.GetSignedInstance();
            var o = new PhotoSearchOptions { Tags = "Test", PerPage = 5 };
            PhotoCollection photos = f.PhotosSearch(o);

            Assert.That(photos.PerPage, Is.EqualTo(5), "PerPage should equal 5.");
        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void PhotosSearchFavorites()
        {
            var o = new PhotoSearchOptions { UserId = "me", Faves = true, Tags = "cat" };

            PhotoCollection p = AuthInstance.PhotosSearch(o);

            Assert.That(p, Has.Count.GreaterThan(1), "Should have returned more than 1 result returned. Count = " + p.Count);
            Assert.That(p, Has.Count.LessThan(100), "Should be less than 100 results returned. Count = " + p.Count);
        }

        [Test, Ignore("Currently 'Camera' searches are not working.")]
        [Category("AccessTokenRequired")]
        public void PhotosSearchCameraIphone()
        {
            var o = new PhotoSearchOptions
            {
                Camera = "iPhone 5S",
                MinUploadDate = DateTime.Now.AddDays(-7),
                MaxUploadDate = DateTime.Now,
                Extras = PhotoSearchExtras.Tags
            };

            var ps = AuthInstance.PhotosSearch(o);

            Assert.That(ps, Is.Not.Null);
            Assert.That(ps, Is.Not.Empty);
        }

        [Test]
        public void PhotoSearchByPathAlias()
        {
            var o = new PhotoSearchOptions
            {
                GroupPathAlias = "api",
                PerPage = 10
            };

            var ps = Instance.PhotosSearch(o);

            Assert.That(ps, Is.Not.Null);
            Assert.That(ps, Is.Not.Empty);
        }

        [Test]
        public void PhotosSearchPerPage()
        {
            var o = new PhotoSearchOptions { PerPage = 10, Tags = "Test" };
            var photos = Instance.PhotosSearch(o);

            Assert.That(photos.Total, Is.GreaterThan(0), "TotalPhotos should be greater than 0.");
            Assert.That(photos.Pages, Is.GreaterThan(0), "TotalPages should be greater than 0.");
            Assert.That(photos.PerPage, Is.EqualTo(10), "PhotosPerPage should be 10.");
            Assert.That(photos.Page, Is.EqualTo(1), "PageNumber should be 1.");
            Assert.That(photos, Has.Count.EqualTo(10), "Count should be 10.");
            Assert.That(photos, Has.Count.EqualTo(photos.PerPage));
        }

        [Test]
        public void PhotosSearchUserIdTest()
        {
            var o = new PhotoSearchOptions { UserId = TestData.TestUserId };

            PhotoCollection photos = Instance.PhotosSearch(o);

            foreach (Photo photo in photos)
            {
                Assert.That(photo.UserId, Is.EqualTo(TestData.TestUserId));
            }
        }

        [Test]
        public void PhotosSearchNoApiKey()
        {
            Instance.ApiKey = "";
            Should.Throw<ApiKeyRequiredException>(() => Instance.PhotosSearch(new PhotoSearchOptions()));
        }

        [Test]
        public void GetOauthRequestTokenNoApiKey()
        {
            Instance.ApiKey = "";
            Should.Throw<ApiKeyRequiredException>(() => Instance.OAuthGetRequestToken("oob"));
        }

        [Test]
        [Ignore("Flickr still doesn't seem to sort correctly by date posted.")]
        public void PhotosSearchSortDateTakenAscending()
        {
            var o = new PhotoSearchOptions
            {
                Tags = "microsoft",
                SortOrder = PhotoSearchSortOrder.DateTakenAscending,
                Extras = PhotoSearchExtras.DateTaken
            };

            var p = Instance.PhotosSearch(o);

            for (var i = 1; i < p.Count; i++)
            {
                Assert.That(p[i].DateTaken, Is.Not.EqualTo(default(DateTime)));
                Assert.That(p[i].DateTaken, Is.GreaterThanOrEqualTo(p[i - 1].DateTaken), "Date taken should increase");
            }
        }

        [Test]
        [Ignore("Flickr still doesn't seem to sort correctly by date posted.")]
        public void PhotosSearchSortDateTakenDescending()
        {
            var o = new PhotoSearchOptions
            {
                Tags = "microsoft",
                SortOrder = PhotoSearchSortOrder.DateTakenDescending,
                Extras = PhotoSearchExtras.DateTaken
            };

            var p = Instance.PhotosSearch(o);

            for (var i = 1; i < p.Count; i++)
            {
                Assert.That(p[i].DateTaken, Is.Not.EqualTo(default(DateTime)));
                Assert.That(p[i].DateTaken, Is.LessThanOrEqualTo(p[i - 1].DateTaken), "Date taken should decrease.");
            }
        }

        [Test]
        [Ignore("Flickr still doesn't seem to sort correctly by date posted.")]
        public void PhotosSearchSortDatePostedAscending()
        {
            var o = new PhotoSearchOptions
            {
                Tags = "microsoft",
                SortOrder = PhotoSearchSortOrder.DatePostedAscending,
                Extras = PhotoSearchExtras.DateUploaded
            };

            var p = Instance.PhotosSearch(o);

            for (var i = 1; i < p.Count; i++)
            {
                Assert.That(p[i].DateUploaded, Is.Not.EqualTo(default(DateTime)));
                Assert.That(p[i].DateUploaded, Is.GreaterThanOrEqualTo(p[i - 1].DateUploaded), "Date taken should increase.");
            }
        }

        [Test]
        [Ignore("Flickr still doesn't seem to sort correctly by date posted.")]
        public void PhotosSearchSortDataPostedDescending()
        {
            var o = new PhotoSearchOptions
            {
                Tags = "microsoft",
                SortOrder = PhotoSearchSortOrder.DatePostedDescending,
                Extras = PhotoSearchExtras.DateUploaded
            };

            var p = Instance.PhotosSearch(o);

            for (int i = 1; i < p.Count; i++)
            {
                Assert.That(p[i].DateUploaded, Is.Not.EqualTo(default(DateTime)));
                Assert.That(p[i].DateUploaded, Is.LessThanOrEqualTo(p[i - 1].DateUploaded), "Date taken should increase.");
            }
        }

        [Test]
        public void PhotosSearchGetLicenseNotNull()
        {
            var o = new PhotoSearchOptions
            {
                Tags = "microsoft",
                SortOrder = PhotoSearchSortOrder.DatePostedDescending,
                Extras = PhotoSearchExtras.License
            };

            PhotoCollection photos = Instance.PhotosSearch(o);

            foreach (Photo photo in photos)
            {
                Assert.That(photo.License, Is.Not.Null);
            }
        }

        [Test]
        public void PhotosSearchGetLicenseAttributionNoDerivs()
        {
            var o = new PhotoSearchOptions
            {
                Tags = "microsoft",
                SortOrder = PhotoSearchSortOrder.DatePostedDescending
            };
            o.Licenses.Add(LicenseType.AttributionNoDerivativesCC);
            o.Extras = PhotoSearchExtras.License;

            PhotoCollection photos = Instance.PhotosSearch(o);

            foreach (Photo photo in photos)
            {
                Assert.That(photo.License, Is.EqualTo(LicenseType.AttributionNoDerivativesCC));
            }
        }

        [Test]
        public void PhotosSearchGetMultipleLicenses()
        {
            var o = new PhotoSearchOptions
            {
                Tags = "microsoft",
                PerPage = 500,
                SortOrder = PhotoSearchSortOrder.DatePostedDescending
            };
            o.Licenses.Add(LicenseType.AttributionNoDerivativesCC);
            o.Licenses.Add(LicenseType.AttributionNoncommercialNoDerivativesCC);
            o.Extras = PhotoSearchExtras.License | PhotoSearchExtras.OwnerName;

            PhotoCollection photos = Instance.PhotosSearch(o);

            foreach (Photo photo in photos)
            {
                if (photo.License != LicenseType.AttributionNoDerivativesCC &&
                    photo.License != LicenseType.AttributionNoncommercialNoDerivativesCC)
                {
                    Assert.Fail("License not one of selected licenses: " + photo.License.ToString());
                }
            }
        }

        [Test]
        public void PhotosSearchGetLicenseNoKnownCopright()
        {
            var o = new PhotoSearchOptions
            {
                Tags = "microsoft",
                SortOrder = PhotoSearchSortOrder.DatePostedDescending
            };
            o.Licenses.Add(LicenseType.NoKnownCopyrightRestrictions);
            o.Extras = PhotoSearchExtras.License;

            PhotoCollection photos = Instance.PhotosSearch(o);

            foreach (Photo photo in photos)
            {
                Assert.That(photo.License, Is.EqualTo(LicenseType.NoKnownCopyrightRestrictions));
            }
        }

        [Test]
        public void PhotosSearchSearchTwice()
        {
            var o = new PhotoSearchOptions { Tags = "microsoft", PerPage = 10 };

            PhotoCollection photos = Instance.PhotosSearch(o);

            Assert.That(photos.PerPage, Is.EqualTo(10), "Per page is not 10");

            o.PerPage = 50;
            photos = Instance.PhotosSearch(o);
            Assert.That(photos.PerPage, Is.EqualTo(50), "Per page has not changed?");

            photos = Instance.PhotosSearch(o);
            Assert.That(photos.PerPage, Is.EqualTo(50), "Per page has changed!");
        }

        [Test]
        public void PhotosSearchPageTest()
        {
            var o = new PhotoSearchOptions { Tags = "colorful", PerPage = 10, Page = 3 };

            PhotoCollection photos = Instance.PhotosSearch(o);

            Assert.That(photos.Page, Is.EqualTo(3));
        }

        [Test]
        public void PhotosSearchByColorCode()
        {
            var o = new PhotoSearchOptions
            {
                ColorCodes = new List<string> { "orange" },
                Tags = "colorful"
            };

            var photos = Instance.PhotosSearch(o);

            Assert.That(photos, Is.Not.Null);
            Assert.That(photos, Is.Not.Empty, "Count should not be zero.");

            foreach (var photo in photos)
            {
                Console.WriteLine(photo.WebUrl);
            }
        }

        [TestCase(Style.BlackAndWhite)]
        [TestCase(Style.DepthOfField)]
        [TestCase(Style.Minimalism)]
        [TestCase(Style.Pattern)]
        public void PhotoSearchByStyles(Style style)
        {
            var o = new PhotoSearchOptions
            {
                Text = "nature",
                Page = 1,
                PerPage = 10,
                Styles = new[] { style }
            };

            var photos = Instance.PhotosSearch(o);

            Assert.That(photos, Is.Not.Null);
            Assert.That(photos, Is.Not.Empty);
        }

        [Test]
        public void PhotosSearchIsCommons()
        {
            var o = new PhotoSearchOptions
            {
                IsCommons = true,
                Tags = "newyork",
                PerPage = 10,
                Extras = PhotoSearchExtras.License
            };

            PhotoCollection photos = Instance.PhotosSearch(o);

            foreach (Photo photo in photos)
            {
                Assert.That(photo.License, Is.EqualTo(LicenseType.NoKnownCopyrightRestrictions));
            }
        }

        [Test]
        public void PhotosSearchDateTakenGranualityTest()
        {
            var o = new PhotoSearchOptions
            {
                UserId = "8748614@N05",
                Tags = "primavera",
                PerPage = 500,
                Extras = PhotoSearchExtras.DateTaken
            };

            var result = Instance.PhotosSearch(o);
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void PhotosSearchDetailedTest()
        {
            var o = new PhotoSearchOptions
            {
                Tags = "applestore",
                UserId = "41888973@N00",
                Extras = PhotoSearchExtras.All
            };
            PhotoCollection photos = Instance.PhotosSearch(o);

            Assert.That(photos.PerPage, Is.EqualTo(100));
            Assert.That(photos.Page, Is.EqualTo(1));

            Assert.That(photos, Is.Not.Empty, "PhotoCollection.Count should not be zero.");

            Assert.That(photos[0].PhotoId, Is.EqualTo("3547139066"));
            Assert.That(photos[0].UserId, Is.EqualTo("41888973@N00"));
            Assert.That(photos[0].Secret, Is.EqualTo("bf4e11b1cb"));
            Assert.That(photos[0].Server, Is.EqualTo("3311"));
            Assert.That(photos[0].Title, Is.EqualTo("Apple Store!"));
            Assert.That(photos[0].Farm, Is.EqualTo("4"));
            Assert.That(photos[0].IsFamily, Is.EqualTo(false));
            Assert.That(photos[0].IsPublic, Is.EqualTo(true));
            Assert.That(photos[0].IsFriend, Is.EqualTo(false));

            var dateTaken = new DateTime(2009, 5, 18, 4, 21, 46);
            var dateUploaded = new DateTime(2009, 5, 19, 21, 21, 46);
            Assert.That(photos[0].LastUpdated, Is.GreaterThan(dateTaken), "Last updated date was not correct.");
            Assert.That(photos[0].DateTaken, Is.EqualTo(dateTaken), "Date taken date was not correct.");
            Assert.That(photos[0].DateUploaded, Is.EqualTo(dateUploaded), "Date uploaded date was not correct.");

            Assert.That(photos[0].OriginalFormat, Is.EqualTo("jpg"), "OriginalFormat should be JPG");
            Assert.That(photos[0].PlaceId, Is.EqualTo("JjXZOYpUV7IbeGVOUQ"), "PlaceID not set correctly.");

            Assert.That(photos[0].Description, Is.Not.Null, "Description should not be null.");

            foreach (Photo photo in photos)
            {
                Assert.That(photo.PhotoId, Is.Not.Null);
                Assert.That(photo.IsPublic, Is.True);
                Assert.That(photo.IsFamily, Is.False);
                Assert.That(photo.IsFriend, Is.False);
            }
        }

        [Test]
        public void PhotosSearchTagsTest()
        {
            var o = new PhotoSearchOptions { PerPage = 10, Tags = "test", Extras = PhotoSearchExtras.Tags };

            PhotoCollection photos = Instance.PhotosSearch(o);

            photos.Total.ShouldBeGreaterThan(0);
            photos.Pages.ShouldBeGreaterThan(0);
            photos.PerPage.ShouldBe(10);
            photos.Page.ShouldBe(1);
            photos.Count.ShouldBeInRange(9, 10, "Ideally should be 10, but sometimes returns 9");

            foreach (Photo photo in photos)
            {
                Assert.That(photo.Tags, Is.Not.Empty, "Should be some tags");
                Assert.That(photo.Tags, Does.Contain("test"), "At least one should be 'test'");
            }
        }

        // Flickr sometimes returns different totals for the same search when a different perPage value is used.
        // As I have no control over this, and I am correctly setting the properties as returned I am ignoring this test.
        [Test]
        [Ignore("Flickr often returns different totals than requested.")]
        public void PhotosSearchPerPageMultipleTest()
        {
            var o = new PhotoSearchOptions { Tags = "microsoft" };
            o.Licenses.Add(LicenseType.AttributionCC);
            o.Licenses.Add(LicenseType.AttributionNoDerivativesCC);
            o.Licenses.Add(LicenseType.AttributionNoncommercialCC);
            o.Licenses.Add(LicenseType.AttributionNoncommercialNoDerivativesCC);
            o.Licenses.Add(LicenseType.AttributionNoncommercialShareAlikeCC);
            o.Licenses.Add(LicenseType.AttributionShareAlikeCC);

            o.MinUploadDate = DateTime.Today.AddDays(-4);
            o.MaxUploadDate = DateTime.Today.AddDays(-2);

            o.PerPage = 1;

            PhotoCollection photos = Instance.PhotosSearch(o);

            int totalPhotos1 = photos.Total;

            o.PerPage = 10;

            photos = Instance.PhotosSearch(o);

            int totalPhotos2 = photos.Total;

            o.PerPage = 100;

            photos = Instance.PhotosSearch(o);

            int totalPhotos3 = photos.Total;

            Assert.That(totalPhotos2, Is.EqualTo(totalPhotos1), "Total Photos 1 & 2 should be equal");
            Assert.That(totalPhotos3, Is.EqualTo(totalPhotos2), "Total Photos 2 & 3 should be equal");
        }

        [Test]
        public void PhotosSearchPhotoSearchBoundaryBox()
        {
            var b = new BoundaryBox(103.675997, 1.339811, 103.689456, 1.357764, GeoAccuracy.World);
            var o = new PhotoSearchOptions
            {
                HasGeo = true,
                BoundaryBox = b,
                Accuracy = b.Accuracy,
                MinUploadDate = DateTime.Now.AddYears(-1),
                MaxUploadDate = DateTime.Now,
                Extras = PhotoSearchExtras.Geo | PhotoSearchExtras.PathAlias,
                Tags = "colorful"
            };

            var ps = Instance.PhotosSearch(o);

            foreach (var p in ps)
            {
                // Annoying, but sometimes Flickr doesn't return the geo properties for a photo even for this type of search.
                if (Math.Abs(p.Latitude - 0) < 1e-8 && Math.Abs(p.Longitude - 0) < 1e-8)
                {
                    continue;
                }

                Assert.That(p.Latitude > b.MinimumLatitude && b.MaximumLatitude > p.Latitude, Is.True,
                              "Latitude is not within the boundary box. {0} outside {1} and {2} for photo {3}", p.Latitude, b.MinimumLatitude, b.MaximumLatitude, p.WebUrl);
                Assert.That(p.Longitude > b.MinimumLongitude && b.MaximumLongitude > p.Longitude, Is.True,
                              "Longitude is not within the boundary box. {0} outside {1} and {2} for photo {3}", p.Longitude, b.MinimumLongitude, b.MaximumLongitude, p.WebUrl);
            }
        }

        [Test]
        public void PhotosSearchLatCultureTest()
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("nb-NO");

            var o = new PhotoSearchOptions { HasGeo = true };
            o.Extras |= PhotoSearchExtras.Geo;
            o.Tags = "colorful";
            o.TagMode = TagMode.AllTags;
            o.PerPage = 10;

            var photos = Instance.PhotosSearch(o);

            Assert.That(photos, Is.Not.Null);
        }


        [Test]
        public void PhotosSearchTagCollectionTest()
        {
            var o = new PhotoSearchOptions
            {
                UserId = TestData.TestUserId,
                PerPage = 10,
                Extras = PhotoSearchExtras.Tags
            };

            PhotoCollection photos = Instance.PhotosSearch(o);

            foreach (Photo p in photos)
            {
                Assert.That(p.Tags, Is.Not.Null, "Tag Collection should not be null");
                Assert.That(p.Tags, Is.Not.Empty, "Should be more than one tag for all photos");
                Assert.That(p.Tags[0], Is.Not.Null);
            }
        }

        [Test]
        public void PhotosSearchMultipleTagsTest()
        {
            var o = new PhotoSearchOptions();
            o.Tags = "art,collection";
            o.TagMode = TagMode.AllTags;
            o.PerPage = 10;
            o.Extras = PhotoSearchExtras.Tags;

            PhotoCollection photos = Instance.PhotosSearch(o);

            foreach (Photo p in photos)
            {
                Assert.That(p.Tags, Is.Not.Null, "Tag Collection should not be null");
                Assert.That(p.Tags, Is.Not.Empty, "Should be more than one tag for all photos");
                Assert.That(p.Tags, Does.Contain("art"), "Should contain 'art' tag.");
                Assert.That(p.Tags, Does.Contain("collection"), "Should contain 'collection' tag.");
            }
        }

        [Test]
        public void PhotosSearchInterestingnessBasicTest()
        {
            var o = new PhotoSearchOptions
            {
                SortOrder = PhotoSearchSortOrder.InterestingnessDescending,
                Tags = "colorful",
                PerPage = 500
            };

            var ps = Instance.PhotosSearch(o);

            Assert.That(ps, Is.Not.Null, "Photos should not be null");
            Assert.That(ps.PerPage, Is.EqualTo(500), "PhotosPerPage should be 500");
            Assert.That(ps, Is.Not.Empty, "Count should be greater than zero.");
        }

        [Test]
        public void PhotosSearchGroupIdTest()
        {
            var o = new PhotoSearchOptions();
            o.GroupId = TestData.GroupId;
            o.PerPage = 10;

            var photos = Instance.PhotosSearch(o);

            Assert.That(photos, Is.Not.Null);
            Assert.That(photos, Is.Not.Empty);

            foreach (var photo in photos)
            {
                Assert.That(photo.PhotoId, Is.Not.Null);
            }
        }

        [Test]
        [Ignore("GeoContext filter doesn't appear to be working.")]
        public void PhotosSearchGeoContext()
        {
            var o = new PhotoSearchOptions
            {
                HasGeo = true,
                GeoContext = GeoContext.Outdoors,
                Tags = "landscape"
            };

            o.Extras |= PhotoSearchExtras.Geo;

            var col = Instance.PhotosSearch(o);

            foreach (var p in col)
            {
                Assert.That(p.GeoContext, Is.EqualTo(GeoContext.Outdoors));
            }
        }

        [Test]
        public void PhotosSearchLatLongGeoRadiusTest()
        {
            var o = new PhotoSearchOptions();
            o.HasGeo = true;
            o.MinTakenDate = DateTime.Today.AddYears(-3);
            o.PerPage = 10;
            o.Latitude = 61.600447;
            o.Longitude = 5.035064;
            o.Radius = 4.75f;
            o.RadiusUnits = RadiusUnit.Kilometers;
            o.Extras |= PhotoSearchExtras.Geo;

            var photos = Instance.PhotosSearch(o);

            Assert.That(photos, Is.Not.Null);
            Assert.That(photos, Is.Not.Empty, "No photos returned by search.");

            foreach (var photo in photos)
            {
                Assert.That(photo.PhotoId, Is.Not.Null);
                Assert.That(photo.Longitude, Is.Not.EqualTo(0), "Longitude should not be zero.");
                Assert.That(photo.Latitude, Is.Not.EqualTo(0), "Latitude should not be zero.");
            }
        }

        [Test]
        public void PhotosSearchLargeRadiusTest()
        {
            const double lat = 61.600447;
            const double lon = 5.035064;

            var o = new PhotoSearchOptions
            {
                PerPage = 100,
                HasGeo = true,
                MinTakenDate = DateTime.Today.AddYears(-3),
                Latitude = lat,
                Longitude = lon,
                Radius = 5.432123456f,
                RadiusUnits = RadiusUnit.Kilometers
            };
            o.Extras |= PhotoSearchExtras.Geo;

            var photos = Instance.PhotosSearch(o);

            Assert.That(photos, Is.Not.Null);
            Assert.That(photos, Is.Not.Empty, "No photos returned by search.");

            foreach (var photo in photos)
            {
                Assert.That(photo.PhotoId, Is.Not.Null);
                Assert.That(photo.Longitude, Is.Not.EqualTo(0), "Longitude should not be zero.");
                Assert.That(photo.Latitude, Is.Not.EqualTo(0), "Latitude should not be zero.");

                LogOnError("Photo ID " + photo.PhotoId,
                           string.Format("Lat={0}, Long={1}", photo.Latitude, photo.Longitude));

                // Note: +/-1 is not an exact match to 5.4km, but anything outside of these bounds is definitely wrong.
                Assert.That(photo.Latitude > lat - 1 && photo.Latitude < lat + 1, Is.True,
                              "Latitude not within acceptable range.");
                Assert.That(photo.Longitude > lon - 1 && photo.Longitude < lon + 1, Is.True,
                              "Latitude not within acceptable range.");

            }
        }

        [Test]
        [Ignore("WOE ID searches don't appear to be working.")]
        public void PhotosSearchFullParamTest()
        {
            var o = new PhotoSearchOptions
            {
                UserId = TestData.TestUserId,
                Tags = "microsoft",
                TagMode = TagMode.AllTags,
                Text = "microsoft",
                MachineTagMode = MachineTagMode.AllTags,
                MachineTags = "dc:author=*",
                MinTakenDate = DateTime.Today.AddYears(-1),
                MaxTakenDate = DateTime.Today,
                PrivacyFilter = PrivacyFilter.PublicPhotos,
                SafeSearch = SafetyLevel.Safe,
                ContentType = ContentTypeSearch.PhotosOnly,
                HasGeo = false,
                WoeId = "30079",
                PlaceId = "X9sTR3BSUrqorQ"
            };

            var photos = Instance.PhotosSearch(o);

            Assert.That(photos, Is.Not.Null);
            Assert.That(photos, Is.Empty);

        }

        [Test, Ignore("Not currently working for some reason.")]
        public void PhotosSearchGalleryPhotos()
        {
            var o = new PhotoSearchOptions { UserId = TestData.TestUserId, InGallery = true, Tags = "art" };
            var photos = Instance.PhotosSearch(o);

            Assert.That(photos, Has.Count.EqualTo(1), "Only one photo should have been returned.");
        }

        [Test]
        public void PhotosSearchUrlLimitTest()
        {
            var o = new PhotoSearchOptions { Extras = PhotoSearchExtras.All, TagMode = TagMode.AnyTag };
            var sb = new StringBuilder();
            for (var i = 1; i < 200; i++)
            {
                sb.Append("tagnumber" + i);
            }

            o.Tags = sb.ToString();

            var photos = Instance.PhotosSearch(o);

            Assert.That(photos, Is.Not.Null);
        }

        [Test]
        public void PhotosSearchRussianCharacters()
        {
            var o = new PhotoSearchOptions();
            o.Tags = "снег";

            var photos = Instance.PhotosSearch(o);

            Assert.That(photos, Is.Not.Empty, "Search should return some results.");
        }

        [Test]
        public void PhotosSearchRussianTagsReturned()
        {
            var o = new PhotoSearchOptions { PerPage = 200, Extras = PhotoSearchExtras.Tags, Tags = "фото" };

            var photos = Instance.PhotosSearch(o);

            photos.Count.ShouldNotBe(0);
            photos.ShouldContain(p => p.Tags.Any(t => t == "фото"));
        }

        [Test]
        public void PhotosSearchRussianTextReturned()
        {
            const string russian = "фото";

            var o = new PhotoSearchOptions { PerPage = 200, Extras = PhotoSearchExtras.Tags | PhotoSearchExtras.Description, Text = russian };

            var photos = Instance.PhotosSearch(o);


            photos.Count.ShouldNotBe(0);
            photos.ShouldContain(p => p.Tags.Any(t => t == russian) || (p.Title != null && p.Title.Contains(russian)) || (p.Description != null && p.Description.Contains(russian)));
        }

        [Test]
        [Category("AccessTokenRequired")]
        public void PhotosSearchAuthRussianCharacters()
        {
            var o = new PhotoSearchOptions();
            o.Tags = "снег";

            var photos = AuthInstance.PhotosSearch(o);

            Assert.That(photos, Is.Not.Empty, "Search should return some results.");
        }

        [Test]
        public void PhotosSearchRotation()
        {
            var o = new PhotoSearchOptions
            {
                Extras = PhotoSearchExtras.Rotation,
                UserId = TestData.TestUserId,
                PerPage = 100
            };
            var photos = Instance.PhotosSearch(o);
            foreach (var photo in photos)
            {
                Assert.That(photo.Rotation.HasValue, Is.True, "Rotation should be set.");
                if (photo.PhotoId == "6861439677")
                {
                    Assert.That(photo.Rotation, Is.EqualTo(90), "Rotation should be 90 for this photo.");
                }

                if (photo.PhotoId == "6790104907")
                {
                    Assert.That(photo.Rotation, Is.EqualTo(0), "Rotation should be 0 for this photo.");
                }
            }
        }

        [Test]
        public void PhotosSearchLarge1600ImageSize()
        {
            var o = new PhotoSearchOptions
            {
                Extras = PhotoSearchExtras.AllUrls,
                Tags = "colorful",
                MinUploadDate = DateTime.UtcNow.AddDays(-1)
            };

            var photos = Instance.PhotosSearch(o);

            Assert.That(photos, Is.Not.Null, "PhotosSearch should not return a null instance.");
            Assert.That(photos.Any(), Is.True, "PhotoSearch should have returned some photos.");
            Assert.That(
                photos.Any(
                    p =>
                    !string.IsNullOrEmpty(p.Large1600Url) && p.Large1600Height.HasValue && p.Large1600Width.HasValue), Is.True,
                "At least one photo should have a large1600 image url and height and width.");
        }

        [Test]
        public void PhotosSearchLarge2048ImageSize()
        {
            var o = new PhotoSearchOptions
            {
                Extras = PhotoSearchExtras.Large2048Url,
                Tags = "colorful",
                MinUploadDate = DateTime.UtcNow.AddDays(-1)
            };

            var photos = Instance.PhotosSearch(o);

            Assert.That(photos, Is.Not.Null, "PhotosSearch should not return a null instance.");
            Assert.That(photos.Any(), Is.True, "PhotoSearch should have returned some photos.");
            Assert.That(
                photos.Any(
                    p =>
                    !string.IsNullOrEmpty(p.Large2048Url) && p.Large2048Height.HasValue && p.Large2048Width.HasValue), Is.True);
        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void PhotosSearchContactsPhotos()
        {
            var contacts = AuthInstance.ContactsGetList(1, 1000).Select(c => c.UserId).ToList();

            // Test with user id = "me"
            var o = new PhotoSearchOptions
            {
                UserId = "me",
                Contacts = ContactSearch.AllContacts,
                PerPage = 50
            };

            var photos = AuthInstance.PhotosSearch(o);

            Assert.That(photos, Is.Not.Null, "PhotosSearch should not return a null instance.");
            Assert.That(photos.Any(), Is.True, "PhotoSearch should have returned some photos.");
            Assert.That(photos.All(p => p.UserId != TestData.TestUserId), Is.True, "None of the photos should be mine.");
            Assert.That(photos.All(p => contacts.Contains(p.UserId)), Is.True, "UserID not found in list of contacts.");

            // Retest with user id specified explicitly
            o.UserId = TestData.TestUserId;
            photos = AuthInstance.PhotosSearch(o);

            Assert.That(photos, Is.Not.Null, "PhotosSearch should not return a null instance.");
            Assert.That(photos.Any(), Is.True, "PhotoSearch should have returned some photos.");
            Assert.That(photos.All(p => p.UserId != TestData.TestUserId), Is.True, "None of the photos should be mine.");
            Assert.That(photos.All(p => contacts.Contains(p.UserId)), Is.True, "UserID not found in list of contacts.");
        }

        [Test]
        public void SearchByUsername()
        {
            var user = Instance.PeopleFindByUserName("Jesus Solana");

            var photos = Instance.PhotosSearch(new PhotoSearchOptions { Username = "Jesus Solana" });

            Assert.That(photos.First().UserId, Is.EqualTo(user.UserId));
        }

        [Test]
        public void SearchByExifExposure()
        {
            var options = new PhotoSearchOptions
            {
                ExifMinExposure = 10,
                ExifMaxExposure = 30,
                Extras = PhotoSearchExtras.PathAlias,
                PerPage = 5
            };

            var photos = Instance.PhotosSearch(options);

            foreach (var photo in photos)
            {
                Console.WriteLine(photo.WebUrl);
            }

            Assert.That(photos, Is.Not.Null);
        }

        [Test]
        public void SearchByExifAperture()
        {
            var options = new PhotoSearchOptions
            {
                ExifMinAperture = 0.0,
                ExifMaxAperture = 1 / 2,
                Extras = PhotoSearchExtras.PathAlias,
                PerPage = 5
            };

            var photos = Instance.PhotosSearch(options);

            foreach (var photo in photos)
            {
                Console.WriteLine(photo.WebUrl);
            }

            Assert.That(photos, Is.Not.Null);
        }

        [Test]
        public void SearchByExifFocalLength()
        {
            var options = new PhotoSearchOptions
            {
                ExifMinFocalLength = 400,
                ExifMaxFocalLength = 800,
                Extras = PhotoSearchExtras.PathAlias,
                PerPage = 5
            };

            var photos = Instance.PhotosSearch(options);

            foreach (var photo in photos)
            {
                Console.WriteLine(photo.WebUrl);
            }

            Assert.That(photos, Is.Not.Null);
        }

        [Test]
        public void ExcludeUserTest()
        {
            var options = new PhotoSearchOptions
            {
                Tags = "colorful",
                MinTakenDate = DateTime.Today.AddDays(-7),
                MaxTakenDate = DateTime.Today.AddDays(-1),
                PerPage = 10
            };

            var photos = Instance.PhotosSearch(options);


            var firstUserId = photos.First().UserId;
            Assert.That(firstUserId, Is.Not.Null);

            options.ExcludeUserID = firstUserId;

            var nextPhotos = Instance.PhotosSearch(options);

            Assert.That(nextPhotos.Any(p => p.UserId == firstUserId), Is.False, "Should not be any photos for user {0}", firstUserId);
        }

        [Test]
        public void GetPhotosByFoursquareVenueId()
        {
            var venueid = "4ac518cef964a520f6a520e3";

            var options = new PhotoSearchOptions
            {
                FoursquareVenueID = venueid
            };

            var photos = Instance.PhotosSearch(options);

            Assert.That(photos, Is.Not.Null);
            Assert.That(photos, Is.Not.Empty, "Should have returned some photos for 'Big Ben'");
        }

        [Test]
        [Ignore("WOE ID searches don't appear to be working.")]
        public void GetPhotosByFoursquareWoeId()
        {
            // Seems to be the same as normal WOE IDs, so not sure what is different about the foursquare ones.
            var woeId = "44417";

            var options = new PhotoSearchOptions
            {
                FoursquareWoeID = woeId
            };

            var photos = Instance.PhotosSearch(options);

            Assert.That(photos, Is.Not.Null);
            Assert.That(photos, Is.Not.Empty, "Should have returned some photos for 'Big Ben'");
        }

        [Test]
        public void CountFavesAndCountComments()
        {
            var options = new PhotoSearchOptions
            {
                Extras = PhotoSearchExtras.CountFaves | PhotoSearchExtras.CountComments,
                Tags = "colorful"
            };

            var photos = Instance.PhotosSearch(options);

            Assert.That(photos.Any(p => p.CountFaves == null), Is.False, "Should not have any null CountFaves");
            Assert.That(photos.Any(p => p.CountComments == null), Is.False, "Should not have any null CountComments");
        }

        [Test]
        public void ExcessiveTagsShouldNotThrowUriFormatException()
        {
            var list = Enumerable.Range(1, 9000).Select(i => "reallybigtag" + i).ToList();
            var options = new PhotoSearchOptions
            {
                Tags = string.Join(",", list)
            };

            Should.Throw<FlickrApiException>(() => Instance.PhotosSearch(options));
        }
    }
}

