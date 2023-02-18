
using NUnit.Framework;
using FlickrNet;
using System.Collections.Generic;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for PhotoSearchOptionsTests
    /// </summary>
    [TestFixture]
    public class PhotoSearchOptionsTests : BaseTest
    {
        [Test]
        public void PhotoSearchOptionsCalculateSlideshowUrlBasicTest()
        {
            var o = new PhotoSearchOptions {Text = "kittens", InGallery = true};

            var url = o.CalculateSlideshowUrl();

            Assert.That(url, Is.Not.Null);

            const string expected = "https://www.flickr.com/show.gne?api_method=flickr.photos.search&method_params=text|kittens;in_gallery|1";

            Assert.That(url, Is.EqualTo(expected));

        }

        [Test]
        public void PhotoSearchExtrasViews()
        {
            var o = new PhotoSearchOptions {Tags = "kittens", Extras = PhotoSearchExtras.Views};

            var photos = Instance.PhotosSearch(o);

            foreach (var photo in photos)
            {
                Assert.That(photo.Views.HasValue, Is.True);
            }
        }

        [Test]
        public void StylesNotAddedToParameters_WhenItIsNotSet()
        {
            var o = new PhotoSearchOptions();
            var parameters = new Dictionary<string, string>();

            o.AddToDictionary(parameters);

            Assert.That(parameters.ContainsKey("styles"), Is.False);
        }

        [Test]
        public void StylesNotAddedToParameters_WhenItIsEmpty()
        {
            var o = new PhotoSearchOptions { Styles = new List<Style>() };
            var parameters = new Dictionary<string, string>();

            o.AddToDictionary(parameters);

            Assert.That(parameters.ContainsKey("styles"), Is.False);
        }

        [TestCase(Style.BlackAndWhite)]
        [TestCase(Style.BlackAndWhite, Style.DepthOfField)]
        [TestCase(Style.BlackAndWhite, Style.DepthOfField, Style.Minimalism)]
        [TestCase(Style.BlackAndWhite, Style.DepthOfField, Style.Minimalism, Style.Pattern)]
        public void StylesAddedToParameters_WhenItIsNotNullOrEmpty(params Style[] styles)
        {
            var o = new PhotoSearchOptions { Styles = new List<Style>(styles) };
            var parameters = new Dictionary<string, string>();

            o.AddToDictionary(parameters);

            Assert.That(parameters.ContainsKey("styles"), Is.True);
        }
    }
}
