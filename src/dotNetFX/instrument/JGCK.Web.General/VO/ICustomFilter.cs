using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JGCK.Web.General.MVC
{
    public interface ICustomFilter<TDomainObject>
    {
        Expression<Func<TDomainObject, bool>> CombineExpression();
    }
}
