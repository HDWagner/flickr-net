using System;
using System.Collections.Generic;
using NUnit.Framework;
using FlickrNet;
using Shouldly;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for OAuthTests
    /// </summary>
    [TestFixture]
    public class OAuthTests : BaseTest
    {
        [Test]
        [Ignore("Use this to generate a require token. Then add verifier to second test")]
        public void OAuthGetRequestTokenBasicTest()
        {
            Flickr f = TestData.GetSignedInstance();

            string callback = "oob";

            OAuthRequestToken requestToken = f.OAuthGetRequestToken(callback);

            Assert.That(requestToken, Is.Not.Null);
            Assert.That(requestToken.Token, Is.Not.Null, "Token should not be null.");
            Assert.That(requestToken.TokenSecret, Is.Not.Null, "TokenSecret should not be null.");

            System.Diagnostics.Process.Start(Flickr.OAuthCalculateAuthorizationUrl(requestToken.Token, AuthLevel.Delete));

            Console.WriteLine("token = " + requestToken.Token);
            Console.WriteLine("token secret = " + requestToken.TokenSecret);

            TestData.RequestToken = requestToken.Token;
            TestData.RequestTokenSecret = requestToken.TokenSecret;
        }

        [Test]
        [Ignore("Use this to generate an access token. Substitute the verifier from above test prior to running")]
        public void OAuthGetAccessTokenBasicTest()
        {
            Flickr f = TestData.GetSignedInstance();

            var requestToken = new OAuthRequestToken();
            requestToken.Token = TestData.RequestToken;
            requestToken.TokenSecret = TestData.RequestTokenSecret;
            string verifier = "846-116-116";

            OAuthAccessToken accessToken = f.OAuthGetAccessToken(requestToken, verifier);

            Console.WriteLine("access token = " + accessToken.Token);
            Console.WriteLine("access token secret = " + accessToken.TokenSecret);

            TestData.AccessToken = accessToken.Token;
            TestData.AccessTokenSecret = accessToken.TokenSecret;
        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void OAuthPeopleGetPhotosBasicTest()
        {
            var photos = AuthInstance.PeopleGetPhotos("me");
            Assert.That(photos, Is.Not.Null);
        }

        [Test]
        public void OAuthInvalidAccessTokenTest()
        {
            Instance.ApiSecret = "asdasd";

            Should.Throw<OAuthException>(() => { Instance.OAuthGetRequestToken("oob"); });
        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void OAuthCheckTokenTest()
        {
            Flickr f = AuthInstance;

            Auth a = f.AuthOAuthCheckToken();

            Assert.That(f.OAuthAccessToken, Is.EqualTo(a.Token));
        }

        [Test]
        public void OAuthCheckEncoding()
        {
            // Test cases taken from OAuth spec
            // http://wiki.oauth.net/w/page/12238556/TestCases
            var test = new Dictionary<string, string>()
            {
                { "abcABC123", "abcABC123" },
                { "-._~", "-._~"},
                {"%", "%25"},
                { "+", "%2B"},
                { "&=*", "%26%3D%2A"},
                { "", ""},
                { "\u000A", "%0A"},
                { "\u0020", "%20"},
                { "\u007F", "%7F"},
                { "\u0080", "%C2%80"},
                { "\u3001", "%E3%80%81"},
                { "$()", "%24%28%29"}
            };

            foreach (var pair in test)
            {
                Assert.That(UtilityMethods.EscapeOAuthString(pair.Key), Is.EqualTo(pair.Value));
            }
        }
    }
}
