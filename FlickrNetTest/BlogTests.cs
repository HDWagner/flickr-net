
using NUnit.Framework;
using FlickrNet;
using FlickrNet.Classes;
using FlickrNetTest.TestUtilities;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for BlogTests
    /// </summary>
    [TestFixture]
    public class BlogTests : BaseTest
    {
       
        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void BlogsGetListTest()
        {
            Flickr f = AuthInstance;

            BlogCollection blogs = f.BlogsGetList();

            Assert.That(blogs, Is.Not.Null, "Blogs should not be null.");

            foreach (Blog blog in blogs)
            {
                Assert.That(blog.BlogId, Is.Not.Null, "BlogId should not be null.");
                Assert.That(blog.NeedsPassword, Is.Not.Null, "NeedsPassword should not be null.");
                Assert.That(blog.BlogName, Is.Not.Null, "BlogName should not be null.");
                Assert.That(blog.BlogUrl, Is.Not.Null, "BlogUrl should not be null.");
                Assert.That(blog.Service, Is.Not.Null, "Service should not be null.");
            }
        }

        [Test]
        public void BlogGetServicesTest()
        {
            Flickr f = Instance;

            BlogServiceCollection services = f.BlogsGetServices();

            Assert.That(services, Is.Not.Null, "BlogServices should not be null.");
            Assert.That(services, Is.Not.Empty, "BlogServices.Count should not be zero.");

            foreach (BlogService service in services)
            {
                Assert.That(service.Id, Is.Not.Null, "BlogService.Id should not be null.");
                Assert.That(service.Name, Is.Not.Null, "BlogService.Name should not be null.");
            }

            Assert.That(services[0].Id, Is.EqualTo("beta.blogger.com"), "First ID should be beta.blogger.com.");
            Assert.That(services[0].Name, Is.EqualTo("Blogger"), "First Name should be beta.blogger.com.");

        }
    }
}
