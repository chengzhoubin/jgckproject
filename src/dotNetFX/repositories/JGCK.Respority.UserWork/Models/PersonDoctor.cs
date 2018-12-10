using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JGCK.Framework.Repository;

namespace JGCK.Respority.UserWork
{
    [Table("PersonDoctor")]
    public class PersonDoctor : AbstractDomainEntity, IEntity<UserDbProxy>
    {
        public PersonDoctor()
        {
            InHospital = new HashSet<DoctorInHospital>();
        }

        public Person WithPerson { get; set; }

        [Required] [StringLength(50)] public string DoctorCode { get; set; }

        public DateTime Birthday { get; set; }

        [Required] [StringLength(100)] public string Email { get; set; }

        [Required] public string DoctorLicensePic { get; set; }

        [StringLength(30)] public string LinePhone { get; set; }

        [StringLength(30)] public string MobilePhone { get; set; }

        public DoctorAuditStatus AuditStatus { get; set; }

        public DateTime? AuditDate { get; set; }

        //public string PrebindInfo { get; set; }

        public virtual ICollection<DoctorInHospital> InHospital { get; set; }
    }

    [Table("DoctorHospital")]
    public class DoctorInHospital : AbstractDomainEntity
    {
        public long? BindedHospitalId { get; set; }

        [StringLength(150)] public string BindedHospitalName { get; set; }

        [Required] [StringLength(150)] public string CustomHospitalName { get; set; }

        [Required] [StringLength(150)] public string HospitalDepartment { get; set; }

        [Required] public string HosDepAddress { get; set; }

        [ForeignKey("PersonDoctorId")] public PersonDoctor Doctor { get; set; }

        public long PersonDoctorId { get; set; }

        public bool IsBinded { get; set; }
    }

    /// <summary>
    /// 审核状态
    /// </summary>
    public enum DoctorAuditStatus
    {
        /// <summary>
        /// 待审核
        /// </summary>
        [Description("待审核")]
        Pending,
        /// <summary>
        /// 审核成功
        /// </summary>
        [Description("审核通过")]
        Pass,
        /// <summary>
        /// 审核失败
        /// </summary>
        [Description("审核不通过")]
        Fail = -1001
    }
}
