using JGCK.Framework.EF;

namespace JGCK.Repority.OrderWork
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class OrderDbProxy : AbstractUnitOfWork
    {
        public OrderDbProxy()
            : base("name=OrderDbProxy")
        {
        }

        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<OrderAttachFile> OrderAttachFile { get; set; }
        public virtual DbSet<OrderProduct> OrderProduct { get; set; }
        public virtual DbSet<OrderTrace> OrderTrace { get; set; }
        public virtual DbSet<Patient> Patient { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .Property(e => e.OrderNO)
                .IsUnicode(false);

            modelBuilder.Entity<Order>()
                .HasMany(e => e.OrderAttachFile)
                .WithRequired(e => e.Order)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Order>()
                .HasMany(e => e.OrderProduct)
                .WithRequired(e => e.Order)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Order>()
                .HasMany(e => e.OrderTrace)
                .WithRequired(e => e.Order)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<OrderProduct>()
                .Property(e => e.Price)
                .HasPrecision(2, 0);
        }
    }
}
