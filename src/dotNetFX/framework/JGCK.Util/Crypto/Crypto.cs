//
// Crypto.cs
//
// �ӡ����ܷ�����
// �ṩ��̬���ַ����������
//
//

using System;
using System.Security.Cryptography;
using System.Text;

namespace JGCK.Util.Crypto
{
    /// <summary>
    /// Crypto ��ժҪ˵����
    /// </summary>
    public sealed class Crypto
    {
        // 0-9,a-z,A-Z �ַ�16���� ASCII
        static readonly byte[] chars = new byte[]{
													 0x30,0x31,0x32,0x33,0x34,0x35,0x36,0x37,0x38,0x39,
													 0x41,0x42,0x43,0x44,0x45,0x46,0x47,0x48,0x49,0x4A,
													 0x4B,0x4C,0x4D,0x4E,0x4F,0x50,0x51,0x52,0x53,0x54,
													 0x55,0x56,0x57,0x58,0x59,0x5A,0x61,0x62,0x63,0x64,
													 0x65,0x66,0x67,0x68,0x69,0x6A,0x6B,0x6C,0x6D,0x6E,
													 0x6F,0x70,0x71,0x72,0x73,0x74,0x75,0x76,0x77,0x78,
													 0x79,0x7A
												 };

        // 0-9,a-z �ַ�16���� ASCII
        static readonly byte[] chars2 = new byte[]{
													  0x30,0x31,0x32,0x33,0x34,0x35,0x36,0x37,0x38,0x39,
													  0x61,0x62,0x63,0x64,0x65,0x66,0x67,0x68,0x69,0x6A,
													  0x6B,0x6C,0x6D,0x6E,0x6F,0x70,0x71,0x72,0x73,0x74,
													  0x75,0x76,0x77,0x78,0x79,0x7A
												  };

        // a-z,A-Z �ַ�16���� ASCII
        static readonly byte[] chars3 = new byte[]{
													 0x41,0x42,0x43,0x44,0x45,0x46,0x47,0x48,0x49,0x4A,
													 0x4B,0x4C,0x4D,0x4E,0x4F,0x50,0x51,0x52,0x53,0x54,
													 0x55,0x56,0x57,0x58,0x59,0x5A,0x61,0x62,0x63,0x64,
													 0x65,0x66,0x67,0x68,0x69,0x6A,0x6B,0x6C,0x6D,0x6E,
													 0x6F,0x70,0x71,0x72,0x73,0x74,0x75,0x76,0x77,0x78,
													 0x79,0x7A
												 };

        /// <summary>
        /// MD5�㷨ɢ������
        /// </summary>
        /// <param name="Text"> ��ɢ���ı� </param>
        /// <returns> ����32λ16�����ַ�(��д) </returns>
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

            return sre.ToString();	//����32λ16��������
        }

        public static string ToMD5Hash(string text)
        {
            return ToMD5Hash(text, "GB2312");
        }

        /// <summary>
        /// byte ����ת���ɿ���ʾ��16���Ʊ�ʾ���ַ���
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
        /// BytesToHexString ������ת��
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
        /// ��������ַ����У���������ĸ���������У�
        /// </summary>
        /// <param name="length"> ����ַ����� </param>
        /// <returns> ����ַ��� </returns>
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
        /// ��������ַ����У�������Сд��ĸ���������У�
        /// </summary>
        /// <param name="length"> ����ַ����� </param>
        /// <returns> ����ַ��� </returns>
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
        /// ��������ַ����У���������Сд��ĸ��
        /// </summary>
        /// <param name="length"> ����ַ����� </param>
        /// <returns> ����ַ��� </returns>
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
        /// �����ַ�����ת�����Ϊ Base64 �����ַ�
        /// �����ܳװ����ڼ��ܽ����
        /// 
        /// ���ܽ����̳���Ϊ 32
        /// Ȼ���� 12 ���� 8 Ϊ��������Ϊ 44��56��64��76 ...
        /// 
        /// ԭ�볤��������ܽ������
        /// 0 - 7   : 32 λ
        /// 8 - 15  : 44 λ = 32 + 12
        /// 16 - 23 : 56 λ = 44 + 12
        /// 24 - 31 : 64 λ = 56 + 8
        /// 32 - 39 : 76 λ = 64 + 8
        /// 40 - 47 : 88 λ = 76 + 12
        /// 48 - 55 : 96 λ = 88 + 8
        /// 56 - 63 : 108 λ = 96 + 12
        /// 64 - 71 : 120 λ = 108 + 12
        /// 72 - 89 : 128 λ = 120 + 8
        /// ......
        /// 
        /// ����ԭ�볤��û���� 8 �����볤����һ��������������Ϊ 12 ���� 8
        /// MIME/BASE64 ���㷨�ܼ򵥣������ַ���˳�����һ�� 24 λ�Ļ�������ȱ
        /// �ַ��ĵط����㡣Ȼ�󽫻������ضϳ�Ϊ 4 �����֣���λ���ȣ�ÿ������ 6 λ��
        /// �������64���ַ����±�ʾ����ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/����
        /// �������ֻ��һ���������ֽڣ���ô������õȺš�=�����㡣����Ը��ϸ��ӵ���Ϣ��ɱ���Ļ��ҡ�
        /// �����BASE64��
        /// </summary>
        /// <param name="str"> �ַ��� </param>
        /// <returns> ���ܵ��ַ��� </returns>
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
        /// �����ַ�����ת�����Ϊ Base64 �����ַ���
        /// ���� htmlString ����ת������ַ��е� "+,=" �ַ��Ա�����ҳ���ϴ���
        /// </summary>
        /// <param name="str"> �ַ��� </param>
        /// <param name="htmlString"> �Ƿ��滻Ϊ������ҳ���ϴ��͵��ַ���[true]�滻 </param>
        /// <returns> ���ܵ��ַ��� </returns>
        public static string EncryptString(string str, bool htmlString)
        {
            StringBuilder temp = new StringBuilder(EncryptString(str));
            if (htmlString)
                return temp.Replace('+', '-').Replace('=', '$').ToString();
            else
                return temp.ToString();
        }

        /// <summary>
        /// �����ַ���
        /// </summary>
        /// <param name="str"> �ַ��� </param>
        /// <returns> ���ܵ��ַ��� </returns>
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
