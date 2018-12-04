using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using JGCK.Respority.BasicInfo;

namespace JGCK.Modules.Configuration
{
    public class DepartmentManager : AbstractConfigurationService
    {
        public Task<List<Department>> GetDepartments()
        {
            Expression<Func<Department, bool>> exp = dep => !dep.IsDeleted;
            return basicDbContext.GetObjectsAsync(exp);
        }

        public Task<int> DeleteDepartment(long depId)
        {
            return base.LogicObjectDelete<Department, long>(depId, true);
        }
    }
}
