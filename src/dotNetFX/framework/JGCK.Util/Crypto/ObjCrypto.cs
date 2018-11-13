//
// ObjCrypto.cs
//
//对对象进行加密和解密 
//

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace JGCK.Util.Crypto
{
    /// <summary>
    /// 对象加密解密
    /// </summary>
    public static class ObjCrypto
    {
        /// <summary>
        /// 对象加密成Base64字符串
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static String Encrypt(Object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            //将对象序列化成字节流 
            bf.Serialize(ms, obj);
            Byte[] bytsEncrypt = ms.ToArray();
            ms.Close();

            return Convert.ToBase64String(bytsEncrypt);
        }

        /// <summary>
        /// 将Base64字符串转化为对象
        /// </summary>
        /// <param name="strBase64"></param>
        /// <param name="type">对象类型</param>
        /// <returns></returns>
        public static Object Decrypt(String strBase64 , Type type)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            //将字节数据写入内存流
            Byte[] bytsDecrypt = Convert.FromBase64String(strBase64);
            ms.Write(bytsDecrypt, 0, bytsDecrypt.Length);
            ms.Position = 0;
            //反序列化对象
            Object obj = bf.Deserialize(ms);
            ms.Close();

            return obj;
        }
    }
}
