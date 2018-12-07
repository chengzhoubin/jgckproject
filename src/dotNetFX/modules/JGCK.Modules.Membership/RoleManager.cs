using JGCK.Respority.UserWork;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JGCK.Modules.Membership
{
    public class RoleManager : AbstractMembershipService
    {
        public Role GetRole(string rName)
        {
            return userDbContext.Role.FirstOrDefault(r => r.Name == rName);
        }

        public Task<List<Role>> GetRoles()
        {
            return userDbContext.Role.OrderByDescending(m=>m.ID).ToListAsync();
        }
    }
}
