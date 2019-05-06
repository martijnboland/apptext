using AppText.Features.ContentDefinition.FieldTypes;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace AppText.Features.ContentDefinition
{
    public class Field
    {
        [Required(ErrorMessage = "AppText:Required")]
        public string Name { get; set; }

        public string Description { get; set; }

        [JsonConverter(typeof(FieldTypeConverter))]
        [Required(ErrorMessage = "AppText:Required")]
        public FieldType FieldType { get; set; }

        public bool IsRequired { get; set; }
    }
}
