using AppText.Core.ContentDefinition.FieldTypes;
using Newtonsoft.Json;

namespace AppText.Core.ContentDefinition
{
    public class Field
    {
        public string Name { get; set; }

        [JsonConverter(typeof(FieldTypeConverter))]
        public FieldType FieldType { get; set; }

        public bool IsRequired { get; set; }
    }
}
