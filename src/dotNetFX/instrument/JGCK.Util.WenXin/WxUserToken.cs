using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using JGCK.Util.WenXin;
using JGCK.Framework.Web.Security;
using HSMY.Util.WeiXin.Json;
//using HSMY.Util.WenXin;
//using HSMY.Web.Security.Token;

namespace HSMY.Util.WeiXin
{
    /// <summary>
    /// 微信用户Token
    /// </summary>
    public class WxUserToken : JsonGetWxUserInfo<WxUserToken>
    {
        [HashParameter(IntoHashList = true)]
        public long userId { get; set; }

        /// <summary>
        /// 获取用户Token
        /// </summary>
        /// <returns></returns>
        public static WxUserToken GetToken()
        {
            var request = HttpContext.Current.Request;
            return TokenHelper.Deserialize<WxUserToken>(
                key => HttpUtility.UrlDecode(request.Cookies[WxDevConst.WenXin_TOKEN_LocalName]?.Value ?? ""),
                WxDevConst.WenXin_TOKEN_LocalName);
        }

        /// <summary>
        /// 设置Token
        /// </summary>
        /// <param name="token"></param>
        public static string SetToken(WxUserToken token)
        {
            var encodingStr = "";
            TokenHelper.SerializeToken<WxUserToken>(token, tokenStr =>
            {
                var response = HttpContext.Current.Response;
                var tokenCookie = new HttpCookie(WxDevConst.WenXin_TOKEN_LocalName)
                {
                    Value = HttpUtility.UrlEncode(tokenStr),
                    //Expires = DateTime.Now.AddHours(WxDevConst.WenXin_LocalToken_ExpiredTime),
                    Domain = WxConfiguration.Instance.TokenDomain
                };
                encodingStr = tokenCookie.Value;
                response.Cookies.Add(tokenCookie);
            });
            return encodingStr;
        }

        /// <summary>
        /// 设置Token
        /// </summary>
        /// <param name="tokenStr"></param>
        public static void SetToken(string tokenStr, bool isEncoding = true)
        {
            var tokenCookie = new HttpCookie(WxDevConst.WenXin_TOKEN_LocalName)
            {
                Value = isEncoding ? tokenStr : HttpUtility.UrlEncode(tokenStr),
                //Expires = DateTime.Now.AddHours(WxDevConst.WenXin_LocalToken_ExpiredTime),
                Domain = WxConfiguration.Instance.TokenDomain
            };
            HttpContext.Current.Response.Cookies.Add(tokenCookie);
        }

        /// <summary>
        /// 删除Token
        /// </summary>
        public static void RemoveToken()
        {
            var request = HttpContext.Current.Request;
            var response = HttpContext.Current.Response;
            var token = request.Cookies[WxDevConst.WenXin_TOKEN_LocalName];
            if (token == null)
                return;
            token.Expires = DateTime.Now.AddSeconds(-1);
            response.Cookies.Add(token);
        }
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        public static explicit operator WxUserToken(JsonGetWxUserInfo<object> json)
        {
            if (json == null)
                throw new ArgumentNullException(nameof(json));
            var ret = new WxUserToken
            {
                city = json.city,
                country = json.country,
                headimgurl = json.headimgurl,
                nickname = json.nickname,
                openid = json.openid,
                priviege = json.priviege,
                province = json.province,
                sex = json.sex,
                unionid = json.unionid
            };
            return ret;
        }
    }
}
