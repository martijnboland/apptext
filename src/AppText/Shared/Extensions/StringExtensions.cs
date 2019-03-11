using System;

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
    }
}
