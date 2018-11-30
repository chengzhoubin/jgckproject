using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using JGCK.Framework.EF;
using JGCK.Respority.UserWork;

namespace JGCK.Modules.Membership
{
    public class DoctorManager : AbstractMembershipService
    {
        public async Task<List<Person>> GetDoctorListAsync(
            Expression<Func<Person, bool>> search,
            AbstractUnitOfWork.OrderByExpression<Person>[] orderBy,
            int pageIndex)
        {
            var pager = new AbstractUnitOfWork.Pager { CurrentIndex = pageIndex };
            return await userDbContext.GetObjectsAsync(search, pager, false, orderBy, p => p.Doctor);
        }
    }
}
