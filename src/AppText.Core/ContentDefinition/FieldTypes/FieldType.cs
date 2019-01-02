using GraphQL.Types;
using Newtonsoft.Json;

namespace AppText.Core.ContentDefinition.FieldTypes
{
    [JsonConverter(typeof(FieldTypeConverter))]
    public abstract class FieldType
    {
        public abstract bool IsLocalizable { get; }

        public abstract bool CanContainContent(object contentValue, bool contentMightBeLocalizable = false);

        public override string ToString()
        {
            return $"FieldType = {this.GetType().Name}, IsLocalizable = {IsLocalizable}";
        }

        public abstract ScalarGraphType GraphQLType { get; }
    }
}
