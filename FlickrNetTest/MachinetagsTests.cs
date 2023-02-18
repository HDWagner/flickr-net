using System;

using NUnit.Framework;
using FlickrNet;

namespace FlickrNetTest
{
    [TestFixture]
    public class MachinetagsTests : BaseTest
    {
        [Test]
        public void MachinetagsGetNamespacesBasicTest()
        {
            NamespaceCollection col = Instance.MachineTagsGetNamespaces();

            Assert.That(col, Has.Count.GreaterThan(10), "Should be greater than 10 namespaces.");

            foreach (var n in col)
            {
                Assert.That(n.NamespaceName, Is.Not.Null, "NamespaceName should not be null.");
                Assert.That(n.Predicates, Is.Not.EqualTo(0), "Predicates should not be zero.");
                Assert.That(n.Usage, Is.Not.EqualTo(0), "Usage should not be zero.");
            }
        }

        [Test]
        public void MachinetagsGetPredicatesBasicTest()
        {
            var col = Instance.MachineTagsGetPredicates();

            Assert.That(col, Has.Count.GreaterThan(10), "Should be greater than 10 namespaces.");

            foreach (var n in col)
            {
                Assert.That(n.PredicateName, Is.Not.Null, "PredicateName should not be null.");
                Assert.That(n.Namespaces, Is.Not.EqualTo(0), "Namespaces should not be zero.");
                Assert.That(n.Usage, Is.Not.EqualTo(0), "Usage should not be zero.");
            }
        }

        [Test]
        public void MachinetagsGetPairsBasicTest()
        {
            var pairs = Instance.MachineTagsGetPairs(null, null, 0, 0);
            Assert.That(pairs, Is.Not.Null);

            Assert.That(pairs, Is.Not.Empty, "Count should not be zero.");

            foreach (Pair p in pairs)
            {
                Assert.That(p.NamespaceName, Is.Not.Null, "NamespaceName should not be null.");
                Assert.That(p.PairName, Is.Not.Null, "PairName should not be null.");
                Assert.That(p.PredicateName, Is.Not.Null, "PredicateName should not be null.");
                Assert.That(p.Usage, Is.Not.EqualTo(0), "Usage should be greater than zero.");
            }
        }


        [Test]
        public void MachinetagsGetPairsNamespaceTest()
        {
            var pairs = Instance.MachineTagsGetPairs("dc", null, 0, 0);
            Assert.That(pairs, Is.Not.Null);

            Assert.That(pairs, Is.Not.Empty, "Count should not be zero.");

            foreach (Pair p in pairs)
            {
                Assert.That(p.NamespaceName, Is.EqualTo("dc"), "NamespaceName should be 'dc'.");
                Assert.That(p.PairName, Is.Not.Null, "PairName should not be null.");
                Assert.That(p.PairName.StartsWith("dc:", StringComparison.Ordinal), Is.True, "PairName should start with 'dc:'.");
                Assert.That(p.PredicateName, Is.Not.Null, "PredicateName should not be null.");
                Assert.That(p.Usage, Is.Not.EqualTo(0), "Usage should be greater than zero.");

            }
        }

        [Test]
        public void MachinetagsGetPairsPredicateTest()
        {
            var pairs = Instance.MachineTagsGetPairs(null, "author", 0, 0);
            Assert.That(pairs, Is.Not.Null);

            Assert.That(pairs, Is.Not.Empty, "Count should not be zero.");

            foreach (Pair p in pairs)
            {
                Assert.That(p.PredicateName, Is.EqualTo("author"), "PredicateName should be 'dc'.");
                Assert.That(p.PairName, Is.Not.Null, "PairName should not be null.");
                Assert.That(p.PairName.EndsWith(":author", StringComparison.Ordinal), Is.True, "PairName should end with ':author'.");
                Assert.That(p.NamespaceName, Is.Not.Null, "NamespaceName should not be null.");
                Assert.That(p.Usage, Is.Not.EqualTo(0), "Usage should be greater than zero.");

            }
        }

        [Test]
        public void MachinetagsGetPairsDcAuthorTest()
        {
            var pairs = Instance.MachineTagsGetPairs("dc", "author", 0, 0);
            Assert.That(pairs, Is.Not.Null);

            Assert.That(pairs, Has.Count.EqualTo(1), "Count should be 1.");

            foreach (Pair p in pairs)
            {
                Assert.That(p.PredicateName, Is.EqualTo("author"), "PredicateName should be 'author'.");
                Assert.That(p.NamespaceName, Is.EqualTo("dc"), "NamespaceName should be 'dc'.");
                Assert.That(p.PairName, Is.EqualTo("dc:author"), "PairName should be 'dc:author'.");
                Assert.That(p.Usage, Is.Not.EqualTo(0), "Usage should be greater than zero.");

            }
        }

        [Test]
        public void MachinetagsGetValuesTest()
        {
            var items = Instance.MachineTagsGetValues("dc", "author");
            Assert.That(items, Is.Not.Null);

            Assert.That(items, Is.Not.Empty, "Count should be not be zero.");
            Assert.That(items.NamespaceName, Is.EqualTo("dc"));
            Assert.That(items.PredicateName, Is.EqualTo("author"));

            foreach (var item in items)
            {
                Assert.That(item.PredicateName, Is.EqualTo("author"), "PredicateName should be 'author'.");
                Assert.That(item.NamespaceName, Is.EqualTo("dc"), "NamespaceName should be 'dc'.");
                Assert.That(item.ValueText, Is.Not.Null, "ValueText should not be null.");
                Assert.That(item.Usage, Is.Not.EqualTo(0), "Usage should be greater than zero.");
            }
        }

        [Test]
        [Ignore("This method is throwing a Not Available error at the moment.")]
        public void MachinetagsGetRecentValuesTest()
        {
            var items = Instance.MachineTagsGetRecentValues(DateTime.Now.AddHours(-5));
            Assert.That(items, Is.Not.Null);

            Assert.That(items, Is.Not.Empty, "Count should be not be zero.");

            foreach (var item in items)
            {
                Assert.That(item.NamespaceName, Is.Not.Null, "NamespaceName should not be null.");
                Assert.That(item.PredicateName, Is.Not.Null, "PredicateName should not be null.");
                Assert.That(item.ValueText, Is.Not.Null, "ValueText should not be null.");
                Assert.That(item.DateFirstAdded, Is.Not.Null, "DateFirstAdded should not be null.");
                Assert.That(item.DateLastUsed, Is.Not.Null, "DateLastUsed should not be null.");
                Assert.That(item.Usage, Is.Not.EqualTo(0), "Usage should be greater than zero.");
            }
        }
    }
}
