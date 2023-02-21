using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessPlanning.Models
{
    public class AddOrUpdateAppointmentViewModel
    {

        public int Id { get; set; }
        public string UserId { get; set; }
        public int AppUserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string TitleName { get; set; }
        public string Decription { get; set; }
        public bool IsCompleted { get; set; }

        public bool IsDeleted { get; set; }
    }
}
