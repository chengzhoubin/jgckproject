using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JGCK.Framework;

namespace JGCK.Util.WenXin
{
    public class WxConfiguration : AbstractDefaultConfiguration<WxConfiguration>
    {
        public string WxUserInfoUrlTemplateString => this.GetValue("getWxUserInfoUrl");

        public string WxQrCodeUrlTemplateString => this.GetValue("getWxQrCodeUrl");

        public string WxOpenIdUrlTemplateString => this.GetValue("getWxOpenIdUrl");

        public string WxUserMsgUrlTemplateString => this.GetValue("getWxUserMsgUrl");

        public string AppId => this.GetValue("wx_AppID");

        public string RedirectUri => this.GetValue("wx_RedirectUri");

        public string AppSecret => this.GetValue("wx_AppSecret");

        public string Scope => this.GetValue("wx_Scope");

        public string NoauthenUrl => this.GetValue("wx_Noauthen_Url");

        public string NoauthenUrlRegex => this.GetValue("wx_Noauthen_Url_Regex");

        public string AuthenWenXinRegex => this.GetValue("wx_Authen_Weixin_Regex");

        public string TokenDomain => this.GetValue("wxTokenDomain");

        public string JumpBaseUrlHost => this.GetValue("wx_RedirectUri_Host");
    }
}
