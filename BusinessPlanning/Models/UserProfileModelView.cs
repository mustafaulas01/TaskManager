using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessPlanning.Models
{
    public class UserProfileModelView
    {

        public string UserId { get; set; }

        [Display(Name ="Kullanıcı Adı")]
        [Required(ErrorMessage ="Kullanıcı Adı Boş Bırakılamaz")]
        public string UserName { get; set; }
        [Display(Name = "Renk Kartı")]
        [Required(ErrorMessage = "Renk Kartı Boş Bırakılamaz")]
        public string Color { get; set; }

        [Display(Name = "Adınız")]
        [Required(ErrorMessage = "Ad Alanı Boş Bırakılamaz")]
        public string FirstName { get; set; }
        [Display(Name = "Soyadınız")]
        [Required(ErrorMessage = "Soyad Alanı Boş Bırakılamaz")]
        public string LastName { get; set; }


        public int CompletedTask { get; set; }

        public int AwaitingTask { get; set; }

        public int DeletedTask { get; set; }

    }
}
