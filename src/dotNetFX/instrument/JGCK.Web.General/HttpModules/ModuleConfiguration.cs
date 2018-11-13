using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Configuration;
using JGCK.Util.WenXin;

namespace JGCK.Web.General.HttpModules
{
    /// <summary>
    /// 自定义模块配置
    /// </summary>
    public static class ModuleConfiguration
    {
        public static WxConfiguration ConfigSection => WxConfiguration.Instance;

        public static string WXNotAuthenUrlFormat => ConfigSection.NoauthenUrl;

        public static Regex RegexWXNotAuthenUrl { get; private set; }

        public static Regex RegexWxAuthenUrl { get; private set; }

        static ModuleConfiguration()
        {
            RegexWXNotAuthenUrl = new Regex(ConfigSection.NoauthenUrlRegex, RegexOptions.IgnoreCase);
            RegexWxAuthenUrl = new Regex(ConfigSection.AuthenWenXinRegex, RegexOptions.IgnoreCase);
        }
    }
}
