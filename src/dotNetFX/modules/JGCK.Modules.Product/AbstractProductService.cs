using JGCK.Framework;
using JGCK.Respority.BasicInfo;
using JGCK.Respority.ProductWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JGCK.Modules.ProductModule
{
    public abstract class AbstractProductService : AbstractDefaultAppService, ITransistService
    {
        protected BasicDbProxy basicDbContext { get; set; }

        protected ProductDbProxy productDbContext { get; set; }
    }
}
