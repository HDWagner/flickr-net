﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using FlickrNet.Classes;

namespace FlickrNet
{
    public partial class Flickr
    {
        /// <summary>
        /// Rotates a photo on Flickr.
        /// </summary>
        /// <remarks>
        /// Does not rotate the original photo.
        /// </remarks>
        /// <param name="photoId">The ID of the photo.</param>
        /// <param name="degrees">The number of degrees to rotate by. Valid values are 90, 180 and 270.</param>
        public void PhotosTransformRotate(string photoId, int degrees)
        {
            if (photoId == null)
            {
                throw new ArgumentNullException(nameof(photoId));
            }

            if (degrees != 90 && degrees != 180 && degrees != 270)
            {
                throw new ArgumentException("Must be 90, 180 or 270", nameof(degrees));
            }

            var parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.photos.transform.rotate");
            parameters.Add("photo_id", photoId);
            parameters.Add("degrees", degrees.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));

            GetResponseNoCache<NoResponse>(parameters);
        }

        /// <summary>
        /// Checks the status of one or more asynchronous photo upload tickets.
        /// </summary>
        /// <param name="tickets">A list of ticket ids</param>
        /// <returns></returns>
        public TicketCollection PhotosUploadCheckTickets(string[] tickets)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.photos.upload.checkTickets");
            parameters.Add("tickets", string.Join(",", tickets));

            return GetResponseNoCache<TicketCollection>(parameters);
        }

    }
}
