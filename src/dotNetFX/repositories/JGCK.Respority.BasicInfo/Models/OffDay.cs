using JGCK.Framework.Repository;

namespace JGCK.Respority.BasicInfo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OffDay")]
    public partial class OffDay : AbstractDomainEntity
    {
        public DateTime? NonworkDate { get; set; }
    }
}
