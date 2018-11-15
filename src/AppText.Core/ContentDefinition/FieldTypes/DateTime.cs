using Newtonsoft.Json.Linq;

namespace AppText.Core.ContentDefinition.FieldTypes
{
    public class DateTime : FieldType
    {
        public override bool IsLocalizable => false;

        public override bool CanContainContent(object contentValue, bool contentMightBeLocalizable)
        {
            return contentValue is System.DateTime;
        }
    }
}
