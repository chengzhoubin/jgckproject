using JGCK.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JGCK.Respority.BasicInfo;
using JGCK.Respority.UserWork;

namespace JGCK.Modules.Membership
{
    public abstract class AbstractMembershipService : AbstractDefaultAppService, ITransistService
    {
        protected BasicDbProxy basicDbContext { get; set; }

        protected UserDbProxy userDbContext { get; set; }
    }
}
