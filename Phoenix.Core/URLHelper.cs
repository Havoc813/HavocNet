using System;
using System.Web;

namespace Phoenix.Core
{
    public static class URLHelper
    {
        public static string GetFirstPart(Uri aURL)
        {
            return aURL.Scheme + "://" +
                aURL.Host + ":" +
                aURL.Port + "/" +
                aURL.PathAndQuery.Substring(0, aURL.PathAndQuery.IndexOf('/', 2));
        }

        public static string GetPageName(Uri aURL)
        {
            return aURL.Segments[aURL.Segments.Length - 1];
        }

        public static string Decode(string URLParam)
        {
            return URLParam == null ? "" : HttpUtility.UrlDecode(URLParam);
        }

        public static string Encode(string URLParam)
        {
            return URLParam == null ? "" : HttpUtility.UrlEncode(URLParam);
        }
    }
}
