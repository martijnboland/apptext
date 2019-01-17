using GraphQL.Types;
using Newtonsoft.Json.Linq;

namespace AppText.Features.ContentDefinition.FieldTypes
{
    public class DateTime : FieldType
    {
        public override bool IsLocalizable => false;

        public override ScalarGraphType GraphQLType => new DateTimeGraphType();

        public override bool CanContainContent(object contentValue, bool contentMightBeLocalizable)
        {
            return contentValue is System.DateTime;
        }
    }
}
