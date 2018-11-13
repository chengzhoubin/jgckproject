using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JGCK.Framework.Web.Security
{
    /// <summary>
    /// Token帮助工具类
    /// </summary>
    public class TokenHelper
    {
        /// <summary>
        /// 获取用户Token
        /// </summary>
        /// <typeparam name="TUserToken"></typeparam>
        /// <param name="getSerializeString"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static TUserToken Deserialize<TUserToken>(Func<object, string> getSerializeString, object state)
            where TUserToken : AbstractToken<TUserToken>, new()
        {
            var serializeStr = getSerializeString(state);
            if (string.IsNullOrEmpty(serializeStr)) //保存本地的token为空
                return null;
            var userToken = JsonConvert.DeserializeObject<TUserToken>(serializeStr);
            if (userToken == null) //数据保存有误，无法反序列化成对象
                return null;
            if (userToken.MD5HashValue != userToken.Sign) //签名有问题，数据已经被篡改
                return null;
            return userToken;
        }

        /// <summary>
        /// 序列化用户Token
        /// </summary>
        /// <typeparam name="TUserToken"></typeparam>
        /// <param name="userToken"></param>
        /// <param name="onSerializedHandleAction"></param>
        public static void SerializeToken<TUserToken>(TUserToken userToken, Action<string> onSerializedHandleAction)
        {
            var str = JsonConvert.SerializeObject(userToken);
            onSerializedHandleAction(str);
        }
    }
}
