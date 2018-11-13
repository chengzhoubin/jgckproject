namespace JGCK.Repority.OrderWork
{
    using JGCK.Framework.Repository;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Order")]
    public partial class Order : AbstractDomainEntity
    {
        public Order()
        {
            OrderAttachFile = new HashSet<OrderAttachFile>();
            OrderProduct = new HashSet<OrderProduct>();
            OrderTrace = new HashSet<OrderTrace>();
        }

        [Required]
        [StringLength(100)]
        public string OrderNO { get; set; }

        public int ProcessingType { get; set; }

        public int OrderStatus { get; set; }

        [StringLength(500)]
        public string Desc { get; set; }

        public DateTime? FinishTime { get; set; }

        public long? PatientId { get; set; }

        public long PersonId { get; set; }

        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }

        public virtual ICollection<OrderAttachFile> OrderAttachFile { get; set; }

        public virtual ICollection<OrderProduct> OrderProduct { get; set; }

        public virtual ICollection<OrderTrace> OrderTrace { get; set; }
    }
}
