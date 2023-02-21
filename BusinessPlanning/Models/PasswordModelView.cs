using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessPlanning.Models
{
    public class PasswordModelView
    {
        [Display(Name ="Eski Şifre")]
        [Required(ErrorMessage ="Eski Şifre Alanı doldurulmalıdır !")]
        public string OldPassword { get; set; }


        [Display(Name = "Yeni Şifre")]
        [Required(ErrorMessage = "Yeni Şifre Alanı doldurulmalıdır !")]
        public string NewPassword { get; set; }


        [Display(Name = "Şifre Tekrarı")]
        [Required(ErrorMessage = "Şifre Tekrarı doldurulmalıdır !")]
        public string rePassword { get; set; }

    }
}
