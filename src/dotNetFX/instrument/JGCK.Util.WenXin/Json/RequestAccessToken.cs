using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using JGCK.Framework;
using JGCK.Util.WenXin;

namespace HSMY.Util.WeiXin.Json
{
    public class RequestAccessToken : AbstractGetRequest<JsonGetAccessToken>,
        ITakeNext<NextExec<JsonGetAccessToken>, RequestAccessToken>
    {
        /// <summary>
        /// 应用唯一标识，在微信开放平台提交应用审核通过后获得
        /// </summary>
        public string appid { get; set; }

        /// <summary>
        /// 应用密钥AppSecret，在微信开放平台提交应用审核通过后获得
        /// </summary>
        public string secret { get; set; }

        /// <summary>
        /// 填authorization_code
        /// </summary>
        public string grant_type { get; set; } = "authorization_code";

        /// <summary>
        /// 填写第一步获取的code参数
        /// </summary>
        public string code { get; set; }

        public RequestAccessToken(string code)
        {
            this.appid = ConfigSection.AppId;
            this.secret = ConfigSection.AppSecret;
            this.code = code;
        }

        public NextExec<JsonGetAccessToken> WillDo(RequestAccessToken token)
        {
            var wxapi = string.Format(
                ConfigSection.WxOpenIdUrlTemplateString,
                this.appid, this.secret, this.code, this.grant_type);
            return this.GetResult(wxapi);
        }
    }
}
