using FluentValidation.Attributes;
using JGCK.Respority.ProductWork;
using JGCK.Util;
using JGCK.Web.Admin.Models.Validator;
using JGCK.Web.General.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace JGCK.Web.Admin.Models
{
    public class VmProductTypeIndex : AbstractVoWithFilter<string, VmProductType>, ICustomFilter<ProductTypeInfo>
    {
        public Expression<Func<ProductTypeInfo, bool>> CombineExpression()
        {
            var exp = PredicateBuilder.Create<ProductTypeInfo>(p => !string.IsNullOrEmpty(p.Name));
            //if (!string.IsNullOrEmpty(this.Filter))
            //{
            //    exp = exp.And(h => (!string.IsNullOrEmpty(p.Name) && p.Name.Contains(this.Filter))
            //                       || (!string.IsNullOrEmpty(h.HCode) && h.HCode.Contains(this.Filter)));
            //}

            return exp;
        }
    }

    [Validator(typeof(VmProductTypeInfoValidator))]
    public class VmProductType : AbstractVO<ProductTypeInfo>
    {

    }
}