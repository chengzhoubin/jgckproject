using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;

namespace JGCK.Web.Admin.Models
{
    public class VmDepartmentValidator : AbstractValidator<VmDepartment>
    {
        public VmDepartmentValidator()
        {
            this.RuleFor(dep => dep.NagigatedDomainObject)
                .NotNull()
                .OverridePropertyName("部门对象");
            this.RuleFor(dep => dep.NagigatedDomainObject.Name)
                .NotEmpty()
                .When(dep => dep.NagigatedDomainObject != null)
                .OverridePropertyName("部门名称");
        }
    }
}