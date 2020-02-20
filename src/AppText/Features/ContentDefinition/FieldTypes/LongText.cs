using GraphQL.Types;

namespace AppText.Features.ContentDefinition.FieldTypes
{
    public class LongText : FieldType
    {
        public override bool IsLocalizable => true;

        public override ScalarGraphType GraphQLType => new StringGraphType();

        public override bool CanContainContent(object contentValue)
        {
            return (contentValue is string);
        }
    }
}
