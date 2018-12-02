using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        public Task<List<Person>> GetDoctorListAsync(
            Expression<Func<Person, bool>> search,
            AbstractUnitOfWork.OrderByExpression<Person>[] orderBy,
            int pageIndex)
        {
            var pager = new AbstractUnitOfWork.Pager { CurrentIndex = pageIndex };
            return userDbContext.GetObjectsAsync(search, pager, false, orderBy, p => p.Doctor);
        }

        public Task<int> GetDoctorCount(Expression<Func<Person, bool>> search)
        {
            return userDbContext.Person.CountAsync(search);
        }

        public async Task<int> UpdateDoctorInfo(Person doctor)
        {
            var orignalDoctor = await userDbContext.GetByIdAsync<Person, long>(doctor.ID);
            if (orignalDoctor == null)
                throw new NullReferenceException("医生信息为空");
            orignalDoctor.Doctor = doctor.Doctor;
            return await userDbContext.SaveChangesAsync();
        }
    }
}
