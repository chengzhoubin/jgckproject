using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSMY.Util.WeiXin.Json;

namespace HSMY.Util.WeiXin
{
    /// <summary>
    /// 微信用户Token
    /// </summary>
    public class WxUserToken : JsonGetWxUserInfo
    {
        [HashParameter]
        public long userId { get; set; }
    }
}
