using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;

namespace JGCK.Web.General.HttpModules
{
    public class PreApplicationStartCode
    {
        public static void PreStart()
        {
            DynamicModuleUtility.RegisterModule(typeof(DynamicBindServiceModules));
        }
    }
}
