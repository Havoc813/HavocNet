using System;

namespace Core
{
    public static class URLHelper
    {
        public static string GetPageFromURL(Uri aURL)
        {
            var endPos = aURL.PathAndQuery.IndexOf("?", StringComparison.Ordinal) > 0 ? aURL.PathAndQuery.IndexOf("?", StringComparison.Ordinal) : aURL.PathAndQuery.Length;
            var startPos = 0;
            return aURL.PathAndQuery.Substring(startPos + 1, endPos - startPos - 1);
        }

        public static string GetQueryFromURL(Uri aURL)
        {
            return aURL.Query.Replace("?", "");
        }

        public static string GetPageIdentifier(Uri aURL)
        {
            return GetPageFromURL(aURL) + "_" + GetQueryFromURL(aURL);
        }

        public static string GetPageNameFromURL(Uri aURL)
        {
            var startPos = aURL.PathAndQuery.LastIndexOf("/", StringComparison.Ordinal);
            var endPos = aURL.PathAndQuery.LastIndexOf("?", StringComparison.Ordinal) > 0 ? aURL.PathAndQuery.IndexOf("?", StringComparison.Ordinal) : aURL.PathAndQuery.Length;

            return aURL.PathAndQuery.Substring(startPos + 1, endPos - startPos - 6);
        }
    }
}
