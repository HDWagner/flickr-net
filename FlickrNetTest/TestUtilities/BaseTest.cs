﻿using FlickrNet;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace FlickrNetTest.TestUtilities
{
    public abstract class BaseTest
    {
        Flickr? _instance;
        Flickr? _authInstance;
        Dictionary<string, string> _errorLog;

        static int _testCount;

        protected Flickr Instance
        {
            get
            {
                return _instance ??= TestData.GetInstance();
            }
        }

        protected Flickr AuthInstance
        {
            get
            {
                return _authInstance ??= TestData.GetAuthInstance();
            }
        }

        protected bool InstanceUsed
        {
            get { return _instance != null; }
        }

        protected bool AuthInstanceUsed
        {
            get { return _authInstance != null; }
        }

        [SetUp]
        public void InitialiseLoggingAndFlickr()
        {
            _instance = null;
            _authInstance = null;
            _errorLog = new Dictionary<string, string>();
            _testCount += 1;
        }

        protected void LogOnError(string key, string information)
        {
            _errorLog.Add(key, information);
        }

        [TearDown]
        public void ErrorLogging()
        {
            if (_testCount % 10 > 0)
            {
                Thread.Sleep(200);
            }

            if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Failed)
            {
                return;
            }

            if (InstanceUsed)
            {
                Console.WriteLine("LastRequest: " + _instance!.LastRequest);
                Console.WriteLine("LastResponse: " + _instance!.LastResponse);
            }
            if (AuthInstanceUsed)
            {
                Console.WriteLine("LastRequest (Auth): " + _authInstance!.LastRequest);
                Console.WriteLine("LastResponse (Auth): " + _authInstance!.LastResponse);
            }

            foreach (var line in _errorLog)
            {
                Console.WriteLine(line.Key + ": " + line.Value);
            }
        }

    }
}
