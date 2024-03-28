using System.ComponentModel.DataAnnotations;

namespace ScopeIndiaWebsite.Models
{
    public class LogIn
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
