using JGCK.Framework.Repository;

namespace JGCK.Respority.ContentWork
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PortalColumn")]
    public partial class PortalColumn : AbstractDomainEntity, IEntity<ContentDbProxy>
    {
        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        public int? ColumnType { get; set; }

        public int? Status { get; set; }

        [StringLength(500)]
        public string Content { get; set; }
   }
}
