namespace AppText.Core.ContentDefinition
{
    public class ContentType
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public Field[] MetaFields { get; set; }

        public Field[] ContentFields { get; set; }
    }
}
