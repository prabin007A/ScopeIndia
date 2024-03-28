using System.ComponentModel.DataAnnotations;

namespace ScopeIndiaWebsite.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Enter otp")]
        public string Otp { get; set; }

        public bool CheckBox { get; set; }
    }
}
