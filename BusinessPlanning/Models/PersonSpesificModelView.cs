using BusinessPlanning.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessPlanning.Models
{
    public class PersonSpesificModelView
    {
        public AppUser ActiveUser { get; set; }

        public int CompletedTask { get; set; }
        public int AwaitingTask { get; set; }

        public int DeletedTask { get; set; }

        public AppTask LastCompletedTask { get; set; }
    }
}
