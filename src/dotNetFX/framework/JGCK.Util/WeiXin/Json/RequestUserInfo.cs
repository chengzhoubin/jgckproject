using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSMY.Util.WeiXin.Json
{
    public class RequestUserInfo : AbstractGetRequest<JsonGetWxUserInfo>,
        ITakeNext<NextExec<JsonGetWxUserInfo>, RequestUserInfo>
    {
        public string access_token { get; set; }

        public string openid { get; set; }

        public string lang { get; set; } = "zh-CN";

        [JsonIgnore]
        public Action<JsonGetWxUserInfo> GetWxUserInfoDoneHandle { set; private get; }

        public RequestUserInfo(string token, string _openid)
        {
            access_token = token;
            openid = _openid;
        }

        public NextExec<JsonGetWxUserInfo> WillDo(RequestUserInfo req)
        {
            var wxapi = string.Format(
                ConfigurationManager.AppSettings[WxDevConst.GetWxUserMsg],
                this.access_token, this.openid, this.lang);
            var ret = this.GetResult(wxapi);
            if (ret != null &&
                ret.IsFinished &&
                !ret.HasError &&
                GetWxUserInfoDoneHandle != null)
            {
                GetWxUserInfoDoneHandle(ret.NextObject);
            }
            return ret;
        }
    }
}
