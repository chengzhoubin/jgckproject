using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation.Attributes;
using JGCK.Respority.BasicInfo;
using JGCK.Respority.UserWork;
using JGCK.Web.General.MVC;

namespace JGCK.Web.Admin.Models
{
    [Validator(typeof(VmDepartmentValidator))]
    public class VmDepartment : AbstractVO<Department>
    {
    }
}