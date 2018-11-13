using DY.Top.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DY.Top.Api.Request;
using DY.Top.Api.Response;
using Newtonsoft.Json;

namespace JGCK.Util.MessagePusher.SMS
{
    public class AlidySmsSender
    {
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <typeparam name="TSendContent"></typeparam>
        /// <param name="phone"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string toSend<TSendContent>(string phone, TSendContent content)
        {
            ITopClient topClient = new DefaultTopClient(
                AlidySmsConfiguration.Instance.SmsWebgateUrl,
                AlidySmsConfiguration.Instance.CxAppKey,
                AlidySmsConfiguration.Instance.CxSecretKey);

            AlibabaAliqinFcSmsNumSendResponse alibabaAliqinFcSmsNumSendResponse =
                topClient.Execute<AlibabaAliqinFcSmsNumSendResponse>(new AlibabaAliqinFcSmsNumSendRequest
                {
                    Extend = "",
                    SmsType = "normal",
                    SmsFreeSignName = AlidySmsConfiguration.Instance.FreeSignName,
                    SmsParam = JsonConvert.SerializeObject(content),
                    RecNum = phone,
                    SmsTemplateCode = AlidySmsConfiguration.Instance.SmsTemplate
                });
            return alibabaAliqinFcSmsNumSendResponse.Body;
        }
    }
}
