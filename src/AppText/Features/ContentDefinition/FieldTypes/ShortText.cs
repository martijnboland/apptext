using GraphQL.Types;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace AppText.Features.ContentDefinition.FieldTypes
{
    public class ShortText : FieldType
    {
        public const int MaxLength = 500;

        public override bool IsLocalizable => true;

        public override ScalarGraphType GraphQLType => new StringGraphType();

        public override bool CanContainContent(object contentValue, bool contentMightBeLocalizable)
        {
            // Content can be a single string value or a dictionary with the language as key
            if (contentMightBeLocalizable && contentValue is JObject) // dictionary
            {
                var jObject = (JObject)contentValue;
                var values = jObject.Values();
                return values.All(v => v.Type == JTokenType.String && CheckLength((string)v));
            }
            else if (contentValue is string)
            {
                var stringValue = (string)contentValue;
                return CheckLength(stringValue);
            }
            return false;
        }

        private bool CheckLength(string theString)
        {
            return theString.Length <= MaxLength;
        }
    }
}
