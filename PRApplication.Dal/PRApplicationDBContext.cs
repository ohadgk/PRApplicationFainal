using PRApplication.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRApplication.Dal
{
    public class PRApplicationDBContext :DbContext
    {

        public PRApplicationDBContext()
            : base("name=PRApplicationEDMX")
            //: base("name=PRApplicationAzureConnection3")
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }

        public DbSet<Guest> Guests { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Guest>()
                .HasRequired(g => g.Event)
                .WithMany(e => e.Guests)
                .HasForeignKey(g => g.EventId);

        }
    }
}
