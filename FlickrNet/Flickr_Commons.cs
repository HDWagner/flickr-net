﻿using System;
using System.Collections.Generic;
using System.Text;
using FlickrNet.Classes;

namespace FlickrNet
{
    public partial class Flickr
    {
        /// <summary>
        /// Gets a collection of Flickr Commons institutions.
        /// </summary>
        /// <returns></returns>
        public InstitutionCollection CommonsGetInstitutions()
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.commons.getInstitutions");

            return GetResponseCache<InstitutionCollection>(parameters);
        }
    }
}
