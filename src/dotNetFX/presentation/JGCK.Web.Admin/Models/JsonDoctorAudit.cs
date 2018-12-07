using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JGCK.Web.Admin.Models
{
    [Validator(typeof(JsonDoctorAudit))]
    public class JsonDoctorAudit
    {
        public long DoctorId { get; set; }

        public bool IsPass { get; set; }
    }
}