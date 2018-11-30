using JGCK.Framework;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using JGCK.Framework.EF;
using JGCK.Respority.BasicInfo;
using JGCK.Respority.UserWork;

namespace JGCK.Modules.Membership
{
    public abstract class AbstractMembershipService : AbstractDefaultAppService, ITransistService
    {
        protected BasicDbProxy basicDbContext { get; set; }

        protected UserDbProxy userDbContext { get; set; }

        //public Expression<Func<T, TProp>> GenerateOrderExpression<T, TProp>(string propertyName)
        //{
        //    var propInfo = typeof(T).GetProperty(propertyName);
        //    return propInfo._GetLamba<T, TProp>();
        //}
    }
}
