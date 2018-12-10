using JGCK.Respority.ProductWork;
using JGCK.Web.General.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;
using JGCK.Util;
using FluentValidation.Attributes;
using JGCK.Web.Admin.Models.Validator;

namespace JGCK.Web.Admin.Models
{
    public class VmProductfIndex : AbstractVoWithFilter<string, VmProduct>, ICustomFilter<Product>
    {
        public Expression<Func<Product, bool>> CombineExpression()
        {
            var exp = PredicateBuilder.Create<Product>(p => !string.IsNullOrEmpty(p.Name));
            //if (!string.IsNullOrEmpty(this.Filter))
            //{
            //    exp = exp.And(h => (!string.IsNullOrEmpty(p.Name) && p.Name.Contains(this.Filter))
            //                       || (!string.IsNullOrEmpty(h.HCode) && h.HCode.Contains(this.Filter)));
            //}

            return exp;
        }
    }

    [Validator(typeof(VmProductValidator))]
    public class VmProduct : AbstractVO<Product>
    {

    }
}