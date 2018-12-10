using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;

namespace JGCK.Web.Admin.Models
{
    public class VmDoctorBindValidator : AbstractValidator<VmDoctorBind>
    {
        public VmDoctorBindValidator()
        {
            this.RuleFor(bind => bind.HospitalId).Must(dId => dId > 0).OverridePropertyName("医院Id");
            this.RuleFor(bind => bind.DoctorId).Must(dId => dId > 0).OverridePropertyName("医生Id");
            this.RuleFor(bind => bind.PreBindId).Must(pbId => pbId > 0).OverridePropertyName("绑定Id");
        }
    }
}