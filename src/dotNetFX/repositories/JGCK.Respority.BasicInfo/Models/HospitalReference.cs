using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JGCK.Framework.Repository;

namespace JGCK.Respority.BasicInfo.Models
{
    [Table("HospitalReference")]
    public class HospitalReference : AbstractDomainEntity, IEntity<BasicDbProxy>
    {
        [StringLength(20)]
        public string Receipter { get; set; }

        [StringLength(50)]
        public string ReceipterJobTitle { get; set; }

        [StringLength(50)]
        public string ReceipterPhone { get; set; }

        [StringLength(20)]
        public string BillContactPerson { get; set; }

        [StringLength(50)]
        public string BcpJobTitle { get; set; }

        [StringLength(50)]
        public string BcpPhone { get; set; }

        [StringLength(100)]
        public string BcpEmail { get; set; }

        //[Required]
        public long? HospitalId { get; set; }

        [ForeignKey("HospitalId")]
        public virtual Hospital WithHospital { get; set; }
    }
}
