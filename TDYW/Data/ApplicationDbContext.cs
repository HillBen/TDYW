using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TDYW.Models;

namespace TDYW.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<Pool> Pools { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<Invitation> Invitations { get; set; }

        public DbSet<Recipient> Recipients { get; set; }

        public DbSet<Person> Persons { get; set; }

        public DbSet<Pick> Picks { get; set; }

        public DbSet<Judgement> Rulings { get; set; }

        public DbSet<Complaint> Complaints { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
