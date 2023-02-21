using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessPlanning.Controllers
{
    public class AppointmentViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }

        public string UserId { get; set; }
        public string TaskTitle { get; set; }
        public string TaskDescription { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Color { get; set; }
    }
}
