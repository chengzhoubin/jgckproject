using JGCK.Framework.Repository;

namespace JGCK.Respority.UserWork
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Role")]
    public partial class Role : AbstractDomainEntity
    {
        public Role()
        {
            Person = new HashSet<Person>();
        }

        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        public virtual ICollection<Person> Person { get; set; }
    }
}
