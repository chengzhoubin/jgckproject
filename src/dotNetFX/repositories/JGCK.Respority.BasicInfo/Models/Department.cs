using System.Collections.Concurrent;

namespace JGCK.Respority.BasicInfo
{
    using JGCK.Framework.Repository;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Department")]
    public partial class Department : AbstractDomainEntity
    {
        [Required] [StringLength(150)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Desc { get; set; }
    }
}
