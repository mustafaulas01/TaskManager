using BusinessPlanning.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessPlanning.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser,AppRole,string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        
        }

    

        public DbSet<AppTask> AppTasks { get; set; }

        public DbSet<AppSettings> AppSettings { get; set; }

    }
}
