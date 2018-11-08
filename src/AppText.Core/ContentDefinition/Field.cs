namespace AppText.Core.ContentDefinition
{
    public class Field
    {
        public string Name { get; set; }
        public FieldType FieldType { get; set; }
        public bool IsRequired { get; set; }
    }

    public enum FieldType
    {
        Text,
        Number,
        Date
    }
}
