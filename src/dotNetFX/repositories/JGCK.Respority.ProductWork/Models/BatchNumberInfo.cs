using JGCK.Framework.Repository;

namespace JGCK.Respority.ProductWork
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BatchNumberInfo")]
    public partial class BatchNumberInfo : AbstractDomainEntity
    {
        public BatchNumberInfo()
        {
            this.ProductBatchNumber = new HashSet<ProductBatchNumber>();
        }

        [Required]
        [StringLength(100)]
        public string BatchNO { get; set; }

        [StringLength(200)]
        public string Desc { get; set; }

        public bool IsActive { get; set; }

        public long MaterialId { get; set; }

        public virtual Material Material { get; set; }

        public virtual ICollection<ProductBatchNumber> ProductBatchNumber { get; set; }
    }
}
