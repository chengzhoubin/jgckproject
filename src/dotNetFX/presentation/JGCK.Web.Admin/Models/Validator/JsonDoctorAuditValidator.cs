using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JGCK.Web.Admin.Models
{
    public class JsonDoctorAuditValidator : AbstractValidator<JsonDoctorAudit>
    {
        public JsonDoctorAuditValidator()
        {
            this.RuleFor(audit => audit.DoctorId).Must(i => i > 0).OverridePropertyName("医生ID");
        }
    }
}