using BusinessPlanning.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessPlanning.Models
{
    public class ManagerViewModel
    {

        //aktif kullanıcı
        public AppUser   ActiveUser { get; set; }

        public IEnumerable<AppUser> ITUsers  { get; set; }

        public List<UserTaskCountModel> UserTaskCountList { get; set; }

        public List<SelectListItem> ITUserList { get; set; }
    }
}
