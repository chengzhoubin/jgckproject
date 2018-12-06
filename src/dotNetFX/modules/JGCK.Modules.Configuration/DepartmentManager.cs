using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using JGCK.Framework.EF;
using JGCK.Respority.BasicInfo;
using JGCK.Util.Enums;

namespace JGCK.Modules.Configuration
{
    public class DepartmentManager : AbstractConfigurationService
    {
        public Task<List<Department>> GetDepartments()
        {
            Expression<Func<Department, bool>> exp = dep => !dep.IsDeleted;
            var orderExps = new AbstractUnitOfWork.OrderByExpression<Department>[]
            {
                new AbstractUnitOfWork.OrderByExpression<Department>
                {
                    OrderByExpressionMember = "ID",
                    SortBy = AscOrDesc.Desc
                }
            };
            return basicDbContext.GetObjectsAsync(exp, orderByExpressions: orderExps);
        }

        public bool DepartmentExists(string name)
        {
            return basicDbContext.Department.Any(dep => dep.Name == name && !dep.IsDeleted);
        }

        public Department GetDepartment(string name)
        {
            return basicDbContext.Department.FirstOrDefault(dep => dep.Name == name && !dep.IsDeleted);
        }

        public Department GetDepartment(long depId)
        {
            return basicDbContext.GetById<Department, long>(depId);
        }
    }
}
