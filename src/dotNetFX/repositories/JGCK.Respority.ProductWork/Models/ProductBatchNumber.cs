using JGCK.Framework.Repository;

namespace JGCK.Respority.ProductWork
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProductBatchNumber")]
    public partial class ProductBatchNumber : AbstractDomainEntity, IEntity<ProductDbProxy>
    {
        public long ProductId { get; set; }

        public long BatchNumberInfoId { get; set; }

        [ForeignKey("BatchNumberInfoId")]
        public virtual BatchNumberInfo BatchNumberInfo { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}
