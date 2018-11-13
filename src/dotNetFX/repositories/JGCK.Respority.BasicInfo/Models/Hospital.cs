using JGCK.Framework.Repository;

namespace JGCK.Respority.BasicInfo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Hospital")]
    public partial class Hospital : AbstractDomainEntity
    {
        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        [StringLength(150)]
        public string Address { get; set; }

        [StringLength(20)]
        public string Grade { get; set; }

        [StringLength(50)]
        public string ContactNO { get; set; }

        public decimal? SaleRate { get; set; }
    }
}
