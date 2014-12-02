namespace WCF_Azure_Service
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class PRApplicationEDMX : DbContext
    {
        public PRApplicationEDMX()
            : base("name=PRApplicationEDMX")
        {
        }

        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<Guest> Guests { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
