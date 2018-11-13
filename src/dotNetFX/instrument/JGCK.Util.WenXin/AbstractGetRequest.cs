using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using JGCK.Framework;
using JGCK.Util.WenXin;
using Newtonsoft.Json;

namespace HSMY.Util.WeiXin.Json
{
    public abstract class AbstractGetRequest<TResult>
    {
        protected WxConfiguration ConfigSection { get; private set; }

        protected internal AbstractGetRequest()
        {
            ConfigSection = AbstractDefaultConfiguration<WxConfiguration>.Instance;
        }

        protected NextExec<TResult> GetResult(string wxapi)
        {
            var ret = new NextExec<TResult>();
            using (var client = new HttpClient())
            {
                var resp = client.GetStringAsync(wxapi).Result;
                ret.IsFinished = true;
                if (string.IsNullOrEmpty(resp))
                {
                    ret.HasError = true;
                    ret.ExecError = "Response content is null";
                    return ret;
                }
                var objtoken = JsonConvert.DeserializeObject<TResult>(resp);
                if (objtoken == null)
                {
                    ret.HasError = true;
                    ret.ExecError = "Deserialize object is null";
                    return ret;
                }
                ret.IsFinished = true;
                ret.CanExecNextStep = true;
                ret.NextObject = objtoken;
                return ret;
            }
        }
    }
}
