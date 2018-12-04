using JGCK.Framework.Repository;

namespace JGCK.Repority.OrderWork
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrderAttachFile")]
    public partial class OrderAttachFile : AbstractDomainEntity, IEntity<OrderDbProxy>
    {
        [StringLength(300)]
        public string FilePath { get; set; }

        public long OrderId { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }
    }
}
