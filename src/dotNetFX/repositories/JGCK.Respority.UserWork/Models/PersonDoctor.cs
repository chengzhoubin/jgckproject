using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JGCK.Framework.Repository;

namespace JGCK.Respority.UserWork
{
    [Table("PersonDoctor")]
    public partial class PersonDoctor : AbstractDomainEntity
    {
        public virtual Person WithPerson { get; set; }

        public long Hospital { get; set; }

        [Required]
        [StringLength(150)]
        public string HospitalName { get; set; }

        [Required]
        [StringLength(150)]
        public string HospitalDepartment { get; set; }

        [Required]
        public string HosDepAddress { get; set; }

        [Required]
        [StringLength(50)]
        public string DoctorCode { get; set; }

        [Required]
        public string DoctorLicensePic { get; set; }

        [StringLength(30)]
        public string LinePhone { get; set; }

        [StringLength(30)]
        public string MobilePhone { get; set; }
    }
}
