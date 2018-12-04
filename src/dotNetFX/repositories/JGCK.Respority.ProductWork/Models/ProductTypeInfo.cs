using JGCK.Framework.Repository;

namespace JGCK.Respority.ProductWork
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProductTypeInfo")]
    public partial class ProductTypeInfo : AbstractDomainEntity, IEntity<ProductDbProxy>
    {
        public ProductTypeInfo()
        {
            Product = new HashSet<Product>();
        }

        [StringLength(100)]
        public string ProductNO { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        public virtual ICollection<Product> Product { get; set; }
    }
}
