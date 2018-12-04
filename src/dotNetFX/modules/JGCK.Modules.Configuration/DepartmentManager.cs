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
        public IEnumerable<Department> GetDepartments()
        {
            Expression<Func<Department, bool>> exp = dep => !dep.IsDeleted;
            return basicDbContext.GetObjects(exp);
        }
    }
}
