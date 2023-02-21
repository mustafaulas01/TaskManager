using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessPlanning.Models
{
    public class RegisterViewModel
    {
        [Display(Name = "User Name")]
        [Required(ErrorMessage ="Name Cannot be Empty")]
      
        public string UserName { get; set; }

        [Display(Name = "FirstName")]
        [Required(ErrorMessage = "FirstName Cannot be Empty")]
        public string FirstName { get; set; }
        [Display(Name = "LastName")]
        [Required(ErrorMessage = "LastName Cannot be Empty")]
        public string LastName { get; set; }
        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password Cannot be Empty")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email Cannot be Empty")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Is Manager")]
        [Required(ErrorMessage = "Is Manager can not be Empty")]
        public bool IsManager { get; set; }

        [Display(Name = "Color")]
        [Required(ErrorMessage = "Color can not be Empty")]
        public string Color { get; set; }

    }
}
