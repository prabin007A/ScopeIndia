using System.ComponentModel.DataAnnotations;

namespace ScopeIndiaWebsite.Models
{
    public class StudentUpdateModel
    {
        public int Id { get; set; }
        public string First_name { get; set; }
        public string Last_name { get; set; }
        public string Gender { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Phone_number { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Hobbie { get; set; }
        public string Avatar { get; set; }
    }
}
