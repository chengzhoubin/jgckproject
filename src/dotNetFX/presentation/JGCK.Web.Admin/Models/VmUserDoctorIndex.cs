using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JGCK.Web.Admin.Models
{
    public class VmUserDoctorIndex
    {
        public string Search { get; set; }

        public VmUserDoctorSimple SimpleDoctor { get; set; }
    }

    public class VmUserDoctorSimple
    {
        public long UserID { get; set; }

        public string DoctorName { get; set; }

        public string DoctorCode { get; set; }

        public string LinePhone { get; set; }

        public string MobilePhone { get; set; }

        public AduitStatus AduitStatus { get; set; }

        public DateTime? AduitDate { get; set; }
    }

    public enum AduitStatus
    {
        Pending,
        Fail,
        Pass
    }
}