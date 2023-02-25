using System;

namespace FlickrNet
{
    /// <summary>
    /// Summary description for SafeNativeMethods.
    /// </summary>

    internal static class SafeNativeMethods
    {
        internal static int GetErrorCode(System.IO.IOException ioe)
        {
            return ioe.HResult & 0xFFFF;
        }
    }
}
