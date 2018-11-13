using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HSMY.Util.WeiXin.Json
{
    /// <summary>
    /// 微信平台返回错误
    /// </summary>
    [JsonObject]
    public class JsonErrorResult
    {
        [JsonProperty("errcode")]
        public string ErrorCode { get; set; }

        [JsonProperty("errmsg")]
        public string Msg { get; set; }
    }
}
