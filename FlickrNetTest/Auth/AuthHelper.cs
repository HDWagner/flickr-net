using FlickrNet;
using NUnit.Framework;
using Shouldly;

namespace FlickrNetTest
{
    [TestFixture]
    public class AuthHelper
    {
        [Test]
        [Ignore("Method requires environment variable")]
        public void CheckEnvironmentVariableForAccessToken()
        {
            var value = Environment.GetEnvironmentVariable("FLICKR_TEST_ACCESSTOKEN");
            value.ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// This method will authenticate the current user, and then store the AuthToken in the 
        /// </summary>
        [Test]
        [Ignore("Use this to generate a new aut token if required")]
        public void AuthHelperMethod()
        {
            Flickr f = TestData.GetOldSignedInstance();

            string frob = f.AuthGetFrob();

            Assert.That(frob, Is.Not.Null, "Frob should not be null.");

            string url = f.AuthCalcUrl(frob, AuthLevel.Delete);

            Assert.That(url, Is.Not.Null, "url should not be null.");

            System.Diagnostics.Process.Start(url);

            // Auth flickr in next 30 seconds

            Thread.Sleep(1000 * 30);

            Auth auth = f.AuthGetToken(frob);

            Assert.That(auth.Token, Is.Not.Null);

            TestData.AuthToken = auth.Token;

            Console.WriteLine(TestData.AuthToken);
        }
    }
}
