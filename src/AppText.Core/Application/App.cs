using System.ComponentModel.DataAnnotations;

namespace AppText.Core.Application
{
    public class App
    {
        public string Id { get; set; }
        [Required]
        [StringLength(20)]
        public string PublicId { get; set; }
        [Required]
        [StringLength(100)]
        public string DisplayName { get; set; }
        public string[] Languages { get; set; }
        public string DefaultLanguage { get; set; }
    }
}
