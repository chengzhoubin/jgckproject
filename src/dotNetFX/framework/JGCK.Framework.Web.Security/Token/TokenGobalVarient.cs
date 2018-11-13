using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Configuration;

namespace JGCK.Framework.Web.Security
{
    /// <summary>
    /// 全局变量
    /// </summary>
    public class TokenVarient
    {
        /// <summary>
        /// 加密字符串
        /// </summary>
        public const String UtmEncryptCode = "HSMY_%()";

        /// <summary>
        /// 反射Utm属性
        /// </summary>

        public static readonly ConcurrentDictionary<String, PropertyInfo[]> RefUtmHashKeyProperies;


        static TokenVarient()
        {
            RefUtmHashKeyProperies = new ConcurrentDictionary<string, PropertyInfo[]>();
        }
    }
}
