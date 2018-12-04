using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JGCK.Framework
{
    public interface IAppService : IDisposable
    {
        /// <summary>
        /// 是否满足逻辑删除条件
        /// </summary>
        Func<object,bool> PreLogicDeleteHandler { get; set; }
    }
}
