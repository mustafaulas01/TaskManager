using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessPlanning.Models
{
    public class LoginViewModel
    {

        [DisplayName("Username")]
        [Required(ErrorMessage = "{0} Field Must not be empty")]
        [MaxLength(100, ErrorMessage = "{0} Must be a maximum of {1} characters")]
        [MinLength(3, ErrorMessage = "{0} En az {1} Karakter Olmalı")]
        public string UserName { get; set; }

        [DisplayName("Password")]
        [Required(ErrorMessage = "{0} Field Must not be empty")]
        [MinLength(6, ErrorMessage = "{0} Must be a minumum of {1} characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("Remember me ?")]
        public bool RememberMe { get; set; }
    }
}
