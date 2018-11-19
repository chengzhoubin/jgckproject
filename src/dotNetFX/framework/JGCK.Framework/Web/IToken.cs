using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JGCK.Framework.Web
{
    /// <summary>
    /// Token
    /// </summary>
    public interface IToken<TTokenObject>
    {
        string TokenName { get; }

        /// <summary>
        /// 创建用户Token
        /// </summary>
        /// <param name="tokenObject"></param>
        void BuildToken();

        /// <summary>
        /// 解析用户Token
        /// </summary>
        /// <param name="tokenString"></param>
        /// <param name="resolveFunc"></param>
        /// <returns></returns>
        TTokenObject ResolveToken();
    }
}
