using System;
using System.ComponentModel.DataAnnotations;

namespace MvcWebAppCore3.Models
{
    public class Person
    {
        [Key]
        public int PersonId { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Please enter your last name"), MaxLength(50)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter your first name"), MaxLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        public DateTime? HireDate { get; set; }

        public DateTime? EnrollmentDate { get; set; }

        [Required(ErrorMessage = "Please enter discriminator value"), MaxLength(50)]
        [Display(Name = "Discriminator")]
        public string Discriminator { get; set; }
    }
}
