using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using FluentValidation.Attributes;

namespace JGCK.Web.Admin.Models
{
    [Validator(typeof(VmUserLoginValidator))]
    public class VmUserLogin
    {
        public string UserName { get; set; }

        public string Pwd { get; set; }

        public bool IsRememberPwd { get; set; }
    }

    public class VmUserLoginValidator : AbstractValidator<VmUserLogin>
    {
        public VmUserLoginValidator()
        {
            RuleFor(login => login.UserName)
                .NotEmpty()
                .WithMessage("用户名不能为空");
            RuleFor(login => login.Pwd)
                .NotEmpty()
                .WithMessage("密码不能为空");
        }
    }
}