using AppText.Features.ContentDefinition;
using AppText.Shared.Model;
using System.ComponentModel.DataAnnotations;

namespace AppText.Features.ContentManagement
{
    public class ContentCollection : IVersionable
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Required")]
        public ContentType ContentType { get; set; }

        public string ListDisplayField { get; set; }

        public int Version { get; set; }
    }
}