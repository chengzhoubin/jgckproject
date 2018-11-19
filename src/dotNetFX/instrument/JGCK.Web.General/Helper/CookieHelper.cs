using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using Rsft.HttpCookieSecure;
using Newtonsoft.Json;

namespace JGCK.Web.General
{
    /// <summary>
    /// cookie操作类
    /// </summary>
    public static class CookieHelper
    {
        public static void CreateCookie<T>(
            T cookieObject,
            string key,
            DateTime? expiredTime = null,
            Func<T, string> resolveObjectToStr = null)
        {
            var newCookie = new HttpCookie(key);
            string cookieValue = null;
            cookieValue = cookieObject is string
                ? Convert.ToString(cookieObject)
                : resolveObjectToStr?.Invoke(cookieObject);
            if (string.IsNullOrEmpty(cookieValue))
            {
                throw new NullReferenceException("set cookie value is null");
            }

            newCookie.Value = cookieValue;
            if (expiredTime.HasValue)
                newCookie.Expires = expiredTime.Value;
            newCookie = CookieSecure.Encode(newCookie);
            HttpContext.Current?.Response.Cookies.Add(newCookie);
        }

        /// <summary>
        /// 将对象先转换成Json String
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cookieObject"></param>
        /// <param name="key"></param>
        /// <param name="expiredTime"></param>
        public static void CreateCookieJsonValue<T>(T cookieObject,
            string key,
            DateTime? expiredTime = null)
        {
            CreateCookie(cookieObject, key, expiredTime, (obj) => JsonConvert.SerializeObject(obj));
        }

        /// <summary>
        /// 获取Cookie值，并且反序列化为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetValue<T>(string key)
        {
            var existCookie = HttpContext.Current?.Request.Cookies[key];
            if (existCookie == null || string.IsNullOrEmpty(existCookie.Value))
                throw new NullReferenceException("can't get cookie");

            existCookie = CookieSecure.Decode(existCookie);
            if (typeof(T) == typeof(string))
                return (T) (object) existCookie.Value;
            return JsonConvert.DeserializeObject<T>(existCookie.Value);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key"></param>
        public static void RemoveIt(string key)
        {
            var existCookie = HttpContext.Current?.Request.Cookies[key];
            if (existCookie == null)
                return;
            existCookie.Expires = DateTime.Now.AddSeconds(-1);
            HttpContext.Current?.Response.Cookies.Add(existCookie);
        }
    }
}
