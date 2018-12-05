using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using JGCK.Framework.EF;
using JGCK.Respority.BasicInfo;
using JGCK.Respority.UserWork;

namespace JGCK.Modules.Configuration
{
    public class HospitalManager : AbstractConfigurationService
    {
        public Task<List<Hospital>> GetHospitalListAsync(
            Expression<Func<Hospital, bool>> search,
            AbstractUnitOfWork.OrderByExpression<Hospital>[] orderBy,
            int pageIndex)
        {
            var pager = new AbstractUnitOfWork.Pager {CurrentIndex = pageIndex};
            return basicDbContext.GetObjectsAsync(
                search,
                pager,
                false,
                orderBy,
                p => p.HospitalInvoices,
                p => p.HospitalReferences);
        }

        public Task<int> GetHospitalCount(Expression<Func<Hospital, bool>> search)
        {
            return basicDbContext.Hospital.CountAsync(search);
        }
    }
}
