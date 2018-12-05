using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JGCK.Framework.Repository
{
    /// <summary>
    /// 领域模型
    /// </summary>
    public abstract class AbstractDomainEntity
    {
        //public DateTime create_date { get; set; } = DateTime.Now;

        //public DateTime modify_date { get; set; } = DateTime.Now;
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;
    }

    public abstract class AbstractDomainEntityWithDeletedProperty : AbstractDomainEntity
    {
        public bool IsDeleted { get; set; }
    }
}
