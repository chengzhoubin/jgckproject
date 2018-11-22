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
    [Table("HospitalInvoice")]
    public class HospitalInvoice : AbstractDomainEntity
    {
        [Required] [StringLength(100)] public string Title { get; set; }

        [Required] [StringLength(50)] public string TaxerNumber { get; set; }

        public string InvoiceAddress { get; set; }

        [StringLength(30)] public string InvoicePhone { get; set; }

        [StringLength(50)] public string Bank { get; set; }

        [StringLength(50)] public string BankAccountNumber { get; set; }

        [StringLength(20)] public string ContractPerson { get; set; }

        [StringLength(10)] public string CpJobTitle { get; set; }

        [StringLength(50)] public string CpPhone { get; set; }

        public string CpAddress { get; set; }

        [StringLength(50)] public string FinanceDepPhone { get; set; }

        public string Note { get; set; }

        public long HospitalId { get; set; }

        [ForeignKey("HospitalId")]
        public virtual Hospital WithHospital { get; set; }
    }
}
