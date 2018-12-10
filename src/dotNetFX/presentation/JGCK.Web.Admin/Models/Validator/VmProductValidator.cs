using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JGCK.Web.Admin.Models.Validator
{
    public class VmProductValidator : AbstractValidator<VmProduct>
    {
        public VmProductValidator()
        {
            //this.RuleFor(bind => bind).Must(dId => dId > 0).OverridePropertyName("医院Id");
            //this.RuleFor(bind => bind.DoctorId).Must(dId => dId > 0).OverridePropertyName("医生Id");
            //this.RuleFor(bind => bind.PreBindId).Must(pbId => pbId > 0).OverridePropertyName("绑定Id");
        }
    }
}