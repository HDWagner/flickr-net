using FlickrNet;
using NUnit.Framework;
using Shouldly;
using System.Net;

namespace FlickrNetTest
{
    [TestFixture]
    public class PhotosTests : BaseTest
    {
        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void PhotosSetDatesTest()
        {
            var f = AuthInstance;
            var photoId = TestData.PhotoId;

            var info = f.PhotosGetInfo(photoId);

            f.PhotosSetDates(photoId, info.DatePosted, info.DateTaken, info.DateTakenGranularity);
        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void PhotosAddTagsTest()
        {
            Flickr f = AuthInstance;
            string testtag = "thisisatesttag";
            string photoId = "6282363572";

            // Add the tag
            f.PhotosAddTags(photoId, testtag);
            // Add second tag using different signature
            f.PhotosAddTags(photoId, new string[] { testtag + "2" });

            // Get list of tags
            var tags = f.TagsGetListPhoto(photoId);

            // Find the tag in the collection
            var tagsToRemove = tags.Where(t => t.TagText.StartsWith(testtag, StringComparison.Ordinal));

            foreach (var tag in tagsToRemove)
            {
                // Remove the tag
                f.PhotosRemoveTag(tag.TagId);
            }
        }

        [Test]
        public void PhotosGetAllContextsBasicTest()
        {
            var a = Instance.PhotosGetAllContexts("4114887196");

            Assert.That(a, Is.Not.Null);
            Assert.That(a.Groups, Is.Not.Null, "Groups should not be null.");
            Assert.That(a.Sets, Is.Not.Null, "Sets should not be null.");

            Assert.That(a.Groups, Has.Count.EqualTo(1), "Groups.Count should be one.");
            Assert.That(a.Sets, Has.Count.EqualTo(1), "Sets.Count should be one.");
        }

        [Test]
        public void PhotosGetPopular()
        {
            var photos = Instance.PhotosGetPopular(TestData.TestUserId);

            photos.ShouldNotBeEmpty();
        }

        [Test]
        public void PhotosGetExifTest()
        {
            Flickr f = Instance;

            ExifTagCollection tags = f.PhotosGetExif("4268023123");

            Console.WriteLine(f.LastResponse);

            Assert.That(tags, Is.Not.Null, "ExifTagCollection should not be null.");

            Assert.That(tags, Has.Count.GreaterThan(20), "More than twenty parts of EXIF data should be returned.");

            Assert.That(tags[0].TagSpace, Is.EqualTo("IFD0"), "First tags TagSpace is not set correctly.");
            Assert.That(tags[0].TagSpaceId, Is.EqualTo(0), "First tags TagSpaceId is not set correctly.");
            Assert.That(tags[0].Tag, Is.EqualTo("Compression"), "First tags Tag is not set correctly.");
            Assert.That(tags[0].Label, Is.EqualTo("Compression"), "First tags Label is not set correctly.");
            Assert.That(tags[0].Raw, Is.EqualTo("JPEG (old-style)"), "First tags RAW is not correct.");
            Assert.That(tags[0].Clean, Is.Null, "First tags Clean should be null.");
        }

        [Test]
        public void PhotosGetContextBasicTest()
        {
            var context = Instance.PhotosGetContext("3845365350");

            Assert.That(context, Is.Not.Null);

            Assert.That(context.PreviousPhoto.PhotoId, Is.EqualTo("3844573707"));
            Assert.That(context.NextPhoto.PhotoId, Is.EqualTo("3992605178"));
        }

        [Test]
        public void PhotosGetExifIPhoneTest()
        {
            bool bFound = false;
            Flickr f = Instance;

            ExifTagCollection tags = f.PhotosGetExif("5899928191");

            Assert.That(tags.Camera, Is.EqualTo("Apple iPhone 4"), "Camera property should be set correctly.");

            foreach (ExifTag tag in tags)
            {
                if (tag.Tag == "Model")
                {
                    Assert.That(tag.Raw, Is.EqualTo("iPhone 4"), "Model tag is not 'iPhone 4'");
                    bFound = true;
                    break;
                }
            }
            Assert.That(bFound, Is.True, "Model tag not found.");
        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void PhotosGetNotInSetAllParamsTest()
        {
            Flickr f = AuthInstance;

            var photos = f.PhotosGetNotInSet(1, 10, PhotoSearchExtras.All);

            Assert.That(photos, Is.Not.Null);
            Assert.That(photos, Has.Count.EqualTo(10));
        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void PhotosGetNotInSetNoParamsTest()
        {
            Flickr f = AuthInstance;

            var photos = f.PhotosGetNotInSet();
        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void PhotosGetNotInSetPagesTest()
        {
            Flickr f = AuthInstance;

            var photos = f.PhotosGetNotInSet(1, 11);

            Assert.That(photos, Is.Not.Null);
            Assert.That(photos, Has.Count.EqualTo(11));

            // Overloads
            f.PhotosGetNotInSet();
            f.PhotosGetNotInSet(1);
            f.PhotosGetNotInSet(new PartialSearchOptions() { Page = 1, PerPage = 10, PrivacyFilter = PrivacyFilter.CompletelyPrivate });
        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void PhotosGetPermsBasicTest()
        {
            var p = AuthInstance.PhotosGetPerms("4114887196");

            Assert.That(p, Is.Not.Null);
            Assert.That(p.PhotoId, Is.EqualTo("4114887196"));
            Assert.That(p.PermissionComment, Is.Not.EqualTo(PermissionComment.Nobody));
        }

        [Test]
        public void PhotosGetRecentBlankTest()
        {
            var photos = Instance.PhotosGetRecent();

            Assert.That(photos, Is.Not.Null);
        }

        [Test]
        public void PhotosGetRecentAllParamsTest()
        {
            var photos = Instance.PhotosGetRecent(1, 20, PhotoSearchExtras.All);

            Assert.That(photos, Is.Not.Null);
            Assert.That(photos.PerPage, Is.EqualTo(20));
            Assert.That(photos, Has.Count.EqualTo(20));
        }

        [Test]
        public void PhotosGetRecentPagesTest()
        {
            var photos = Instance.PhotosGetRecent(1, 20);

            Assert.That(photos, Is.Not.Null);
            Assert.That(photos.PerPage, Is.EqualTo(20));
            Assert.That(photos, Has.Count.EqualTo(20));
        }

        [Test]
        public void PhotosGetRecentExtrasTest()
        {
            var photos = Instance.PhotosGetRecent(PhotoSearchExtras.OwnerName);

            Assert.That(photos, Is.Not.Null);
            Assert.That(photos, Is.Not.Empty);

            var photo = photos.First();
            Assert.That(photo.OwnerName, Is.Not.Null);
        }

        [Test]
        public void PhotosGetSizes10Test()
        {
            var o = new PhotoSearchOptions { Tags = "microsoft", PerPage = 10 };

            var photos = Instance.PhotosSearch(o);

            foreach (var p in photos)
            {
                var sizes = Instance.PhotosGetSizes(p.PhotoId);
                foreach (var s in sizes)
                {

                }
            }

            Assert.That(photos, Is.Not.Null);
        }

        [Test]
        public void PhotosGetSizesBasicTest()
        {
            var sizes = Instance.PhotosGetSizes("4114887196");

            Assert.That(sizes, Is.Not.Null);
            Assert.That(sizes, Is.Not.Empty);

            foreach (Size s in sizes)
            {
                Assert.That(s.Label, Is.Not.Null, "Label should not be null.");
                Assert.That(s.Source, Is.Not.Null, "Source should not be null.");
                Assert.That(s.Url, Is.Not.Null, "Url should not be null.");
                Assert.That(s.Height, Is.Not.EqualTo(0), "Height should not be zero.");
                Assert.That(s.Width, Is.Not.EqualTo(0), "Width should not be zero.");
                Assert.That(s.MediaType, Is.Not.EqualTo(MediaType.None), "MediaType should be set.");
            }
        }

        [Test]
        public void PhotosGetSizesVideoTest()
        {
            //https://www.flickr.com/photos/tedsherarts/4399135415/
            var sizes = Instance.PhotosGetSizes("4399135415");

            sizes.ShouldContain(s => s.MediaType == MediaType.Videos, "At least one size should contain a Video media type.");
            sizes.ShouldContain(s => s.MediaType == MediaType.Photos, "At least one size should contain a Photo media type.");
        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void PhotosGetUntaggedAllParamsTest()
        {
            Flickr f = AuthInstance;

            var photos = f.PhotosGetUntagged(1, 10, PhotoSearchExtras.All);
        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void PhotosGetUntaggedNoParamsTest()
        {
            Flickr f = AuthInstance;

            var photos = f.PhotosGetUntagged();

            Assert.That(photos, Is.Not.Null);
            Assert.That(photos, Is.Not.Empty);

            var photo = photos.First();
        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void PhotosGetUntaggedExtrasTest()
        {
            Flickr f = AuthInstance;

            var photos = f.PhotosGetUntagged(PhotoSearchExtras.OwnerName);

            Assert.That(photos, Is.Not.Null);
            Assert.That(photos, Is.Not.Empty);

            var photo = photos.First();

            Assert.That(photo.OwnerName, Is.Not.Null);
        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void PhotosGetUntaggedPagesTest()
        {
            Flickr f = AuthInstance;

            var photos = f.PhotosGetUntagged(1, 10);

            Assert.That(photos, Is.Not.Null);
            Assert.That(photos, Has.Count.EqualTo(10));
        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void PhotosRecentlyUpdatedTests()
        {
            var sixMonthsAgo = DateTime.Today.AddMonths(-6);
            var f = AuthInstance;

            var photos = f.PhotosRecentlyUpdated(sixMonthsAgo, PhotoSearchExtras.All, 1, 20);

            Assert.That(photos, Is.Not.Null);
            Assert.That(photos.PerPage, Is.EqualTo(20));
            Assert.That(photos, Is.Not.Empty);

            // Overloads

            photos = f.PhotosRecentlyUpdated(sixMonthsAgo);
            photos = f.PhotosRecentlyUpdated(sixMonthsAgo, PhotoSearchExtras.DateTaken);
            photos = f.PhotosRecentlyUpdated(sixMonthsAgo, 1, 10);
        }

        [Test]
        public void PhotosSearchDoesLargeExist()
        {
            var o = new PhotoSearchOptions();
            o.Extras = PhotoSearchExtras.AllUrls;
            o.PerPage = 50;
            o.Tags = "test";

            PhotoCollection photos = Instance.PhotosSearch(o);

            foreach (Photo p in photos)
            {
                Assert.That(p.DoesLargeExist || !p.DoesLargeExist, Is.True);
                Assert.That(p.DoesMediumExist || !p.DoesMediumExist, Is.True);
            }
        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void PhotosSetMetaLargeDescription()
        {
            string description;

            using (WebClient wc = new WebClient())
            {
                description = wc.DownloadString("http://en.wikipedia.org/wiki/Scots_Pine");
                // Limit to size of a url to 65519 characters, so chop the description down to a large but not too large size.
                description = description.Substring(0, 6551);
            }

            string title = "Blacksway Cat";
            string photoId = "5279984467";

            Flickr f = AuthInstance;
            f.PhotosSetMeta(photoId, title, description);
        }

        [Test]
        public void PhotosUploadCheckTicketsTest()
        {
            Flickr f = Instance;

            string[] tickets = new string[3];
            tickets[0] = "invalid1";
            tickets[1] = "invalid2";
            tickets[2] = "invalid3";

            var t = f.PhotosUploadCheckTickets(tickets);

            Assert.That(t, Has.Count.EqualTo(3));

            Assert.That(t[0].TicketId, Is.EqualTo("invalid1"));
            Assert.That(t[0].PhotoId, Is.Null);
            Assert.That(t[0].InvalidTicketId, Is.True);
        }

        [Test]
        public void PhotosPeopleGetListTest()
        {
            var photoId = "3547137580";

            var people = Instance.PhotosPeopleGetList(photoId);

            Assert.That(people.Total, Is.Not.EqualTo(0), "Total should not be zero.");
            Assert.That(people, Is.Not.Empty, "Count should not be zero.");
            Assert.That(people.Total, Is.EqualTo(people.Count), "Count should equal Total.");

            foreach (var person in people)
            {
                Assert.That(person.UserId, Is.Not.Null, "UserId should not be null.");
                Assert.That(person.PhotostreamUrl, Is.Not.Null, "PhotostreamUrl should not be null.");
                Assert.That(person.BuddyIconUrl, Is.Not.Null, "BuddyIconUrl should not be null.");
            }
        }

        [Test]
        public void PhotosPeopleGetListSpecificUserTest()
        {
            string photoId = "104267998"; // https://www.flickr.com/photos/thunderchild5/104267998/
            string userId = "41888973@N00"; //sam judsons nsid

            Flickr f = Instance;
            PhotoPersonCollection ppl = f.PhotosPeopleGetList(photoId);
            PhotoPerson pp = ppl[0];
            Assert.That(pp.UserId, Is.EqualTo(userId));
            Assert.That(pp.BuddyIconUrl, Does.Contain(".staticflickr.com/"), "Buddy icon doesn't contain correct details.");
        }

        [Test]
        public void WebUrlContainsUserIdIfPathAliasIsEmpty()
        {
            var options = new PhotoSearchOptions
            {
                UserId = "39858630@N06",
                PerPage = 1,
                Extras = PhotoSearchExtras.PathAlias
            };

            var flickr = Instance;
            var photos = flickr.PhotosSearch(options);

            string webUrl = photos[0].WebUrl;
            string userPart = webUrl.Split('/')[4];

            Console.WriteLine("WebUrl is: " + webUrl);
            Assert.That(userPart, Is.Not.EqualTo(string.Empty), "User part of the URL cannot be empty");
        }

        [Test]
        public void PhotostreamUrlContainsUserIdIfPathAliasIsEmpty()
        {
            var photoPerson = new PhotoPerson()
            {
                PathAlias = string.Empty,
                UserId = "UserId",
            };

            string userPart = photoPerson.PhotostreamUrl.Split('/')[4];

            Assert.That(userPart, Is.Not.EqualTo(string.Empty), "User part of the URL cannot be empty");
        }

        [Test]
        public void PhotosTestLargeSquareSmall320()
        {
            var o = new PhotoSearchOptions();
            o.Extras = PhotoSearchExtras.LargeSquareUrl | PhotoSearchExtras.Small320Url;
            o.UserId = TestData.TestUserId;
            o.PerPage = 10;

            var photos = Instance.PhotosSearch(o);
            Assert.That(photos, Is.Not.Empty, "Should return more than zero photos.");

            foreach (var photo in photos)
            {
                Assert.That(photo.Small320Url, Is.Not.Null, "Small320Url should not be null.");
                Assert.That(photo.LargeSquareThumbnailUrl, Is.Not.Null, "LargeSquareThumbnailUrl should not be null.");
            }
        }

    }
}
