using AppText.Shared.Model;
using System.ComponentModel.DataAnnotations;

namespace AppText.Features.ContentDefinition
{
    public class ContentType : IVersionable
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Required")]
        public string AppId { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }

        public string Description { get; set; }

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
