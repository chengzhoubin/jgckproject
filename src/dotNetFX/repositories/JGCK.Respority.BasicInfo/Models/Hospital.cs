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

    #region 枚举类型

    /// <summary>
    /// 医院类型
    /// </summary>
    public enum HospitalType
    {
        /// <summary>
        /// 公立
        /// </summary>
        [Description("公立")]
        Public = 1,

        /// <summary>
        /// 私立
        /// </summary>
        [Description("私立")]
        Private,

        /// <summary>
        /// 集团
        /// </summary>
        [Description("集团")]
        Group,

        /// <summary>
        /// 其他
        /// </summary>
        [Description("其他")]
        Other
    }

    /// <summary>
    /// 支付类型
    /// </summary>
    [Flags]
    public enum PaymentType
    {
        /// <summary>
        /// 基本账号
        /// </summary>
        [Description("基本账号")] Basic = 1,

        /// <summary>
        /// 支付宝
        /// </summary>
        [Description("Alipay")] Alipay = 2,

        /// <summary>
        /// 其他
        /// </summary>
        [Description("其他")] Other = 4
    }

    [Flags]
    public enum PaymentPeriod
    {
        /// <summary>
        /// 一个月
        /// </summary>
        [Description("一个月")] OneMonth = 1,

        /// <summary>
        /// 预付
        /// </summary>
        [Description("预付")] PrePay = 2,

        /// <summary>
        /// 其他
        /// </summary>
        [Description("其他")] Other = 4
    }

    #endregion
}
