using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;

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
            this.RuleFor(h => h.NagigatedDomainObject.SaleRate).GreaterThan(0).LessThanOrEqualTo(1).WithMessage("折扣系数格式输入错误");
        }
    }
}