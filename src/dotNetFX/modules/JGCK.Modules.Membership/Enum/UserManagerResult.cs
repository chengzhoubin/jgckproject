using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JGCK.Modules.Membership
{
    /// <summary>
    /// 检查用户名密码
    /// </summary>
    public enum CheckUserPwdResult
    {
        /// <summary>
        /// 用户不存在
        /// </summary>
        User_Not_Exist,
        /// <summary>
        /// 用户名密码不匹配
        /// </summary>
        User_Pwd_Not_Match,
        /// <summary>
        /// 成功
        /// </summary>
        Success
    }
}
