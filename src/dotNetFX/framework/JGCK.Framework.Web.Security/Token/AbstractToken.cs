using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using JGCK.Util.Crypto;
using Newtonsoft.Json;

namespace JGCK.Framework.Web.Security
{
    /// <summary>
    /// Utm基类
    /// </summary>
    public abstract class AbstractToken<TToken> where TToken : class, new()
    {
        [JsonIgnore]
        public Func<object, bool> IgnoreHandler { get; set; }

        [JsonIgnore]
        public Func<object, TToken> OnInitaledHandler { get; set; }

        [JsonIgnore]
        public Func<object, TToken> OnLoadedHandler { get; set; }

        /// <summary>
        /// 扩展字符串(可以是时间戳或者随机数)
        /// </summary>
        [HashParameter(IntoHashList = true)]
        public String Ext { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public String Sign { get; set; }

        /// <summary>
        /// MD5HashValue
        /// </summary>
        [JsonIgnore]
        public string MD5HashValue
        {
            get
            {
                var keyName = typeof(TToken).ToString();
                PropertyInfo[] tokenProperies = TokenVarient.RefUtmHashKeyProperies.GetOrAdd(keyName, key =>
                {
                    var allProperties = typeof(TToken).GetProperties();
                    return allProperties.Where(p =>
                    {
                        var toHashAttr =
                            Attribute.GetCustomAttribute(p, typeof(HashParameterAttribute)) as HashParameterAttribute;
                        return toHashAttr != null && toHashAttr.IntoHashList;
                    }).ToArray();
                });

                var hashValueStr = new StringBuilder();
                foreach (var p in tokenProperies)
                {
                    var v = p.GetValue(this);
                    hashValueStr.Append(v?.ToString() ?? "");
                }
                hashValueStr.Append(TokenVarient.UtmEncryptCode);
                return Crypto.ToMD5Hash(hashValueStr.ToString());
            }
        }
    }
}
