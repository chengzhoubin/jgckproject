using JGCK.Framework.EF;

namespace JGCK.Respority.ProductWork
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ProductDbProxy : AbstractUnitOfWork
    {
        public ProductDbProxy()
            : base("name=ProductDbProxy")
        {
        }

        public virtual DbSet<BatchNumberInfo> BatchNumberInfo { get; set; }
        public virtual DbSet<Material> Material { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductBatchNumber> ProductBatchNumber { get; set; }
        public virtual DbSet<ProductionGroup> ProductionGroup { get; set; }
        public virtual DbSet<ProductTypeInfo> ProductTypeInfo { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BatchNumberInfo>()
                .Property(e => e.BatchNO)
                .IsUnicode(false);

            //modelBuilder.Entity<BatchNumberInfo>().HasOptional(e => e.ProductBatchNumber);
            modelBuilder.Entity<BatchNumberInfo>().HasMany(e => e.ProductBatchNumber);
                

            modelBuilder.Entity<Material>()
                .Property(e => e.MaterialNO)
                .IsUnicode(false);

            modelBuilder.Entity<Material>()
                .HasMany(e => e.BatchNumberInfo)
                .WithRequired(e => e.Material)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.ProductNO)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.VersionNO)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.Price)
                .HasPrecision(2, 0);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.ProductBatchNumber)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ProductTypeInfo>()
                .Property(e => e.ProductNO)
                .IsUnicode(false);

            modelBuilder.Entity<ProductTypeInfo>()
                .HasMany(e => e.Product)
                .WithOptional(e => e.ProductTypeInfo)
                .HasForeignKey(e => e.ProductTypeId);
        }
    }
}
