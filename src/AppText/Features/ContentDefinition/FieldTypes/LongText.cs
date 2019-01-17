using GraphQL.Types;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace AppText.Features.ContentDefinition.FieldTypes
{
    public class LongText : FieldType
    {
        public override bool IsLocalizable => true;

        public override ScalarGraphType GraphQLType => new StringGraphType();

        public override bool CanContainContent(object contentValue, bool contentMightBeLocalizable)
        {
            // Content can be a single string value or a dictionary with the language as key
            // String length doesn't matter
            if (contentMightBeLocalizable && contentValue is JObject) // dictionary
            {
                var jObject = (JObject)contentValue;
                var values = jObject.Values();
                return values.All(v => v.Type == JTokenType.String);
            }
            else if (contentValue is string)
            {
                return true; // any value is possible
            }
            return false;
        }
    }
}
