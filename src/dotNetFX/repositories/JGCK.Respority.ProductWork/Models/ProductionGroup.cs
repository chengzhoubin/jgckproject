using JGCK.Framework.Repository;

namespace JGCK.Respority.ProductWork
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProductionGroup")]
    public partial class ProductionGroup : AbstractDomainEntity
    {
    }
}
