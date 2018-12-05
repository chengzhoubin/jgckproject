using JGCK.Framework.Repository;

namespace JGCK.Repority.OrderWork
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Patient")]
    public partial class Patient : AbstractDomainEntity, IEntity<OrderDbProxy>
    {
        public Patient()
        {
            Order = new HashSet<Order>();
        }

        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        public DateTime? Birthday { get; set; }

        public bool Sex { get; set; }

        public virtual ICollection<Order> Order { get; set; }
    }
}
