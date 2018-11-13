using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JGCK.Framework.Web.Security
{
    [AttributeUsage(AttributeTargets.Property)]
    public class HashParameterAttribute : Attribute
    {
        /// <summary>
        /// 是否进入Hash
        /// </summary>
        public bool IntoHashList { get; set; }
    }
}
