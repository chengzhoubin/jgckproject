namespace JGCK.Respority.BasicInfo
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class BasicDbProxy : DbContext
    {
        public BasicDbProxy()
            : base("name=BasicDbProxy")
        {
        }

        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<Hospital> Hospital { get; set; }
        public virtual DbSet<OffDay> OffDay { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hospital>()
                .Property(e => e.SaleRate)
                .HasPrecision(2, 0);
        }
    }
}
