using System.ComponentModel.DataAnnotations;
using AppText.Core.Application;
using AppText.Core.Shared.Model;

namespace AppText.Core.ContentDefinition
{
    public class ContentType : IVersionable
    {
        public string Id { get; set; }

        public AppReference App { get; set; }

        public string Name { get; set; }

        public Field[] MetaFields { get; set; }

        public Field[] ContentFields { get; set; }

        public int Version { get; set; }

        public ContentType()
        {
            MetaFields = new Field[0];
            ContentFields = new Field[0];
        }
    }
}
