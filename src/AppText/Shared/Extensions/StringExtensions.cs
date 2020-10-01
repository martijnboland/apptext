using System;
using System.Text;

namespace AppText.Shared.Extensions
{
    public static class StringExtensions
    {
        public static string EnsureStartsWith(this String theString, string startsWith)
        {
            if (! theString.StartsWith(startsWith))
            {
                theString = startsWith + theString;
            }
            return theString;
        }

        public static string EnsureEndsWith(this String theString, string endsWith)
        {
            if (! theString.EndsWith(endsWith))
            {
                theString += endsWith;
            }
            return theString;
        }

        public static string EnsureDoesNotEndWith(this String theString, string endsWith)
        {
            while (theString.EndsWith(endsWith))
            {
                theString = theString.Substring(0, theString.Length - 1);
            }
            return theString;
        }

        public static string ToCamelCase(this string theString, bool invariantCulture = true)
        {
            if (String.IsNullOrEmpty(theString))
            {
                return theString;
            }

            var parts = theString.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            var sb = new StringBuilder();
            sb.Append((invariantCulture ? char.ToLowerInvariant(parts[0][0]) : char.ToLower(parts[0][0])) + parts[0].Substring(1));
            for (int i = 1; i < parts.Length; i++)
            {
                sb.Append(".");
                sb.Append((invariantCulture ? char.ToLowerInvariant(parts[i][0]) : char.ToLower(parts[i][0])) + parts[i].Substring(1));
            }

            return sb.ToString();
        }
    }
}
