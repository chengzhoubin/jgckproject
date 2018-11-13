using System;
using System.IO;
using System.Security.Cryptography;

namespace JGCK.Util.Crypto
{
	/// <summary>
	/// 使用Rijndael算法得加密解密
	/// </summary>
	public class RijndaelCrypto
	{
		/// <summary> 密匙 </summary>
		private byte[] rijkey;
		/// <summary> 向量 </summary>
		private byte[] rijiv;
		/// <summary> Rij 密匙 </summary>
		public byte[] RijKey
		{
			get { return rijkey; }
			set	{	rijkey = value; }
		}

		/// <summary> Rij向量 </summary>
		public byte[] RijIv
		{
			get	{	return rijiv;	}
			set	{	rijiv = value; }
		}

		/// <summary> 创建类的实例并随机生成一对密匙和向量 </summary>
		public RijndaelCrypto()
		{
			RijndaelManaged rij = new RijndaelManaged();
			rij.GenerateKey();
			rij.GenerateIV();
			rijkey = rij.Key;
			rijiv = rij.IV;
			rij.Clear();
			rij = null;
		}

		/// <summary> 创建类的实例并初始化密匙和向量 </summary>
		/// <param name="RijKey">密匙</param>
		/// <param name="RijIV">向量</param>
		public RijndaelCrypto(byte[] RijKey,byte[] RijIV)
		{
			rijkey = RijKey;
			rijiv = RijIV;
		}

		/// <summary> 加密数据并返回加密结果 </summary>
		/// <param name="Data"> 未加密数据 </param>
		/// <returns> 加密后数据 </returns>
		public byte[] RijEncrypt(byte[] Data)
		{
			//加密
			MemoryStream min = new MemoryStream();
 
			RijndaelManaged rij = new RijndaelManaged();
			CryptoStream encStream = new CryptoStream(min,rij.CreateEncryptor(rijkey,rijiv),CryptoStreamMode.Write);
			encStream.Write(Data,0,Data.Length);
			encStream.FlushFinalBlock();
			min.Position = 0;

			byte[] bin = new byte[min.Length];
			min.Read(bin,0,bin.Length);
			
			rij.Clear();
			encStream.Close();
			min.Close();

			rij = null;
			encStream = null;
			min = null;

			return bin;
		}
		
		/// <summary> 解密数据并返回结果 </summary>
		/// <param name="Data"> 已经加密数据 </param>
		/// <returns> 解密结果 </returns>
		public byte[] RijDecrypt(byte[] Data)
		{
			//解密
			MemoryStream min = new MemoryStream(Data.Length);

			RijndaelManaged rij = new RijndaelManaged();
			CryptoStream decStream = new CryptoStream(min,rij.CreateDecryptor(rijkey,rijiv),CryptoStreamMode.Read);
			
			min.Write(Data,0,Data.Length);
			min.Position = 0;
			
			byte[] bin = new Byte[Data.Length];
			decStream.Read(bin,0,bin.Length);
			 
			decStream.Close();
			decStream = null;
			min.Close();
			min = null;
			rij.Clear();
			rij = null;
			
			return bin;
		}

		/// <summary>
		/// 加密数据并保存结果到指定的文件中
		/// </summary>
		/// <param name="Data">需加密数据</param>
		/// <param name="WriteFileName">保存结果的文件</param>
		public void RijEncrypt(byte[] Data,string WriteFileName)
		{
			//加密数据到文件中
			FileStream fout = new FileStream(WriteFileName,FileMode.OpenOrCreate, FileAccess.Write);
			
			RijndaelManaged rij = new RijndaelManaged();
			CryptoStream encStream = new CryptoStream(fout,rij.CreateEncryptor(rijkey,rijiv),CryptoStreamMode.Write);
			encStream.Write(Data,0,Data.Length);

			rij.Clear();
			rij = null;
			encStream.Close();
			encStream = null;
			fout.Close();		
			fout = null;
		}

		/// <summary>
		/// 解密数据指定文件中的数据
		/// </summary>
		/// <param name="ReadFileName">保存数据的文件</param>
		/// <returns>解密结果</returns>
		public byte[] RijDecrypt(string ReadFileName)
		{
			//从文件中解密数据
			FileStream fin = new FileStream(ReadFileName,FileMode.Open,FileAccess.Read);
  
			RijndaelManaged rij = new RijndaelManaged();
			CryptoStream decStream = new CryptoStream(fin,rij.CreateDecryptor(rijkey,rijiv),CryptoStreamMode.Read);  
			byte[] bin = new byte[fin.Length];
			decStream.Read(bin,0,bin.Length);

			rij.Clear();
			rij = null;
			decStream.Close();
			decStream = null;
			fin.Close();
			fin = null;

			return bin;
		}
	}
}
