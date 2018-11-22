using JGCK.Framework.Repository;
using JGCK.Respority.BasicInfo.Models;

namespace JGCK.Respority.BasicInfo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Hospital")]
    public partial class Hospital : AbstractDomainEntity
    {
        public Hospital()
        {
            HospitalReferences = new HashSet<HospitalReference>();
            HospitalInvoices = new HashSet<HospitalInvoice>();
        }

        [Required] [StringLength(150)] public string Name { get; set; }

        [Required] [StringLength(20)] public string HCode { get; set; }

        //[StringLength(150)]
        public string Address { get; set; }

        public string BusinessLicensePic { get; set; }

        public string MedicalInstitutionLicensePic { get; set; }

        public string CertificationPic { get; set; }

        public virtual ICollection<HospitalReference> HospitalReferences { get; set; }

        public virtual ICollection<HospitalInvoice> HospitalInvoices { get; set; }

        [StringLength(20)] public string Grade { get; set; }

        [StringLength(50)] public string ContactNO { get; set; }

        public decimal SaleRate { get; set; }

        public HospitalType HospitalType { get; set; }

        public PaymentType PaymentType { get; set; }

        public string PaymentNote { get; set; }

        public PaymentPeriod PaymentPeriod { get; set; }

        public string PaymentPeriodNote { get; set; }
    }

    #region ö������

    /// <summary>
    /// ҽԺ����
    /// </summary>
    public enum HospitalType
    {
        /// <summary>
        /// ����
        /// </summary>
        Public = 1,

        /// <summary>
        /// ˽��
        /// </summary>
        Private,

        /// <summary>
        /// ����
        /// </summary>
        Group,

        /// <summary>
        /// ����
        /// </summary>
        Other
    }

    /// <summary>
    /// ֧������
    /// </summary>
    [Flags]
    public enum PaymentType
    {
        /// <summary>
        /// �����˺�
        /// </summary>
        Basic = 2,

        /// <summary>
        /// ֧����
        /// </summary>
        Alipay,

        /// <summary>
        /// ����
        /// </summary>
        Other
    }

    [Flags]
    public enum PaymentPeriod
    {
        /// <summary>
        /// һ����
        /// </summary>
        OneMonth = 2,

        /// <summary>
        /// Ԥ��
        /// </summary>
        PrePay,

        /// <summary>
        /// ����
        /// </summary>
        Other
    }

    #endregion
}
