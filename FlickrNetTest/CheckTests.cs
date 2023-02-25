using FlickrNet;
using NUnit.Framework;
using Shouldly;

namespace FlickrNetTest
{
    [TestFixture]
    public class CheckTests : BaseTest
    {
        [Test]
        public void CheckApiKeyThrowsExceptionWhenNotPresent()
        {
            var f = new Flickr();
#pragma warning disable CS8625 // Ein NULL-Literal kann nicht in einen Non-Nullable-Verweistyp konvertiert werden.
            f.ApiKey = null;
#pragma warning restore CS8625 // Ein NULL-Literal kann nicht in einen Non-Nullable-Verweistyp konvertiert werden.

            Should.Throw<ApiKeyRequiredException>(() => f.CheckApiKey());
        }

        [Test]
        public void CheckApiKeyDoesNotThrowWhenPresent()
        {
            var f = new Flickr();
            f.ApiKey = "X";

            Should.NotThrow(() => f.CheckApiKey());
        }

        [Test]
        public void CheckSignatureKeyThrowsExceptionWhenSecretNotPresent()
        {
            var f = new Flickr();
            f.ApiKey = "X";
            Should.Throw<SignatureRequiredException>(() => f.CheckSigned());
        }

        [Test]
        public void CheckSignatureKeyDoesntThrowWhenSecretPresent()
        {
            var f = new Flickr();
            f.ApiKey = "X";
            f.ApiSecret = "Y";

            Should.NotThrow(() => f.CheckSigned());
        }

        [Test]
        public void CheckRequestAuthenticationThrowsExceptionWhenNothingPresent()
        {
            var f = new Flickr();
            f.ApiKey = "X";
            f.ApiSecret = "Y";

            Should.Throw<AuthenticationRequiredException>(() => f.CheckRequiresAuthentication());
        }

        [Test]
        public void CheckRequestAuthenticationDoesNotThrowWhenAuthTokenPresent()
        {
            var f = new Flickr();
            f.ApiKey = "X";
            f.ApiSecret = "Y";

            f.AuthToken = "Z";

            Assert.DoesNotThrow(f.CheckRequiresAuthentication);
        }

        [Test]
        public void CheckRequestAuthenticationDoesNotThrowWhenOAuthTokenPresent()
        {
            var f = new Flickr();
            f.ApiKey = "X";
            f.ApiSecret = "Y";

            f.OAuthAccessToken = "Z1";
            f.OAuthAccessTokenSecret = "Z2";

            Assert.DoesNotThrow(f.CheckRequiresAuthentication);
        }
    }
}
