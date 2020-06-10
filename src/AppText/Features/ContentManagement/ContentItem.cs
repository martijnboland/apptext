using AppText.Shared.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppText.Features.ContentManagement
{
    public class ContentItem : IVersionable
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Required")]
        public string AppId { get; set; }

        [Required(ErrorMessage = "Required")]
        public string ContentKey { get; set; }

        [Required(ErrorMessage = "Required")]
        public string CollectionId { get; set; }

        public Dictionary<string, object> Meta { get; set; }

        public Dictionary<string, object> Content { get; set; }

        public int Version { get; set; }
        
        public DateTime? CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? LastModifiedAt { get; set; }

        public string LastModifiedBy { get; set; }

        public ContentItem()
        {
            this.Meta = new Dictionary<string, object>();
            this.Content = new Dictionary<string, object>();
        }
    }
}
