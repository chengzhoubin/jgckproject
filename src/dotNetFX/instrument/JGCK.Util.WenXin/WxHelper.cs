using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace JGCK.Util.WenXin
{
    public static class WxHelper
    {
        public static bool IsMicroMessagerBrowser
        {
            get
            {
                var userAgent = HttpContext.Current.Request.UserAgent;
                return !string.IsNullOrEmpty(userAgent) && userAgent.Contains("MicroMessenger");
            }

        }
    }
}
