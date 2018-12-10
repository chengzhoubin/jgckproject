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
using JGCK.Util;

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

        public bool HospitalExists(string hospitalName)
        {
            return basicDbContext.Hospital.Any(h => h.Name == hospitalName && !h.IsDeleted);
        }

        public Hospital GetHospital(long hid)
        {
            return basicDbContext.Hospital
                .Include(h => h.HospitalInvoices)
                .Include(h => h.HospitalReferences)
                .FirstOrDefault(h => h.ID == hid);
        }

        public Hospital GetHospital(string hName)
        {
            return basicDbContext.Hospital
                .Include(h => h.HospitalInvoices)
                .Include(h => h.HospitalReferences)
                .FirstOrDefault(h => h.Name == hName);
        }

        public Task<List<Hospital>> GetHospitals(string name, bool isFuzzyQuery = true)
        {
            var exp = PredicateBuilder.Create<Hospital>(h => !h.IsDeleted);
            if (!string.IsNullOrEmpty(name))
                exp = isFuzzyQuery ? exp.And(h => h.Name.Contains(name)) : exp.And(h => h.Name == name);
            return basicDbContext.Hospital.Where(exp).ToListAsync();
        }
    }
}
