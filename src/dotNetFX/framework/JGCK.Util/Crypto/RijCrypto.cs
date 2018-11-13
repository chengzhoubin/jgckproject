using System;
using System.IO;
using System.Security.Cryptography;

namespace JGCK.Util.Crypto
{
	/// <summary>
	/// ʹ��Rijndael�㷨�ü��ܽ���
	/// </summary>
	public class RijndaelCrypto
	{
		/// <summary> �ܳ� </summary>
		private byte[] rijkey;
		/// <summary> ���� </summary>
		private byte[] rijiv;
		/// <summary> Rij �ܳ� </summary>
		public byte[] RijKey
		{
			get { return rijkey; }
			set	{	rijkey = value; }
		}

		/// <summary> Rij���� </summary>
		public byte[] RijIv
		{
			get	{	return rijiv;	}
			set	{	rijiv = value; }
		}

		/// <summary> �������ʵ�����������һ���ܳ׺����� </summary>
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

		/// <summary> �������ʵ������ʼ���ܳ׺����� </summary>
		/// <param name="RijKey">�ܳ�</param>
		/// <param name="RijIV">����</param>
		public RijndaelCrypto(byte[] RijKey,byte[] RijIV)
		{
			rijkey = RijKey;
			rijiv = RijIV;
		}

		/// <summary> �������ݲ����ؼ��ܽ�� </summary>
		/// <param name="Data"> δ�������� </param>
		/// <returns> ���ܺ����� </returns>
		public byte[] RijEncrypt(byte[] Data)
		{
			//����
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
		
		/// <summary> �������ݲ����ؽ�� </summary>
		/// <param name="Data"> �Ѿ��������� </param>
		/// <returns> ���ܽ�� </returns>
		public byte[] RijDecrypt(byte[] Data)
		{
			//����
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
		/// �������ݲ���������ָ�����ļ���
		/// </summary>
		/// <param name="Data">���������</param>
		/// <param name="WriteFileName">���������ļ�</param>
		public void RijEncrypt(byte[] Data,string WriteFileName)
		{
			//�������ݵ��ļ���
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
		/// ��������ָ���ļ��е�����
		/// </summary>
		/// <param name="ReadFileName">�������ݵ��ļ�</param>
		/// <returns>���ܽ��</returns>
		public byte[] RijDecrypt(string ReadFileName)
		{
			//���ļ��н�������
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
