using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using JGCK.Respority.UserWork;
using JGCK.Util;
using JGCK.Web.General.MVC;
using JGCK.Web.General.VO;

namespace JGCK.Web.Admin.Models
{
    public class VmUserDoctorIndex : AbstractVoWithFilter<string, VmUserDoctorSimple>, ICustomFilter<Person>
    {
        public Expression<Func<Person, bool>> CombineExpression()
        {
            var exp = PredicateBuilder.Create<Person>(p => p.IsDoctor);
            if (!string.IsNullOrEmpty(this.Filter))
            {
                exp = exp.And(p => p.RealName.Contains(Filter)
                                   || p.Doctor.DoctorCode.Contains(Filter)
                                   || p.Doctor.LinePhone.Contains(Filter)
                                   || p.Doctor.MobilePhone.Contains(Filter));
            }

            return exp;
        }
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