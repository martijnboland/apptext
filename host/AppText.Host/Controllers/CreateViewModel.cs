using System.ComponentModel.DataAnnotations;

namespace AppText.Host.Controllers
{
    public class CreateViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}