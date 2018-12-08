using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation.Attributes;

namespace JGCK.Web.Admin.Models
{
    [Validator(typeof(VmDoctorBindValidator))]
    public class VmDoctorBind
    {
        public long HospitalId { get; set; }

        public long DoctorId { get; set; }

        public long PreBindId { get; set; }
    }
}