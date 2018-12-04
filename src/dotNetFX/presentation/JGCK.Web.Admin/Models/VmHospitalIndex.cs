using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using JGCK.Respority.BasicInfo;
using JGCK.Util;
using JGCK.Web.General.MVC;

namespace JGCK.Web.Admin.Models
{
    public class VmHospitalIndex : AbstractVoWithFilter<string, VmHospital>, ICustomFilter<Hospital>
    {
        public Expression<Func<Hospital, bool>> CombineExpression()
        {
            var exp = PredicateBuilder.Create<Hospital>(h => !h.IsDeleted);
            if (!string.IsNullOrEmpty(this.Filter))
            {
                exp = exp.And(h => (!string.IsNullOrEmpty(h.Name) && h.Name.Contains(this.Filter))
                                   || (!string.IsNullOrEmpty(h.HCode) && h.HCode.Contains(this.Filter)));
            }

            return exp;
        }
    }

    public class VmHospital : AbstractVO<Hospital>
    {

    }
}