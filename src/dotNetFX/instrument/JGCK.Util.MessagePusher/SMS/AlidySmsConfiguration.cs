using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JGCK.Framework;

namespace JGCK.Util.MessagePusher.SMS
{
    /// <summary>
    /// 阿里大鱼短信接口
    /// </summary>
    internal class AlidySmsConfiguration : AbstractDefaultConfiguration<AlidySmsConfiguration>
    {
        public string SmsWebgateUrl => this.GetValue("alidayuSmswebgateUrl");

        public string CxAppKey => this.GetValue("alidayuCxAppKey");

        public string CxSecretKey => this.GetValue("alidayuCxSecretKey");

        public string SmsTemplate => this.GetValue("alidayuSmsTemplate");

        public string FreeSignName => this.GetValue("alidayuFreeSignName");
    }
}
