using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using JGCK.Respority.UserWork;
using JGCK.Util.Helper;

namespace JGCK.Web.Admin.Models
{
    public class VmStaffValidator : AbstractValidator<VmStaff>
    {
        public VmStaffValidator()
        {
            this.RuleFor(staff => staff.NagigatedDomainObject).NotNull().OverridePropertyName("员工对象");
            this.RuleFor(staff => staff.NagigatedDomainObject.RoleId).GreaterThan(0)
                .When(staff => staff.NagigatedDomainObject != null)
                .OverridePropertyName("角色");
            this.RuleFor(staff => staff.NagigatedDomainObject.Name)
                .NotEmpty()
                .When(staff => staff.NagigatedDomainObject != null).OverridePropertyName("用户名");
            this.RuleFor(staff => staff.NagigatedDomainObject.Pwd)
                .NotEmpty()
                .When(staff => staff.NagigatedDomainObject != null).OverridePropertyName("密码");
            this.RuleFor(staff => staff.NagigatedDomainObject.PersonType)
                .Must(pt => pt > 0)
                .When(staff => staff.NagigatedDomainObject != null).OverridePropertyName("类型");
            this.RuleFor(staff => staff.NagigatedDomainObject.DepartmentId)
                .GreaterThan(0)
                .When(staff => staff.NagigatedDomainObject != null).OverridePropertyName("所属部门");
            this.RuleFor(staff => staff.NagigatedDomainObject.RealName)
                .NotEmpty()
                .When(staff => staff.NagigatedDomainObject != null).OverridePropertyName("姓名");
            this.RuleFor(staff => staff.NagigatedDomainObject.Sex)
                .Must(gender => gender > 0)
                .When(staff => staff.NagigatedDomainObject != null)
                .OverridePropertyName("性别");
            this.RuleFor(staff => staff.NagigatedDomainObject.IdCard)
                .NotEmpty().Matches(vf => RegexHelper.RegexChineseIDCardRule)
                .When(staff => staff.NagigatedDomainObject != null).OverridePropertyName("身份证");
            this.RuleFor(staff => staff.NagigatedDomainObject.HireDate)
                .NotEmpty()
                .When(staff => staff.NagigatedDomainObject != null).OverridePropertyName("入职时间");
            this.RuleFor(staff => staff.NagigatedDomainObject.Education)
                .Must(edu => edu > 0)
                .When(staff => staff.NagigatedDomainObject != null)
                .OverridePropertyName("学历");
            this.RuleFor(staff => staff.NagigatedDomainObject.GraduateInstitution)
                .NotEmpty()
                .When(staff => staff.NagigatedDomainObject != null)
                .OverridePropertyName("毕业院校/专业");
            this.RuleFor(staff => staff.NagigatedDomainObject.Phone)
                .NotEmpty()
                .Matches(vm => RegexHelper.RegexPhoneOrMobileRule)
                .When(staff => staff.NagigatedDomainObject != null).OverridePropertyName("联系电话");
            this.RuleFor(staff => staff.NagigatedDomainObject.FamliyAddress)
                .NotEmpty()
                .When(staff => staff.NagigatedDomainObject != null).OverridePropertyName("家庭住址");
            this.RuleFor(staff => staff.NagigatedDomainObject.EmergencyContact)
                .NotEmpty()
                .When(staff => staff.NagigatedDomainObject != null)
                .OverridePropertyName("紧急联系人");
            this.RuleFor(staff => staff.NagigatedDomainObject.EmergencyPhone)
                .NotEmpty().Matches(vm => RegexHelper.RegexPhoneOrMobileRule)
                .When(staff => staff.NagigatedDomainObject != null)
                .OverridePropertyName("紧急联系人电话");
            this.RuleFor(staff => staff.NagigatedDomainObject.Email)
                .NotEmpty()
                .EmailAddress()
                .When(staff => staff.NagigatedDomainObject != null)
                .OverridePropertyName("电子邮件");
            this.RuleFor(staff => staff.NagigatedDomainObject).Custom((ps, cc) =>
            {
                if (ps == null)
                    return;

                if (ps.PersonType == OnJobType.OnPractice && !ps.PracticeBeginDate.HasValue)
                {
                    cc.AddFailure("实习开始时间未设置");
                }
            });
        }
    }
}