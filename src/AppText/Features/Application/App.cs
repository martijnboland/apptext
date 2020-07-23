using System.ComponentModel.DataAnnotations;

namespace AppText.Features.Application
{
    public class App
    {
        [Required(ErrorMessage = "Required")]
        [StringLength(20, ErrorMessage = "StringLength|{1}")]
        [RegularExpression(@"^[_a-z][_0-9a-z]*", ErrorMessage = "AppIdInvalid")]
        public string Id { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(100, ErrorMessage = "StringLength|{1}")]
        public string DisplayName { get; set; }

        public string[] Languages { get; set; }

        public string DefaultLanguage { get; set; }

        public bool IsSystemApp { get; set; }
    }
}