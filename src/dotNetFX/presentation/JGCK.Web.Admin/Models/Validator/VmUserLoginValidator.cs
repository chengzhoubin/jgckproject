using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;

namespace JGCK.Web.Admin.Models.Validator
{
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