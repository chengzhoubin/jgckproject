using JGCK.Framework.Repository;

namespace JGCK.Respority.ProductWork
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Material")]
    public partial class Material : AbstractDomainEntity, IEntity<ProductDbProxy>
    {
        public Material()
        {
            BatchNumberInfo = new HashSet<BatchNumberInfo>();
        }

        [StringLength(100)] public string MaterialNO { get; set; }

        [Required] [StringLength(150)] public string Name { get; set; }

        public virtual ICollection<BatchNumberInfo> BatchNumberInfo { get; set; }
    }
}
