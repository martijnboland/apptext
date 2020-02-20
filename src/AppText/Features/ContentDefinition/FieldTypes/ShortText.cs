using GraphQL.Types;

namespace AppText.Features.ContentDefinition.FieldTypes
{
    public class ShortText : FieldType
    {
        public const int MaxLength = 500;

        public override bool IsLocalizable => true;

        public override ScalarGraphType GraphQLType => new StringGraphType();

        public override bool CanContainContent(object contentValue)
        {
            if (contentValue is string)
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
