using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using FluentValidation.Attributes;
using JGCK.Web.Admin.Models.Validator;

namespace JGCK.Web.Admin.Models
{
    [Validator(typeof(VmUserLoginValidator))]
    public class VmUserLogin
    {
        public string UserName { get; set; }

        public string Pwd { get; set; }

        public bool IsRememberPwd { get; set; }
    }
}