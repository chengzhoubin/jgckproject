using JGCK.Framework.EF;
using JGCK.Respority.BasicInfo.Models;

namespace JGCK.Respority.BasicInfo
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using ProductWork;

    public partial class BasicDbProxy : AbstractUnitOfWork
    {
        public BasicDbProxy()
            : base("name=BasicDbProxy")
        {
        }

        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<Hospital> Hospital { get; set; }

        public virtual DbSet<OffDay> OffDay { get; set; }
        public virtual DbSet<HospitalInvoice> HospitalInvoice { get; set; }
        public virtual DbSet<HospitalReference> HospitalReference { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hospital>()
                .Property(e => e.SaleRate)
                .HasPrecision(2, 0);
        }
    }
}
