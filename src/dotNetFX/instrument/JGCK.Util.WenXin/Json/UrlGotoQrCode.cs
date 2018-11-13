using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HSMY.Util.WeiXin.Json
{
    public class UrlGotoQrCode : AbstractGetRequest<UrlGotoQrCode>
    {
        /// <summary>
        /// 应用唯一标识 
        /// </summary>
        public string appid { get; set; }

        /// <summary>
        /// 请使用urlEncode对链接进行处理
        /// </summary>
        public string redirect_uri { get; set; }

        /// <summary>
        /// 填code
        /// </summary>
        public string response_type { get; set; }

        /// <summary>
        /// 应用授权作用域，拥有多个作用域用逗号（,）分隔，网页应用目前仅填写snsapi_login即可
        /// </summary>
        public string scope { get; set; }

        /// <summary>
        /// 用于保持请求和回调的状态，授权请求后原样带回给第三方。
        /// 该参数可用于防止csrf攻击（跨站请求伪造攻击），建议第三方带上该参数，
        /// 可设置为简单的随机数加session进行校验
        /// </summary>
        public string state { get; set; }

        public UrlGotoQrCode()
        {
            this.appid = ConfigSection.AppId;
            this.redirect_uri = HttpUtility.UrlEncode(ConfigSection.RedirectUri);
            this.scope = ConfigSection.Scope;
        }

        public string GetQrCodeUrl()
        {
            return string.Format(
                ConfigSection.WxQrCodeUrlTemplateString,
                this.appid,
                this.redirect_uri,
                this.scope,
                this.state);
        }
    }
}
