using Newtonsoft.Json;
using System;

namespace AppText.Features.ContentDefinition.FieldTypes
{
    public class FieldTypeConverter : JsonConverter<FieldType>
    {
        private readonly string _defaultNamespace = typeof(FieldType).Namespace;

        public override FieldType ReadJson(JsonReader reader, Type objectType, FieldType existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var typeName = (string)reader.Value;
            if (! typeName.Contains("."))
            {
                typeName = $"{_defaultNamespace}.{typeName}";
            }
            var fieldTypeType = Type.GetType(typeName);
            return (FieldType)Activator.CreateInstance(fieldTypeType);
        }

        public override void WriteJson(JsonWriter writer, FieldType value, JsonSerializer serializer)
        {
            if (value.GetType().Namespace == _defaultNamespace)
            {
                writer.WriteValue(value.GetType().Name);
            }
            else
            {
                writer.WriteValue(value.GetType().AssemblyQualifiedName);
            }
        }
    }
}
