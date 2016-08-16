using System;
using System.Globalization;

namespace Phoenix.Core
{
    public static class FSStringHelper
    {
        public static string GetItemFromString(string text, string item, string terminator)
        {
            if (String.IsNullOrEmpty(text)) return "";
            if (String.IsNullOrEmpty(item)) return "";

            var itemIndex = text.IndexOf(item, StringComparison.Ordinal);
            if (itemIndex < 0) return "";
            text = text.Substring(itemIndex + item.Length);

            if (string.IsNullOrEmpty(terminator)) return text;

            var itemClose = text.IndexOf(terminator, StringComparison.Ordinal);
            if (itemClose < 0) return text;

            return text.Substring(0, itemClose);
        }

        public static string Right(string text, int numberCharacters)
        {
            if (String.IsNullOrEmpty(text)) return "";
            return text.Length > numberCharacters ? text.Substring(text.Length - numberCharacters, numberCharacters) : text;
        }

        public static string IsNull(string text)
        {
            return IsNull(text, "");
        }

        public static string IsNull(string text, string nullValue)
        {
            return String.IsNullOrEmpty(text) ? nullValue : text;
        }

        public static string ToHTML(string text)
        {
            return text.Replace(new string((char)13, 1), "<br />")
                       .Replace("\r\n", "<br />");
        }

        public static string ToColourHTML(string text, string colour)
        {
            return "<SPAN style=\"COLOR: " + colour + "\">" + text + "</SPAN>";
        }

        public static string StripLineBreak(string source)
        {
            return source.Replace(Char.ToString((char)13), String.Empty)
                         .Replace(Char.ToString((char)10), String.Empty)
                         .Replace(Char.ToString((char)9), String.Empty);
        }

        public static string Left(string text, int numberCharacters)
        {
            if (String.IsNullOrEmpty(text)) return "";
            return text.Length > numberCharacters ? text.Substring(0, numberCharacters) : text;
        }

        public static string Capitalise(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;

            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text);
        }
    }
}
