using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JGCK.Web.General.MVC
{
    public interface IObjectMap<TFrom, TTo>
    {
        /// <summary>
        /// 映射
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        TTo Map(TTo to);
    }
}
