using System;

using NUnit.Framework;
using FlickrNet;
using Shouldly;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for PlacesForUserTests
    /// </summary>
    [TestFixture]
    [Ignore("Place functionality is not working very well at the moment!")]
    public class PlacesTests : BaseTest
    {
        [Test]
        public void PlacesFindBasicTest()
        {
            var places = Instance.PlacesFind("Newcastle");

            Assert.That(places, Is.Not.Null);
            Assert.That(places, Is.Not.Empty);
        }

        [Test]
        public void PlacesFindNewcastleTest()
        {
            var places = Instance.PlacesFind("Newcastle upon Tyne");

            Assert.That(places, Is.Not.Null);
            Assert.That(places, Has.Count.EqualTo(1));
        }

        [Test]
        public void PlacesFindByLatLongNewcastleTest()
        {
            double lat = 54.977;
            double lon = -1.612;

            var place = Instance.PlacesFindByLatLon(lat, lon);

            Assert.That(place, Is.Not.Null);
            Assert.That(place.Description, Is.EqualTo("Haymarket, Newcastle upon Tyne, England, GB, United Kingdom"));
        }

        [Test]
        public void PlacesPlacesForUserAuthenticationRequiredTest()
        {
            Flickr f = Instance;
            Should.Throw<SignatureRequiredException>(() => f.PlacesPlacesForUser());
        }

        [Test]
        [Category("AccessTokenRequired")]
        public void PlacesPlacesForUserHasContinentsTest()
        {
            Flickr f = AuthInstance;
            PlaceCollection places = f.PlacesPlacesForUser();

            foreach (Place place in places)
            {
                Assert.That(place.PlaceId, Is.Not.Null, "PlaceId should not be null.");
                Assert.That(place.WoeId, Is.Not.Null, "WoeId should not be null.");
                Assert.That(place.Description, Is.Not.Null, "Description should not be null.");
                Assert.That(place.PlaceType, Is.EqualTo(PlaceType.Continent), "PlaceType should be continent.");
            }

            Assert.That(places[0].PlaceId, Is.EqualTo("6dCBhRRTVrJiB5xOrg"));
            Assert.That(places[0].Description, Is.EqualTo("Europe"));
            Assert.That(places[1].PlaceId, Is.EqualTo("l5geY0lTVrLoNkLgeQ"));
            Assert.That(places[1].Description, Is.EqualTo("North America"));
        }

        [Test, Ignore("Not currently returning any records for some reason.")]
        public void PlacesGetChildrenWithPhotosPublicPlaceIdTest()
        {
            string placeId = "6dCBhRRTVrJiB5xOrg"; // Europe
            Flickr f = Instance;

            var places = f.PlacesGetChildrenWithPhotosPublic(placeId, null);
            Console.WriteLine(f.LastRequest);
            Console.WriteLine(f.LastResponse);

            Assert.That(places, Is.Not.Null);
            Assert.That(places, Is.Not.Empty);

            foreach (var place in places)
            {
                Assert.That(place.PlaceType, Is.EqualTo(PlaceType.Country));
            }
        }

        [Test, Ignore("Not currently returning any records for some reason.")]
        public void PlacesGetChildrenWithPhotosPublicWoeIdTest()
        {
            string woeId = "24865675"; // Europe

            var places = Instance.PlacesGetChildrenWithPhotosPublic(null, woeId);
            Assert.That(places, Is.Not.Null);
            Assert.That(places, Is.Not.Empty);

            foreach (var place in places)
            {
                Assert.That(place.PlaceType, Is.EqualTo(PlaceType.Country));
            }
        }

        [Test]
        [Category("AccessTokenRequired")]
        public void PlacesPlacesForUserContinentHasRegionsTest()
        {
            Flickr f = AuthInstance;

            // Test place ID of '6dCBhRRTVrJiB5xOrg' is Europe
            PlaceCollection p = f.PlacesPlacesForUser(PlaceType.Region, null, "6dCBhRRTVrJiB5xOrg");

            foreach (Place place in p)
            {
                Assert.That(place.PlaceId, Is.Not.Null, "PlaceId should not be null.");
                Assert.That(place.WoeId, Is.Not.Null, "WoeId should not be null.");
                Assert.That(place.Description, Is.Not.Null, "Description should not be null.");
                Assert.That(place.PlaceUrl, Is.Not.Null, "PlaceUrl should not be null");
                Assert.That(place.PlaceType, Is.EqualTo(PlaceType.Region), "PlaceType should be Region.");
            }
        }

        [Test]
        [Category("AccessTokenRequired")]
        public void PlacesPlacesForContactsBasicTest()
        {
            var f = AuthInstance;
            var places = f.PlacesPlacesForContacts(PlaceType.Country, null, null, 0, ContactSearch.AllContacts, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue);

            Assert.That(places, Is.Not.Null);

            Assert.That(places, Is.Not.Empty);

            foreach (var place in places)
            {
                Assert.That(place.PlaceType, Is.EqualTo(PlaceType.Country));
                Assert.That(place.PlaceId, Is.Not.Null);
            }
        }

        [Test]
        [Category("AccessTokenRequired")]
        public void PlacesPlacesForContactsFullParamTest()
        {
            DateTime lastYear = DateTime.Today.AddYears(-1);
            DateTime today = DateTime.Today;

            var f = AuthInstance;
            var places = f.PlacesPlacesForContacts(PlaceType.Country, null, null, 1, ContactSearch.AllContacts, lastYear, today, lastYear, today);

            Console.WriteLine(f.LastRequest);

            Assert.That(places, Is.Not.Null);

            Assert.That(places, Is.Not.Empty);

            foreach (var place in places)
            {
                Assert.That(place.PlaceType, Is.EqualTo(PlaceType.Country));
                Assert.That(place.PlaceId, Is.Not.Null);
            }
        }

        [Test]
        [Category("AccessTokenRequired")]
        public void PlacesPlacesForTagsBasicTest()
        {
            var f = AuthInstance;
            var places = f.PlacesPlacesForTags(PlaceType.Country, null, null, 0, new string[] {"newcastle"},
                                               TagMode.AllTags, null, MachineTagMode.None, DateTime.MinValue,
                                               DateTime.MinValue, DateTime.MinValue, DateTime.MinValue);

            Assert.That(places, Is.Not.Null);

            Assert.That(places, Is.Not.Empty);

            foreach (var place in places)
            {
                Assert.That(place.PlaceType, Is.EqualTo(PlaceType.Country));
                Assert.That(place.PlaceId, Is.Not.Null);
            }
        }

        [Test]
        [Category("AccessTokenRequired")]
        public void PlacesPlacesForTagsFullParamTest()
        {
            var f = AuthInstance;
            var places = f.PlacesPlacesForTags(PlaceType.Country, null, null, 0, new string[] {"newcastle"},
                                               TagMode.AllTags, new string[] {"dc:author=*"}, MachineTagMode.AllTags,
                                               DateTime.Today.AddYears(-10), DateTime.Today,
                                               DateTime.Today.AddYears(-10), DateTime.Today);

            Assert.That(places, Is.Not.Null);
        }

        [Test]
        public void PlacesGetInfoBasicTest()
        {
            var f = Instance;
            var placeId = "X9sTR3BSUrqorQ";
            PlaceInfo p = f.PlacesGetInfo(placeId, null);

            Console.WriteLine(f.LastResponse);

            Assert.That(p, Is.Not.Null);
            Assert.That(p.PlaceId, Is.EqualTo(placeId));
            Assert.That(p.WoeId, Is.EqualTo("30079"));
            Assert.That(p.PlaceType, Is.EqualTo(PlaceType.Locality));
            Assert.That(p.Description, Is.EqualTo("Newcastle upon Tyne, England, United Kingdom"));

            Assert.That(p.Locality?.PlaceId, Is.EqualTo("X9sTR3BSUrqorQ"));
            Assert.That(p.County?.PlaceId, Is.EqualTo("myqh27pQULzLWcg7Kg"));
            Assert.That(p.Region?.PlaceId, Is.EqualTo("2eIY2QFTVr_DwWZNLg"));
            Assert.That(p.Country?.PlaceId, Is.EqualTo("cnffEpdTUb5v258BBA"));

            Assert.That(p.HasShapeData, Is.True);
            Assert.That(p.ShapeData, Is.Not.Null);
            Assert.That(p.ShapeData.Alpha, Is.EqualTo(0.00015));
            Assert.That(p.ShapeData.PolyLines, Has.Count.EqualTo(1));
            Assert.That(p.ShapeData.PolyLines[0], Has.Count.EqualTo(89));
            Assert.That(p.ShapeData.PolyLines[0][88].X, Is.EqualTo(55.030498504639));
            Assert.That(p.ShapeData.PolyLines[0][88].Y, Is.EqualTo(-1.6404060125351));
        }

        [Test]
        public void PlacesGetInfoByUrlBasicTest()
        {
            var f = Instance;
            var placeId = "X9sTR3BSUrqorQ";
            PlaceInfo p1 = f.PlacesGetInfo(placeId, null);
            Assert.That(p1.PlaceUrl, Is.Not.Null);
            PlaceInfo p2 = f.PlacesGetInfoByUrl(p1.PlaceUrl);

            Assert.That(p2, Is.Not.Null);
            Assert.That(p2.PlaceId, Is.EqualTo(p1.PlaceId));
            Assert.That(p2.WoeId, Is.EqualTo(p1.WoeId));
            Assert.That(p2.PlaceType, Is.EqualTo(p1.PlaceType));
            Assert.That(p2.Description, Is.EqualTo(p1.Description));

            Assert.That(p2.PlaceFlickrUrl, Is.Not.Null);
        }

        [Test]
        public void PlacesGetTopPlacesListTest()
        {
            var f = Instance;
            var places = f.PlacesGetTopPlacesList(PlaceType.Continent);

            Assert.That(places, Is.Not.Null);
            Assert.That(places, Is.Not.Empty);

            foreach (var p in places)
            {
                Assert.That(p.PlaceType, Is.EqualTo(PlaceType.Continent));
                Assert.That(p.PlaceId, Is.Not.Null);
                Assert.That(p.WoeId, Is.Not.Null);
            }
        }

        [Test]
        public void PlacesGetShapeHistoryTest()
        {
            var placeId = "X9sTR3BSUrqorQ";
            var f = Instance;
            var col = f.PlacesGetShapeHistory(placeId, null);

            Assert.That(col, Is.Not.Null, "ShapeDataCollection should not be null.");
            Assert.That(col, Has.Count.EqualTo(7), "Count should be six.");
            Assert.That(col.PlaceId, Is.EqualTo(placeId));

            Assert.That(col[1].PolyLines, Has.Count.EqualTo(1), "The second shape should have one polylines.");
        }

        [Test]
        public void PlacesGetTagsForPlace()
        {
            var placeId = "X9sTR3BSUrqorQ";
            var f = Instance;
            var col = f.PlacesTagsForPlace(placeId, null);

            Assert.That(col, Is.Not.Null, "TagCollection should not be null.");
            Assert.That(col, Has.Count.EqualTo(100), "Count should be one hundred.");

            foreach (var t in col)
            {
                Assert.That(t.Count, Is.Not.EqualTo(0), "Count should be greater than zero.");
                Assert.That(t.TagName, Is.Not.Null, "TagName should not be null.");
            }

        }

        [Test]
        public void PlacesGetPlaceTypes()
        {
            var pts = Instance.PlacesGetPlaceTypes();
            Assert.That(pts, Is.Not.Null);
            Assert.That(pts, Has.Count.GreaterThan(1), "Count should be greater than one. Count = " + pts.Count + ".");

            foreach (var kp in pts)
            {
                Assert.That(kp.Id, Is.Not.EqualTo(0), "Key should not be zero.");
                Assert.That(kp.Name, Is.Not.Null, "Value should not be null.");

                Assert.That(Enum.IsDefined(typeof(PlaceType), kp.Id), Is.True, "PlaceType with ID " + kp.Id + " and Value '" + kp.Name + "' not defined in PlaceType enum.");
                var type = (PlaceType)kp.Id;
                Assert.That(kp.Name, Is.EqualTo(type.ToString("G").ToLower()), "Name of enum should match.");
            }
        }

        [Test]
        public void PlacesPlacesForBoundingBoxUsaTest()
        {
            Flickr f = Instance;

            var places = f.PlacesPlacesForBoundingBox(PlaceType.County, null, null, BoundaryBox.UKNewcastle);

            Assert.That(places, Is.Not.Null);
            Assert.That(places, Is.Not.Empty);

            foreach (var place in places)
            {
                Assert.That(place.PlaceId, Is.Not.Null);
                Assert.That(place.PlaceType, Is.EqualTo(PlaceType.County));
            }
        }

    }
}
