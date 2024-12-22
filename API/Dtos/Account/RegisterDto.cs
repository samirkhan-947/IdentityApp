using System.ComponentModel.DataAnnotations;

namespace API.Dtos.Account
{
    public class RegisterDto
    {
        [Required]
        [StringLength(15,MinimumLength =3,ErrorMessage ="First Name must be at least {2},and maximum {1} charcters")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "Last Name must be at least {2},and maximum {1} charcters")]
        public string LastName { get; set; }
        [Required]
        [RegularExpression("^[\\w\\.=-]+@[\\w\\.-]+\\.[\\w]{2,3}$", ErrorMessage ="Invalid email address")]
        public string Email { get; set; }
        [Required]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "Password must be at least {2},and maximum {1} charcters")]
        public string Password { get; set; }
    }
}
