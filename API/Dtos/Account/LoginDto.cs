using System.ComponentModel.DataAnnotations;

namespace API.Dtos.Account
{
    public class LoginDto
    {
        [Required(ErrorMessage ="UserName is required")]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
