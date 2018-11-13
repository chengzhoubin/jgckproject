using JGCK.Framework.Repository;

namespace JGCK.Repority.OrderWork
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrderTrace")]
    public partial class OrderTrace : AbstractDomainEntity
    {
        [StringLength(200)]
        public string Comment { get; set; }

        public long PersonId { get; set; }

        public long OrderId { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }
    }
}
