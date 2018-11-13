using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JGCK.Framework
{
    /// <summary>
    /// 默认策略
    /// </summary>
    public abstract class AbstractDefaultPolicy<TPolicyRule>
    {
        /// <summary>
        /// 策略名称
        /// </summary>
        public string PolicyName { get; set; }

        /// <summary>
        /// 策略Rule
        /// </summary>
        public TPolicyRule Rules { get; set; }
    }
}
