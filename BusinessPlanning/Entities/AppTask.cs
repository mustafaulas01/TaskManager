using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessPlanning.Entities
{
    public class AppTask
    {
        public int Id { get; set; }
        public string AppUserId { get; set; }
        public virtual AppUser AppUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string TitleName { get; set; }
        public string Decription { get; set; }
        public bool IsCompleted { get; set; }

        public bool IsDeleted { get; set; }
    }
}
