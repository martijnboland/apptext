using AppText.Core.ContentDefinition;
using System.ComponentModel.DataAnnotations;

namespace AppText.Core.ContentManagement
{
    public class ContentCollection
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public ContentType ContentType { get; set; }
        public int Version { get; set; }
    }
}