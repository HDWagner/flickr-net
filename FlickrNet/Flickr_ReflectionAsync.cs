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
        /// Gets an array of supported method names for Flickr.
        /// </summary>
        /// <remarks>
        /// Note: Not all methods might be supported by the FlickrNet Library.</remarks>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public void ReflectionGetMethodsAsync(Action<FlickrResult<MethodCollection>> callback)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.reflection.getMethods");

            GetResponseAsync(parameters, callback);
        }

        /// <summary>
        /// Gets the method details for a given method.
        /// </summary>
        /// <param name="methodName">The name of the method to retrieve.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public void ReflectionGetMethodInfoAsync(string methodName, Action<FlickrResult<Method>> callback)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.reflection.getMethodInfo");
            parameters.Add("api_key", ApiKey);
            parameters.Add("method_name", methodName);

            GetResponseAsync(parameters, callback);
        }

    }
}
