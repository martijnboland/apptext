using System.ComponentModel.DataAnnotations;

namespace AppText.Features.Application
{
    public class App
    {
        [Required(ErrorMessage = "AppText:Required")]
        [StringLength(20, ErrorMessage = "AppText:StringLength|{1}")]
        public string Id { get; set; }

        [Required(ErrorMessage = "AppText:Required")]
        [StringLength(100, ErrorMessage = "AppText:StringLength|{1}")]
        public string DisplayName { get; set; }

        public string[] Languages { get; set; }

        public string DefaultLanguage { get; set; }
    }
}