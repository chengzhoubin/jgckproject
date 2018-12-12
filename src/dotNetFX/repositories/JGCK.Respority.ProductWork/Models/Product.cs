using JGCK.Framework.Repository;

namespace JGCK.Respority.ProductWork
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Product")]
    public partial class Product : AbstractDomainEntity, IEntity<ProductDbProxy>
    {
        public Product()
        {
            ProductBatchNumber = new HashSet<ProductBatchNumber>();
        }

        [StringLength(100)]
        public string ProductNO { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        public DateTime? BeginDate { get; set; }

        public DateTime? EndDate { get; set; }

        [StringLength(100)]
        public string VersionNO { get; set; }

        public decimal? Price { get; set; }

        public long? Output { get; set; }

        public long? CreaterId { get; set; }

        public long? ProductTypeId { get; set; }

        [ForeignKey("ProductTypeId")]
        public virtual ProductTypeInfo ProductTypeInfo { get; set; }

        public virtual ICollection<ProductBatchNumber> ProductBatchNumber { get; set; }

        public bool IsDeleted { get; set; }
    }
}
