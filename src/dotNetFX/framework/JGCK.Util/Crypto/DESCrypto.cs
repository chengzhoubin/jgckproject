//
// DESCrypto.cs
//
// DES 算法的加密、解密方法
// 包括对数据或者文件加、解密
//

using System.IO;
using System.Security.Cryptography;

namespace JGCK.Util.Crypto
{
	/// <summary>
	/// 使用 DES 算法加、解密方法
	/// </summary>
	public class DESCrypto
	{
		/// <summary> 密匙(8 bytes) </summary>
		private byte[] deskey;
		/// <summary> 向量(8 bytes) </summary>
		private byte[] desiv;
		
		/// <summary> DES 密匙(8 bytes) </summary>
		public byte[] desKey
		{
			get	{	return deskey; }
			set	{	deskey = value;	}
		}

		/// <summary> DES向量(8 bytes) </summary>
		public byte[] desIV
		{
			get	{	return desiv;	}
			set	{	desiv = value; }
		}

		/// <summary> 创建类的实例，并随机创建一对密匙和向量 </summary>
		public DESCrypto()
		{
			// 每次创建加密示例，创建一个密码和向量。
			DES des = new DESCryptoServiceProvider();
			des.GenerateKey();
			des.GenerateIV(); 
			deskey = des.Key;
			desiv = des.IV;
			des.Clear();
			des = null;
		}

		/// <summary>
		/// 创建类的实例并初始化密匙和向量
		/// </summary>
		/// <param name="desKey">密匙</param>
		/// <param name="desIV">向量</param>
		public DESCrypto(byte[] desKey,byte[] desIV)
		{
			deskey = desKey;
			desiv = desIV;
		}

		/// <summary> 加密信息并保存到文件中 </summary>
		/// <param name="Data"> 加密信息 </param>
		/// <param name="WriteFileName"> 文件名称 </param>
		public void DESEncrypt(byte[] Data,	string WriteFileName)
		{
			//加密数据到指定的文件中（文件包含路径）。

			FileStream fout = new FileStream(WriteFileName,FileMode.OpenOrCreate,FileAccess.Write);
			fout.SetLength(0);	//每次都覆盖原有文件内容。

			DES des = new DESCryptoServiceProvider();
			CryptoStream encStream = new CryptoStream(fout, des.CreateEncryptor(deskey, desiv), CryptoStreamMode.Write);

			//加密			
			encStream.Write(Data, 0,Data.Length);

			encStream.Close();
			fout.Close();
			des.Clear();

			des = null;
			fout = null;
			encStream = null;
		}

		/// <summary> 从指定的文件中解密数据。</summary>
		/// <param name="ReadFileName"> 保存数据的文件[完整路径和文件名称] </param>
		/// <returns> 解密结果 </returns>
		public byte[] DESDecrypt(string ReadFileName)
		{
			FileStream fin = new FileStream(ReadFileName, FileMode.Open, FileAccess.Read);

			DES des = new DESCryptoServiceProvider();
			CryptoStream encStream = new CryptoStream(fin, des.CreateDecryptor(deskey, desiv), CryptoStreamMode.Read);

			byte[] bin = new byte[fin.Length];
			encStream.Read(bin,0,bin.Length);
			encStream.Close();  
			fin.Close();
			des.Clear();
			
			des = null;
			fin = null;
			encStream = null;
			
			return bin;
		}

		/// <summary> 加密信息并放回加密结果 </summary>
		/// <param name="Data"> 未加密信息 </param>
		/// <returns> 加密后的结果 </returns>
		public byte[] DESEncrypt(byte[] Data)
		{
			//加密数据并返回byte[]结果
			DES des = new DESCryptoServiceProvider();
			
			MemoryStream min = new MemoryStream();
			CryptoStream encStream = new CryptoStream(min,des.CreateEncryptor(deskey,desiv),CryptoStreamMode.Write);
			
			encStream.Write(Data,0,Data.Length);
			encStream.FlushFinalBlock();
			min.Position = 0;
			
			byte[] bin = new byte[min.Length];
			min.Read(bin,0,bin.Length);
			encStream.Close(); 
			min.Close();
			des.Clear();
			des = null;
			encStream = null;
			min = null;

			return bin;
		}

		/// <summary>
		/// 解密信息并返回解密结果
		/// </summary>
		/// <param name="Data">未解密信息</param>
		/// <returns>解密后的结果</returns>
		public byte[] DESDecrypt(byte[] Data)
		{
			//解密数据并返回byte[]结果。
			DES des = new DESCryptoServiceProvider();
			
			MemoryStream min = new MemoryStream(Data);

			CryptoStream decStream = new CryptoStream(min,des.CreateDecryptor(deskey,desiv),CryptoStreamMode.Read);
  
			byte[] bin = new byte[Data.Length];

			decStream.Read(bin,0,bin.Length);
			
			decStream.Close();
			min.Close();
			des.Clear();

			des = null;
			min = null;
			decStream = null;

			return bin;
		}
	}
}
