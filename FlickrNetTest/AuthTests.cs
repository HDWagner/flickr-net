using FlickrNet;
using FlickrNet.Exceptions;
using NUnit.Framework;
using Shouldly;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Xml;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for AuthTests
    /// </summary>
    [TestFixture]
    public class AuthTests : BaseTest
    {
        [Test]
        public void AuthGetFrobTest()
        {
            string frob = TestData.GetOldSignedInstance().AuthGetFrob();

            Assert.That(frob, Is.Not.Null, "frob should not be null.");
            Assert.That(frob, Is.Not.EqualTo(""), "Frob should not be zero length string.");
        }

        [Test]
        [Ignore("Calling this will invalidate the existing token so use wisely.")]
        public void AuthGetFrobAsyncTest()
        {
            var w = new AsyncSubject<FlickrResult<string>>();

            TestData.GetOldSignedInstance().AuthGetFrobAsync(r => { w.OnNext(r); w.OnCompleted(); });

            var frobResult = w.Next().First();

            Assert.That(frobResult.HasError, Is.False);

            string frob = frobResult.Result;

            Assert.That(frob, Is.Not.Null, "frob should not be null.");
            Assert.That(frob, Is.Not.EqualTo(""), "Frob should not be zero length string.");
        }

        [Test]
        public void AuthGetFrobSignRequiredTest()
        {
            Action getFrobAction = () => Instance.AuthGetFrob();
            getFrobAction.ShouldThrow<SignatureRequiredException>();
        }

        [Test]
        public void AuthCalcUrlTest()
        {
            string frob = "abcdefgh";

            string url = TestData.GetOldSignedInstance().AuthCalcUrl(frob, AuthLevel.Read);

            Assert.That(url, Is.Not.Null, "url should not be null.");
        }

        [Test]
        public void AuthCalcUrlSignRequiredTest()
        {
            string frob = "abcdefgh";

            Action calcUrlAction = () => Instance.AuthCalcUrl(frob, AuthLevel.Read);
            calcUrlAction.ShouldThrow<SignatureRequiredException>();
        }

        [Test]
        [Ignore("No longer needed. Delete in future version")]
        public void AuthCheckTokenBasicTest()
        {
            Flickr f = TestData.GetOldAuthInstance();

            string authToken = f.AuthToken;

            Assert.That(authToken, Is.Not.Null, "authToken should not be null.");

            Auth auth = f.AuthCheckToken(authToken);

            Assert.That(auth, Is.Not.Null, "Auth should not be null.");
            Assert.That(auth.Token, Is.EqualTo(authToken));
        }

        [Test]
        [Ignore("No longer needed. Delete in future version")]
        public void AuthCheckTokenCurrentTest()
        {
            Flickr f = TestData.GetOldAuthInstance();

            Auth auth = f.AuthCheckToken();

            Assert.That(auth, Is.Not.Null, "Auth should not be null.");
            Assert.That(auth.Token, Is.EqualTo(f.AuthToken));
        }

        [Test]
        public void AuthCheckTokenSignRequiredTest()
        {
            string token = "abcdefgh";

            Should.Throw<SignatureRequiredException>(() => Instance.AuthCheckToken(token));
        }

        [Test]
        public void AuthCheckTokenInvalidTokenTest()
        {
            string token = "abcdefgh";

            Should.Throw<LoginFailedInvalidTokenException>(() => TestData.GetOldSignedInstance().AuthCheckToken(token));
        }

        [Test]
        public void AuthClassBasicTest()
        {
            string authResponse = "<auth><token>TheToken</token><perms>delete</perms><user nsid=\"41888973@N00\" username=\"Sam Judson\" fullname=\"Sam Judson\" /></auth>";

            var reader = new XmlTextReader(new StringReader(authResponse));
            reader.Read();

            var auth = new Auth();
            var parsable = auth as IFlickrParsable;

            parsable.Load(reader);

            Assert.That(auth.Token, Is.EqualTo("TheToken"));
            Assert.That(auth.Permissions, Is.EqualTo(AuthLevel.Delete));
            Assert.That(auth.User.UserId, Is.EqualTo("41888973@N00"));
            Assert.That(auth.User.UserName, Is.EqualTo("Sam Judson"));
            Assert.That(auth.User.FullName, Is.EqualTo("Sam Judson"));

        }
    }
}

