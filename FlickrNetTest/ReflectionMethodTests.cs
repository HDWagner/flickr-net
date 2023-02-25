using System;
using System.Collections.Generic;

using NUnit.Framework;
using FlickrNet;
using System.Reflection;
using FlickrNet.Classes;
using FlickrNetTest.TestUtilities;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for ReflectionMethodTests
    /// </summary>
    [TestFixture]
    public class ReflectionMethodTests : BaseTest
    {
        [Test]
        public void ReflectionMethodsBasic()
        {
            Flickr f = Instance;

            MethodCollection methodNames = f.ReflectionGetMethods();

            Assert.That(methodNames, Is.Not.Null, "Should not be null");
            Assert.That(methodNames, Is.Not.Empty, "Should return some method names.");
            Assert.That(methodNames[0], Is.Not.Null, "First item should not be null");

        }

        [Test]
        public void ReflectionMethodsCheckWeSupport()
        {
            Flickr f = Instance;

            MethodCollection methodNames = f.ReflectionGetMethods();

            Assert.That(methodNames, Is.Not.Null, "Should not be null");
            Assert.That(methodNames, Is.Not.Empty, "Should return some method names.");
            Assert.That(methodNames[0], Is.Not.Null, "First item should not be null");

            Type type = typeof(Flickr);
            MethodInfo[] methods = type.GetMethods();

            int failCount = 0;

            foreach (string methodName in methodNames)
            {
                bool found = false;
                string trueName = methodName.Replace("flickr.", "").Replace(".", "").ToLower();
                foreach (MethodInfo info in methods)
                {
                    if (trueName == info.Name.ToLower())
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    failCount++;
                    Console.WriteLine("Method '" + methodName + "' not found in FlickrNet.Flickr.");
                }
            }

            if (failCount > 0)
            {
                Assert.Inconclusive("FailCount should be zero. Currently " + failCount + " unsupported methods found.");
            }
        }

        [Test]
        public void ReflectionMethodsCheckWeSupportAsync()
        {
            Flickr f = Instance;

            MethodCollection methodNames = f.ReflectionGetMethods();

            Assert.That(methodNames, Is.Not.Null, "Should not be null");
            Assert.That(methodNames, Is.Not.Empty, "Should return some method names.");
            Assert.That(methodNames[0], Is.Not.Null, "First item should not be null");

            Type type = typeof(Flickr);
            MethodInfo[] methods = type.GetMethods();

            int failCount = 0;

            foreach (string methodName in methodNames)
            {
                bool found = false;
                string trueName = methodName.Replace("flickr.", "").Replace(".", "").ToLower() + "async";
                foreach (MethodInfo info in methods)
                {
                    if (trueName == info.Name.ToLower())
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    failCount++;
                    Console.WriteLine("Async Method '" + methodName + "' not found in FlickrNet.Flickr.");
                }
            }

            if (failCount > 0)
            {
                Assert.Inconclusive("FailCount should be zero. Currently " + failCount + " unsupported methods found.");
            }
        }

        [Test]
        public void ReflectionGetMethodInfoSearchArgCheck()
        {
            PropertyInfo[] properties = typeof(PhotoSearchOptions).GetProperties();

            Method flickrMethod = Instance.ReflectionGetMethodInfo("flickr.photos.search");

            // These arguments are covered, but are named slightly differently from Flickr.
            var exceptions = new Dictionary<string, string>();
            exceptions.Add("license", "licenses"); // Licenses
            exceptions.Add("sort", "sortorder"); // SortOrder
            exceptions.Add("bbox", "boundarybox"); // BoundaryBox
            exceptions.Add("lat", "latitude"); // Latitude
            exceptions.Add("lon", "longitude"); // Longitude
            exceptions.Add("media", "mediatype"); // MediaType
            exceptions.Add("exifminfocallen", "exifminfocallength"); // Focal Length
            exceptions.Add("exifmaxfocallen", "exifmaxfocallength"); // Focal Length

            int numMissing = 0;

            foreach (MethodArgument argument in flickrMethod.Arguments)
            {
                if (argument.Name == "api_key")
                {
                    continue;
                }

                Assert.That(argument.Name, Is.Not.Null);

                bool found = false;

                string arg = argument.Name.Replace("_", "").ToLower();

                if (exceptions.ContainsKey(arg))
                {
                    arg = exceptions[arg];
                }

                foreach (PropertyInfo info in properties)
                {
                    string propName = info.Name.ToLower();
                    if (arg == propName)
                    {
                        found = true;
                        break;
                    }
                }


                if (!found)
                {
                    numMissing++;
                    Console.WriteLine("Argument    : " + argument.Name + " not found.");
                    Console.WriteLine("Description : " + argument.Description);
                }
            }

            Assert.That(numMissing, Is.EqualTo(0), "Number of missing arguments should be zero.");
        }

        [Test]
        [Ignore("Test takes a long time")]
        public void ReflectionMethodsCheckWeSupportAndParametersMatch()
        {
            var exceptions = new List<string>();
            exceptions.Add("flickr.photos.getWithGeoData");
            exceptions.Add("flickr.photos.getWithoutGeoData");
            exceptions.Add("flickr.photos.search");
            exceptions.Add("flickr.photos.getNotInSet");
            exceptions.Add("flickr.photos.getUntagged");

            Flickr f = Instance;

            MethodCollection methodNames = f.ReflectionGetMethods();

            Assert.That(methodNames, Is.Not.Null, "Should not be null");
            Assert.That(methodNames, Is.Not.Empty, "Should return some method names.");
            Assert.That(methodNames[0], Is.Not.Null, "First item should not be null");

            Type type = typeof(Flickr);
            MethodInfo[] methods = type.GetMethods();

            int failCount = 0;

            foreach (string methodName in methodNames)
            {
                bool found = false;
                bool foundTrue = false;
                string trueName = methodName.Replace("flickr.", "").Replace(".", "").ToLower();
                foreach (MethodInfo info in methods)
                {
                    if (trueName == info.Name.ToLower())
                    {
                        found = true;
                        break;
                    }
                }
                // Check the number of arguments to see if we have a matching method.
                if (found && !exceptions.Contains(methodName))
                {
                    Method method = f.ReflectionGetMethodInfo(methodName);
                    foreach (MethodInfo info in methods)
                    {
                        if (method.Arguments.Count - 1 == info.GetParameters().Length)
                        {
                            foundTrue = true;
                            break;
                        }
                    }
                }
                if (!found)
                {
                    failCount++;
                    Console.WriteLine("Method '" + methodName + "' not found in FlickrNet.Flickr.");
                }
                if (found && !foundTrue)
                {
                    Console.WriteLine("Method '" + methodName + "' found but no matching method with all arguments.");
                }
            }

            Assert.That(failCount, Is.EqualTo(0), "FailCount should be zero. Currently " + failCount + " unsupported methods found.");
        }


        [Test]
        public void ReflectionGetMethodInfoTest()
        {
            Flickr f = Instance;
            Method method = f.ReflectionGetMethodInfo("flickr.reflection.getMethodInfo");

            Assert.That(method, Is.Not.Null, "Method should not be null");
            Assert.That(method.Name, Is.EqualTo("flickr.reflection.getMethodInfo"), "Method name not set correctly");

            Assert.That(method.RequiredPermissions, Is.EqualTo(MethodPermission.None));

            Assert.That(method.Arguments, Has.Count.EqualTo(2), "There should be two arguments");
            Assert.That(method.Arguments[0].Name, Is.EqualTo("api_key"), "First argument should be api_key.");
            Assert.That(method.Arguments[0].IsOptional, Is.False, "First argument should not be optional.");

            Assert.That(method.Errors, Has.Count.EqualTo(9), "There should be 8 errors.");
            Assert.That(method.Errors[0].Code, Is.EqualTo(1), "First error should have code of 1");
            Assert.That(method.Errors[0].Message, Is.EqualTo("Method not found"), "First error should have code of 1");
            Assert.That(method.Errors[0].Description, Is.EqualTo("The requested method was not found."), "First error should have code of 1");
        }

        [Test]
        public void ReflectionGetMethodInfoFavContextArguments()
        {
            var methodName = "flickr.favorites.getContext";
            var method = Instance.ReflectionGetMethodInfo(methodName);

            Assert.That(method.Arguments, Has.Count.EqualTo(3));
            Assert.That(method.Arguments[1].Description, Is.EqualTo("The id of the photo to fetch the context for."));
        }
    }
}
