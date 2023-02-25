
using NUnit.Framework;
using FlickrNet.Classes;
using FlickrNetTest.TestUtilities;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for CommonsTests
    /// </summary>
    [TestFixture]
    public class CommonsTests : BaseTest
    {
       
        [Test]
        public void CommonsGetInstitutions()
        {
            InstitutionCollection insts = Instance.CommonsGetInstitutions();

            Assert.That(insts, Is.Not.Null);
            Assert.That(insts, Has.Count.GreaterThan(5));

            foreach (var i in insts)
            {
                Assert.That(i, Is.Not.Null);
                Assert.That(i.InstitutionId, Is.Not.Null);
                Assert.That(i.InstitutionName, Is.Not.Null);
            }
        }
    }
}
