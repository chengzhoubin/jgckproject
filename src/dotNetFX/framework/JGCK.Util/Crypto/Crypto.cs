//
// Crypto.cs
//
// 加、解密服务类
// 提供静态的字符串密码操作
//
//

using System;
using System.Security.Cryptography;
using System.Text;

namespace JGCK.Util.Crypto
{
    /// <summary>
    /// Crypto 的摘要说明。
    /// </summary>
    public sealed class Crypto
    {
        // 0-9,a-z,A-Z 字符16进制 ASCII
        static readonly byte[] chars = new byte[]{
													 0x30,0x31,0x32,0x33,0x34,0x35,0x36,0x37,0x38,0x39,
													 0x41,0x42,0x43,0x44,0x45,0x46,0x47,0x48,0x49,0x4A,
													 0x4B,0x4C,0x4D,0x4E,0x4F,0x50,0x51,0x52,0x53,0x54,
													 0x55,0x56,0x57,0x58,0x59,0x5A,0x61,0x62,0x63,0x64,
													 0x65,0x66,0x67,0x68,0x69,0x6A,0x6B,0x6C,0x6D,0x6E,
													 0x6F,0x70,0x71,0x72,0x73,0x74,0x75,0x76,0x77,0x78,
													 0x79,0x7A
												 };

        // 0-9,a-z 字符16进制 ASCII
        static readonly byte[] chars2 = new byte[]{
													  0x30,0x31,0x32,0x33,0x34,0x35,0x36,0x37,0x38,0x39,
													  0x61,0x62,0x63,0x64,0x65,0x66,0x67,0x68,0x69,0x6A,
													  0x6B,0x6C,0x6D,0x6E,0x6F,0x70,0x71,0x72,0x73,0x74,
													  0x75,0x76,0x77,0x78,0x79,0x7A
												  };

        // a-z,A-Z 字符16进制 ASCII
        static readonly byte[] chars3 = new byte[]{
													 0x41,0x42,0x43,0x44,0x45,0x46,0x47,0x48,0x49,0x4A,
													 0x4B,0x4C,0x4D,0x4E,0x4F,0x50,0x51,0x52,0x53,0x54,
													 0x55,0x56,0x57,0x58,0x59,0x5A,0x61,0x62,0x63,0x64,
													 0x65,0x66,0x67,0x68,0x69,0x6A,0x6B,0x6C,0x6D,0x6E,
													 0x6F,0x70,0x71,0x72,0x73,0x74,0x75,0x76,0x77,0x78,
													 0x79,0x7A
												 };

        /// <summary>
        /// MD5算法散列数据
        /// </summary>
        /// <param name="Text"> 需散列文本 </param>
        /// <returns> 定长32位16进制字符(大写) </returns>
        public static string ToMD5Hash(string text, string encode)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] md5re = md5.ComputeHash(Encoding.GetEncoding(encode).GetBytes(text));

            StringBuilder sre = new StringBuilder(32);
            for (int i = 0; i < md5re.Length; i++)
            {
                if (md5re[i] < 16) sre.Append("0");
                sre.Append(md5re[i].ToString("x"));
            }
            md5.Clear();
            md5 = null;

            return sre.ToString();	//定长32位16进制数据
        }

        public static string ToMD5Hash(string text)
        {
            return ToMD5Hash(text, "GB2312");
        }

        /// <summary>
        /// byte 数组转换成可显示的16进制表示的字符串
        /// </summary>
        public static string BytesToHexString(byte[] data)
        {
            StringBuilder temp = new StringBuilder(data.Length);
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] < 16) temp.Append("0");
                temp.Append(data[i].ToString("x"));
            }
            return temp.ToString();
        }

        /// <summary>
        /// BytesToHexString 的逆向转换
        /// </summary>
        public static byte[] HexStringToBytes(string data)
        {
            byte[] temp = new Byte[data.Length / 2];
            int j = 0;
            for (int i = 0; i < data.Length; i += 2)
            {
                temp[j++] = Convert.ToByte("0x" + data[i] + data[i + 1], 16);
            }
            return temp;
        }

        /// <summary>
        /// 产生随机字符序列（仅包含字母和数字序列）
        /// </summary>
        /// <param name="length"> 随机字符长度 </param>
        /// <returns> 随机字符串 </returns>
        public static string Rnd(int length)
        {
            StringBuilder sb = new StringBuilder();
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] data = new byte[length];
            rng.GetNonZeroBytes(data);
            rng = null;

            for (int i = 0; i < data.Length; i++)
            {
                data[i] = chars[data[i] %= 62];
            }
            return Encoding.ASCII.GetString(data);
        }

        /// <summary>
        /// 产生随机字符序列（仅包含小写字母和数字序列）
        /// </summary>
        /// <param name="length"> 随机字符长度 </param>
        /// <returns> 随机字符串 </returns>
        public static string RndLowerCase(int length)
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] data = new byte[length];
            rng.GetNonZeroBytes(data);
            rng = null;

            for (int i = 0; i < data.Length; i++)
            {
                data[i] = chars2[data[i] %= 36];
            }

            return Encoding.ASCII.GetString(data);
        }

        /// <summary>
        /// 产生随机字符序列（仅包含大小写字母）
        /// </summary>
        /// <param name="length"> 随机字符长度 </param>
        /// <returns> 随机字符串 </returns>
        public static string RndCase(int length)
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] data = new byte[length];
            rng.GetNonZeroBytes(data);
            rng = null;

            for (int i = 0; i < data.Length; i++)
            {
                data[i] = chars3[data[i] %= 52];
            }

            return Encoding.ASCII.GetString(data);
        }

        /// <summary>
        /// 加密字符串，转换结果为 Base64 编码字符
        /// 加密密匙包含在加密结果中
        /// 
        /// 加密结果最短长度为 32
        /// 然后以 12 或者 8 为基数增加为 44，56，64，76 ...
        /// 
        /// 原码长度在与加密结果长度
        /// 0 - 7   : 32 位
        /// 8 - 15  : 44 位 = 32 + 12
        /// 16 - 23 : 56 位 = 44 + 12
        /// 24 - 31 : 64 位 = 56 + 8
        /// 32 - 39 : 76 位 = 64 + 8
        /// 40 - 47 : 88 位 = 76 + 12
        /// 48 - 55 : 96 位 = 88 + 8
        /// 56 - 63 : 108 位 = 96 + 12
        /// 64 - 71 : 120 位 = 108 + 12
        /// 72 - 89 : 128 位 = 120 + 8
        /// ......
        /// 
        /// 即：原码长度没超过 8 ，密码长度升一个数量级，基数为 12 或者 8
        /// MIME/BASE64 的算法很简单，它将字符流顺序放入一个 24 位的缓冲区，缺
        /// 字符的地方补零。然后将缓冲区截断成为 4 个部分，高位在先，每个部分 6 位，
        /// 用下面的64个字符重新表示：“ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/”。
        /// 如果输入只有一个或两个字节，那么输出将用等号“=”补足。这可以隔断附加的信息造成编码的混乱。
        /// 这就是BASE64。
        /// </summary>
        /// <param name="str"> 字符串 </param>
        /// <returns> 加密的字符串 </returns>
        public static string EncryptString(string str)
        {
            DESCrypto des = new DESCrypto();
            byte[] key = des.desKey;
            byte[] iv = des.desIV;
            byte[] en = des.DESEncrypt(Encoding.ASCII.GetBytes(str));
            byte[] result = new byte[en.Length + 16];

            result[0] = en[0];
            if (result[0] % 2 == 0)
            {
                for (int i = 0; i < 8; i++) result[i + 1] = key[i];
                for (int i = 0; i < 8; i++) result[i + 9] = iv[i];
                for (int i = 1; i < en.Length; i++) result[i + 16] = en[i];
            }
            else
            {
                for (int i = 1; i < en.Length; i++) result[i] = en[i];
                for (int i = 0; i < 8; i++) result[i + en.Length] = key[i];
                for (int i = 0; i < 8; i++) result[i + en.Length + 8] = iv[i];
            }

            return Convert.ToBase64String(result);
        }

        /// <summary>
        /// 加密字符串，转换结果为 Base64 编码字符。
        /// 根据 htmlString 参数转换结果字符中的 "+,=" 字符以便于在页面上传送
        /// </summary>
        /// <param name="str"> 字符串 </param>
        /// <param name="htmlString"> 是否替换为可以在页面上传送的字符，[true]替换 </param>
        /// <returns> 加密的字符串 </returns>
        public static string EncryptString(string str, bool htmlString)
        {
            StringBuilder temp = new StringBuilder(EncryptString(str));
            if (htmlString)
                return temp.Replace('+', '-').Replace('=', '$').ToString();
            else
                return temp.ToString();
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="str"> 字符串 </param>
        /// <returns> 解密的字符串 </returns>
        public static string DecryptString(string str)
        {
            byte[] data = Convert.FromBase64String(str);
            byte[] key = new byte[8];
            byte[] iv = new byte[8];
            byte[] en = new byte[data.Length - 16];
            en[0] = data[0];
            if (en[0] % 2 == 0)
            {
                for (int i = 0; i < 8; i++) key[i] = data[i + 1];
                for (int i = 0; i < 8; i++) iv[i] = data[i + 9];
                for (int i = 1; i < en.Length; i++) en[i] = data[i + 16];
            }
            else
            {
                for (int i = 1; i < en.Length; i++) en[i] = data[i];
                for (int i = 0; i < 8; i++) key[i] = data[i + en.Length];
                for (int i = 0; i < 8; i++) iv[i] = data[i + en.Length + 8];
            }

            DESCrypto des = new DESCrypto(key, iv);
            return Encoding.ASCII.GetString(des.DESDecrypt(en)).TrimEnd(new char[] { '\0' });
        }

        public static string DecryptString(string str, bool htmlString)
        {
            if (htmlString)
            {
                StringBuilder temp = new StringBuilder(str);
                return DecryptString(temp.Replace('-', '+').Replace('$', '=').ToString());
            }
            else
                return DecryptString(str);
        }
    }
}
