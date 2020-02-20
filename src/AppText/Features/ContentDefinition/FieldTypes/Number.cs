using GraphQL.Types;
using System;
using System.Globalization;

namespace AppText.Features.ContentDefinition.FieldTypes
{
    public class Number : FieldType
    {
        public override bool IsLocalizable => false;

        public override ScalarGraphType GraphQLType => new DecimalGraphType();

        public override bool CanContainContent(object contentValue)
        {
            if (contentValue == null)
            {
                return true;
            }
            return decimal.TryParse(Convert.ToString(contentValue
                          , CultureInfo.InvariantCulture)
                          , NumberStyles.Any
                          , NumberFormatInfo.InvariantInfo
                          , out decimal number);
        }
    }
}
