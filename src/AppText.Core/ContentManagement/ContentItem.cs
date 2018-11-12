using AppText.Core.Shared.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppText.Core.ContentManagement
{
    public class ContentItem : IVersionable
    {
        public string Id { get; set; }
        [Required]
        public string ContentKey { get; set; }
        [Required]
        public string CollectionId { get; set; }
        public Dictionary<string, object> Meta { get; set; }
        public Dictionary<string, Dictionary<string, object>> Content { get; set; }
        public int Version { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public string LastModifiedBy { get; set; }
    }
}
