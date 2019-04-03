using AppText.Features.ContentDefinition.FieldTypes;
using Newtonsoft.Json;

namespace AppText.Features.ContentDefinition
{
    public class Field
    {
        public string Name { get; set; }

        public string Description { get; set; }

        [JsonConverter(typeof(FieldTypeConverter))]
        public FieldType FieldType { get; set; }

        public bool IsRequired { get; set; }
    }
}
