﻿using System;
using NUnit.Framework;
using FlickrNet;
using FlickrNet.Classes;
using FlickrNetTest.TestUtilities;

namespace FlickrNetTest
{
    [TestFixture]
    public class PhotosLicensesTests : BaseTest
    {
        [Test]
        public void PhotosLicensesGetInfoBasicTest()
        {
            LicenseCollection col = Instance.PhotosLicensesGetInfo();

            foreach (License lic in col)
            {
                if (!Enum.IsDefined(typeof(LicenseType), lic.LicenseId))
                {
                    Assert.Fail("License with ID " + (int)lic.LicenseId + ", " + lic.LicenseName + " dooes not exist.");
                }
            }
        }

        [Test]
        [Category("AccessTokenRequired")]
        [Ignore("Method requires authentication")]
        public void PhotosLicensesSetLicenseTest()
        {
            Flickr f = AuthInstance;
            string photoId = "7176125763";

            var photoInfo = f.PhotosGetInfo(photoId); // Rainbow Rose
            var origLicense = photoInfo.License;

            var newLicense = origLicense == LicenseType.AttributionCC ? LicenseType.AttributionNoDerivativesCC : LicenseType.AttributionCC;
            f.PhotosLicensesSetLicense(photoId, newLicense);

            var newPhotoInfo = f.PhotosGetInfo(photoId);

            Assert.That(newPhotoInfo.License, Is.EqualTo(newLicense), "License has not changed");

            // Reset license 
            f.PhotosLicensesSetLicense(photoId, origLicense);
        }

    }
}
