using System.Text.RegularExpressions;

namespace AppText.Core.GraphQL
{
    public static class NameConverter
    {
        private const string GraphQLRegExPattern = @"[_A-Za-z][_0-9A-Za-z]*";

        /// <summary>
        /// Convert a name of a collection or content item field to a name that is compatible with GraphQL name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool TryConvertToGraphQLName(string name, out string convertedName)
        {
            // Replace spaces with underscores
            convertedName = name.Replace(' ', '_');

            // More?

            // Finally, validate with RegEx
            return Regex.IsMatch(convertedName, GraphQLRegExPattern);
        }
    }
}
