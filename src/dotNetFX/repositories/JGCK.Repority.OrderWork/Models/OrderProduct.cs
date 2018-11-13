using JGCK.Framework.Repository;

namespace JGCK.Repority.OrderWork
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrderProduct")]
    public partial class OrderProduct : AbstractDomainEntity
    {
        public long? ProductCount { get; set; }

        public decimal Price { get; set; }

        public long OrderId { get; set; }

        public long ProductId { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }
    }
}
