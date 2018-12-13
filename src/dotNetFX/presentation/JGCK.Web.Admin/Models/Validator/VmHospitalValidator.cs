using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using JGCK.Util.Helper;

namespace JGCK.Web.Admin.Models
{
    public class VmHospitalValidator : AbstractValidator<VmHospital>
    {
        public VmHospitalValidator()
        {
            this.RuleFor(h => h.NagigatedDomainObject).NotNull().OverridePropertyName("医院对象");
            this.RuleFor(h => h.NagigatedDomainObject.HCode).NotEmpty().When(h => h.NagigatedDomainObject != null).OverridePropertyName("医院编号");
            this.RuleFor(h => h.NagigatedDomainObject.Name).NotEmpty().When(h => h.NagigatedDomainObject != null).OverridePropertyName("医院名称");
            this.RuleFor(h => h.NagigatedDomainObject.Address).NotEmpty().When(h => h.NagigatedDomainObject != null).OverridePropertyName("医院地址");
            this.RuleFor(h => h.NagigatedDomainObject.HospitalType).Must(t => t > 0).When(h => h.NagigatedDomainObject != null).WithMessage("医院属性未选择");
            this.RuleFor(h => h.NagigatedDomainObject.PaymentType).Must(t => t > 0).When(h => h.NagigatedDomainObject != null).WithMessage("付款方式未选择");
            this.RuleFor(h => h.NagigatedDomainObject.PaymentPeriod).Must(t => t > 0).When(h => h.NagigatedDomainObject != null).WithMessage("付款周期未选择");
            this.RuleFor(h => h.NagigatedDomainObject.SaleRate).GreaterThan(0).LessThanOrEqualTo(1).When(h => h.NagigatedDomainObject != null).WithMessage("折扣系数格式输入错误").OverridePropertyName("折扣系数");
            this.RuleFor(h => h.NagigatedDomainObject).Custom((hi, vctx) =>
            {
                if (hi == null)
                    return;
                if (hi.HospitalInvoices?.Count > 0)
                {
                    var inv = hi.HospitalInvoices.FirstOrDefault();
                    if (inv == null)
                        return;

                    if (!string.IsNullOrEmpty(inv.CpPhone) && !RegexHelper.RegexPhoneOrMobileRule.IsMatch(inv.CpPhone))
                        vctx.AddFailure("发票联系人电话格式输入错误");
                    if (!string.IsNullOrEmpty(inv.FinanceDepPhone) && !RegexHelper.RegexPhoneOrMobileRule.IsMatch(inv.FinanceDepPhone))
                        vctx.AddFailure("财务科电话格式输入错误");
                    if (!string.IsNullOrEmpty(inv.InvoicePhone) && !RegexHelper.RegexPhoneOrMobileRule.IsMatch(inv.InvoicePhone))
                        vctx.AddFailure("开票电话格式输入错误");
                }
                if (hi.HospitalReferences?.Count > 0)
                {
                    var hosRef = hi.HospitalReferences.FirstOrDefault();
                    if (hosRef == null)
                        return;

                    if (!string.IsNullOrEmpty(hosRef.BcpEmail) && !RegexHelper.RegexEmailRule.IsMatch(hosRef.BcpEmail))
                        vctx.AddFailure("账单联系人邮箱格式输入错误");
                    if (!string.IsNullOrEmpty(hosRef.BcpPhone) && !RegexHelper.RegexPhoneOrMobileRule.IsMatch(hosRef.BcpPhone))
                        vctx.AddFailure("账单联系人电话格式输入错误");
                    if (!string.IsNullOrEmpty(hosRef.ReceipterPhone) && !RegexHelper.RegexPhoneOrMobileRule.IsMatch(hosRef.ReceipterPhone))
                        vctx.AddFailure("收件人电话格式输入错误");
                }
            });
        }
    }
}