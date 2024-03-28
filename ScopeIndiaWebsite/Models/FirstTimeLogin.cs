using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ScopeIndiaWebsite.Models
{
    public class FirstTimeLogin
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter otp")]
        public string Otp { get; set; }

        [Required(ErrorMessage = "Enter password")]
        public string Password { get; set; }
    }
}
