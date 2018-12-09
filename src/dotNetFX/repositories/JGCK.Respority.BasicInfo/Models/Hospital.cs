using System.ComponentModel;
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
    public partial class Hospital : AbstractDomainEntityWithDeletedProperty, IEntity<BasicDbProxy>
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

        public string HospitalComments { get; set; }
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
        [Description("����")]
        Public = 1,

        /// <summary>
        /// ˽��
        /// </summary>
        [Description("˽��")]
        Private,

        /// <summary>
        /// ����
        /// </summary>
        [Description("����")]
        Group,

        /// <summary>
        /// ����
        /// </summary>
        [Description("����")]
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
        [Description("�����˺�")] Basic = 1,

        /// <summary>
        /// ֧����
        /// </summary>
        [Description("Alipay")] Alipay = 2,

        /// <summary>
        /// ����
        /// </summary>
        [Description("����")] Other = 4
    }

    [Flags]
    public enum PaymentPeriod
    {
        /// <summary>
        /// һ����
        /// </summary>
        [Description("һ����")] OneMonth = 1,

        /// <summary>
        /// Ԥ��
        /// </summary>
        [Description("Ԥ��")] PrePay = 2,

        /// <summary>
        /// ����
        /// </summary>
        [Description("����")] Other = 4
    }

    #endregion
}
