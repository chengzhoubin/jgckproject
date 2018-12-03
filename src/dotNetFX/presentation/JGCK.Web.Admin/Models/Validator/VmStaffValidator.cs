using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using JGCK.Respority.UserWork;

namespace JGCK.Web.Admin.Models
{
    public class VmStaffValidator : AbstractValidator<VmStaff>
    {
        public VmStaffValidator()
        {
            this.RuleFor(staff => staff.NagigatedDomainObject.RoleId).GreaterThan(0).OverridePropertyName("角色");
            this.RuleFor(staff => staff.NagigatedDomainObject.Name).NotEmpty().OverridePropertyName("用户名");
            this.RuleFor(staff => staff.NagigatedDomainObject.Pwd).NotEmpty().OverridePropertyName("密码");
            this.RuleFor(staff => staff.NagigatedDomainObject.PersonType).Must(pt => pt > 0).OverridePropertyName("类型");
            this.RuleFor(staff => staff.NagigatedDomainObject.DepartmentId).GreaterThan(0).OverridePropertyName("所属部门");
            this.RuleFor(staff => staff.NagigatedDomainObject.RealName).NotEmpty().OverridePropertyName("姓名");
            this.RuleFor(staff => staff.NagigatedDomainObject.Sex).Must(gender => gender > 0).OverridePropertyName("性别");
            this.RuleFor(staff => staff.NagigatedDomainObject.IdCard).NotEmpty().OverridePropertyName("身份证");
            this.RuleFor(staff => staff.NagigatedDomainObject.HireDate).NotEmpty().OverridePropertyName("入职时间");
            this.RuleFor(staff => staff.NagigatedDomainObject.Education).Must(edu => edu > 0).OverridePropertyName("学历");
            this.RuleFor(staff => staff.NagigatedDomainObject.GraduateInstitution).NotEmpty().OverridePropertyName("毕业院校/专业");
            this.RuleFor(staff => staff.NagigatedDomainObject.Phone).NotEmpty().OverridePropertyName("联系电话");
            this.RuleFor(staff => staff.NagigatedDomainObject.FamliyAddress).NotEmpty().OverridePropertyName("家庭住址");
            this.RuleFor(staff => staff.NagigatedDomainObject.EmergencyContact).NotEmpty().OverridePropertyName("紧急联系人");
            this.RuleFor(staff => staff.NagigatedDomainObject.EmergencyPhone).NotEmpty()
                .OverridePropertyName("紧急联系人电话");
            this.RuleFor(staff => staff.NagigatedDomainObject).Custom((ps, cc) =>
            {
                if (ps.PersonType == OnJobType.OnPractice && !ps.PracticeBeginDate.HasValue)
                {
                    cc.AddFailure("实习开始时间未设置");
                }
            });
        }
    }
}