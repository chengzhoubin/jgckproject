﻿using System;
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
                exp = exp.And(p => p.RealName.Contains(Filter)
                                   || p.Phone.Contains(Filter));
            }

            return exp;
        }

        public Gender Sex { get; set; }

        public IEnumerable<SelectListItem> SexList => EnumHelper.GetSelectItemByEnum<Gender>(true);
    }

    [Validator(typeof(VmStaffValidator))]
    public class VmStaff : AbstractVO<Person>
    {
    }
}