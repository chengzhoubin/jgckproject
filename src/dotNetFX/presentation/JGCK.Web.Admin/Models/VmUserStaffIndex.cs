using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using FluentValidation.Attributes;
using JGCK.Respority.UserWork;
using JGCK.Util;
using JGCK.Web.General.Extensions;
using JGCK.Web.General.MVC;

namespace JGCK.Web.Admin.Models
{
    public class VmUserStaffIndex : AbstractVoWithFilter<string, VmStaff>, ICustomFilter<Person>
    {
        public Expression<Func<Person, bool>> CombineExpression()
        {
            var exp = PredicateBuilder.Create<Person>(p => !p.IsDoctor && !p.IsDeleted);
            if (!string.IsNullOrEmpty(this.Filter))
            {
                exp = exp.And(p => (!string.IsNullOrEmpty(p.RealName) && p.RealName.Contains(Filter))
                                   || (!string.IsNullOrEmpty(p.Phone) && p.Phone.Contains(Filter)));
            }

            return exp;
        }

        public Gender Sex { get; set; }

        public OnJobType PersonType { get; set; }

        public EducationType EducationType { get; set; }

        public IEnumerable<SelectListItem> SexList {
            get
            {
                var ret = EnumHelper.GetSelectHtmlTag<Gender>(false);
                var selected = ret.FirstOrDefault(s => s.Value == ((int)Sex).ToString());
                if (selected != null)
                    selected.Selected = true;
                return ret;
            }
        }

        public IEnumerable<SelectListItem> PersonTypeList
        {
            get
            {
                var ret = EnumHelper.GetSelectHtmlTag<OnJobType>(false);
                var selected = ret.FirstOrDefault(s => s.Value == ((int)PersonType).ToString());
                if (selected != null)
                    selected.Selected = true;
                return ret;
            }
        }

        public IEnumerable<SelectListItem> EducationTypeList
        {
            get
            {
                var ret = EnumHelper.GetSelectHtmlTag<EducationType>(false);
                var selected = ret.FirstOrDefault(s => s.Value == ((int)EducationType).ToString());
                if (selected != null)
                    selected.Selected = true;
                return ret;
            }
        }
    }

    [Validator(typeof(VmStaffValidator))]
    public class VmStaff : AbstractVO<Person>
    {
    }
}