using System;
using System.Linq;
using NUnit.Framework;
using FlickrNet;

namespace FlickrNetTest
{
    [TestFixture]
    public class PhotosSuggestionsTests : BaseTest
    {
        readonly string photoId = "6282363572";

        [SetUp]
        public void TestInitialize()
        {
            Flickr.CacheDisabled = true;
        }
        
        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Throws a 500 exception for some reason.")]
        public void GetListTest()
        {
            var f = AuthInstance;

            // Remove any pending suggestions
            var suggestions = f.PhotosSuggestionsGetList(photoId, SuggestionStatus.Pending);
            Assert.That(suggestions, Is.Not.Null, "SuggestionCollection should not be null.");

            foreach (var s in suggestions)
            {
                if (s.SuggestionId == null)
                {
                    Console.WriteLine(f.LastRequest);
                    Console.WriteLine(f.LastResponse);
                }
                Assert.That(s.SuggestionId, Is.Not.Null, "Suggestion ID should not be null.");
                f.PhotosSuggestionsRemoveSuggestion(s.SuggestionId);
            }

            // Add test suggestion
            AddSuggestion();

            // Get list of suggestions and check
            suggestions = f.PhotosSuggestionsGetList(photoId, SuggestionStatus.Pending);

            Assert.That(suggestions, Is.Not.Null, "SuggestionCollection should not be null.");
            Assert.That(suggestions, Is.Not.Empty, "Count should not be zero.");

            var suggestion = suggestions.First();

            Assert.That(suggestion.SuggestedByUserId, Is.EqualTo("41888973@N00"));
            Assert.That(suggestion.SuggestedByUserName, Is.EqualTo("Sam Judson"));
            Assert.That(suggestion.Note, Is.EqualTo("I really think this is a good suggestion."));
            Assert.That(suggestion.Latitude, Is.EqualTo(54.977), "Latitude should be the same.");

            f.PhotosSuggestionsRemoveSuggestion(suggestion.SuggestionId);

            // Add test suggestion
            AddSuggestion();
            suggestion = f.PhotosSuggestionsGetList(photoId, SuggestionStatus.Pending).First();
            f.PhotosSuggestionsApproveSuggestion(suggestion.SuggestionId);
            f.PhotosSuggestionsRemoveSuggestion(suggestion.SuggestionId);

        }

        private void AddSuggestion()
        {
            var f = AuthInstance;

            var lat = 54.977;
            var lon = -1.612;
            var accuracy = GeoAccuracy.Street;
            var woeId = "30079";
            var placeId = "X9sTR3BSUrqorQ";
            var note = "I really think this is a good suggestion.";

            f.PhotosSuggestionsSuggestLocation(photoId, lat, lon, accuracy, woeId, placeId, note);
        }
    }
}
