using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;

namespace JGCK.Web.Admin.Models.Validator
{
    public class VmDoctorValidator : AbstractValidator<VmUserDoctor>
    {
        public VmDoctorValidator()
        {
            this.RuleFor(ud => ud.NagigatedDomainObject.Name)
                .NotNull()
                .NotEmpty().WithMessage("医生用户名不能为空");
            this.RuleFor(ud => ud.NagigatedDomainObject.RealName)
                .NotNull()
                .NotEmpty().WithMessage("医生姓名不能为空");
            this.RuleFor(ud=>ud.NagigatedDomainObject.Pwd)
                .NotNull()
                .NotEmpty().WithMessage("密码不能为空");
            this.RuleFor(ud => ud.NagigatedDomainObject.Doctor.InHospital)
                .Must(h => h?.Count > 0)
                .WithMessage("所属医院，所属科室或者科室地址不能为空");
            this.RuleFor(ud=>ud.NagigatedDomainObject.Doctor.DoctorCode)
                .NotNull()
                .NotEmpty().WithMessage("医生编号不能为空");
            this.RuleFor(ud => ud.NagigatedDomainObject.Doctor.DoctorLicensePic)
                .NotNull()
                .NotEmpty().WithMessage("医师资格证不能为空");
            this.RuleFor(v => v.NagigatedDomainObject.Doctor).Custom((pd, cc) =>
            {
                if (string.IsNullOrEmpty(pd?.LinePhone) && string.IsNullOrEmpty(pd?.MobilePhone))
                {
                    cc.AddFailure("医生固定电话或者手机号不能都为空");
                }
            });
        }
    }
}