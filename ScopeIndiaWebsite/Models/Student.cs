using System.ComponentModel.DataAnnotations;

namespace ScopeIndiaWebsite.Models
{
    public class Student
    {
        [Required(ErrorMessage = "Name is required")]
        public string First_name { get; set; }
        public string Last_name { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Date of birth is required")]
        [Display(Name = "Date Of Birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Enter email address")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter phone number")]
        [Phone] 
        public string Phone_number { get; set; }

        [Required(ErrorMessage = "Country is required")]
        public string Country { get; set; }

        [Required(ErrorMessage = "State is required")]
        public string State { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        public string Hobbie { get; set; }

        [Required(ErrorMessage = "Please upload image")]
        public string Avatar { get; set; }
    }
}
