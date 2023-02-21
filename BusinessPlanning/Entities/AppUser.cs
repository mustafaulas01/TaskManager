using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessPlanning.Entities
{
    public class AppUser:IdentityUser
    {
        public bool IsManager { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Color { get; set; }

        public List<AppTask> AppTasks { get; set; }
    }
}
