using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSMY.Util.WeiXin
{
    /// <summary>
    /// 微信开发配置常量
    /// </summary>
    public class WxDevConst
    {
        public const string GetWxUserInfo =
            "https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang=zh_CN";

        public const string GetWxQrCode =
                "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope={2}&state={3}#wechat_redirect"
            ;

        public const string GetWxOpenId =
                "https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code"
            ;

        public const string GetWxUserMsg =
            "https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN";

        public const string GetWxMenu = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}";

        public const string GetWxTemplate = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}";

        /// <summary>
        /// <!--微信-->
        //  <add key = "wx_AppID" value=""/>
        //  <add key = "wx_RedirectUri" value=""/>
        //  <add key = "wx_AppSecret" value=""/>
        /// </summary>
        internal const string Config_AppId_Key = "wx_AppID";
        internal const string Config_wx_RedirectUri_Key = "wx_RedirectUri";
        internal const string Config_wx_AppSecret_Key = "wx_AppSecret";
        internal const string Config_wx_Scope_Key = "wx_Scope";
        /// <summary>
        /// 微信Token本地保存名称
        /// </summary>
        internal const string WenXin_TOKEN_LocalName = "hsmy_wx_token";
    }
}
