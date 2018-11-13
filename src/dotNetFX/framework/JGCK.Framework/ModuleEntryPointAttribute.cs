using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JGCK.Framework
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class ModuleEntryPointAttribute : Attribute
    {
        public string CategoryName
        {
            get;
            set;
        }
    }
}
