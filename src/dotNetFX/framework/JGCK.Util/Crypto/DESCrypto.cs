//
// DESCrypto.cs
//
// DES �㷨�ļ��ܡ����ܷ���
// ���������ݻ����ļ��ӡ�����
//

using System.IO;
using System.Security.Cryptography;

namespace JGCK.Util.Crypto
{
	/// <summary>
	/// ʹ�� DES �㷨�ӡ����ܷ���
	/// </summary>
	public class DESCrypto
	{
		/// <summary> �ܳ�(8 bytes) </summary>
		private byte[] deskey;
		/// <summary> ����(8 bytes) </summary>
		private byte[] desiv;
		
		/// <summary> DES �ܳ�(8 bytes) </summary>
		public byte[] desKey
		{
			get	{	return deskey; }
			set	{	deskey = value;	}
		}

		/// <summary> DES����(8 bytes) </summary>
		public byte[] desIV
		{
			get	{	return desiv;	}
			set	{	desiv = value; }
		}

		/// <summary> �������ʵ�������������һ���ܳ׺����� </summary>
		public DESCrypto()
		{
			// ÿ�δ�������ʾ��������һ�������������
			DES des = new DESCryptoServiceProvider();
			des.GenerateKey();
			des.GenerateIV(); 
			deskey = des.Key;
			desiv = des.IV;
			des.Clear();
			des = null;
		}

		/// <summary>
		/// �������ʵ������ʼ���ܳ׺�����
		/// </summary>
		/// <param name="desKey">�ܳ�</param>
		/// <param name="desIV">����</param>
		public DESCrypto(byte[] desKey,byte[] desIV)
		{
			deskey = desKey;
			desiv = desIV;
		}

		/// <summary> ������Ϣ�����浽�ļ��� </summary>
		/// <param name="Data"> ������Ϣ </param>
		/// <param name="WriteFileName"> �ļ����� </param>
		public void DESEncrypt(byte[] Data,	string WriteFileName)
		{
			//�������ݵ�ָ�����ļ��У��ļ�����·������

			FileStream fout = new FileStream(WriteFileName,FileMode.OpenOrCreate,FileAccess.Write);
			fout.SetLength(0);	//ÿ�ζ�����ԭ���ļ����ݡ�

			DES des = new DESCryptoServiceProvider();
			CryptoStream encStream = new CryptoStream(fout, des.CreateEncryptor(deskey, desiv), CryptoStreamMode.Write);

			//����			
			encStream.Write(Data, 0,Data.Length);

			encStream.Close();
			fout.Close();
			des.Clear();

			des = null;
			fout = null;
			encStream = null;
		}

		/// <summary> ��ָ�����ļ��н������ݡ�</summary>
		/// <param name="ReadFileName"> �������ݵ��ļ�[����·�����ļ�����] </param>
		/// <returns> ���ܽ�� </returns>
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

		/// <summary> ������Ϣ���Żؼ��ܽ�� </summary>
		/// <param name="Data"> δ������Ϣ </param>
		/// <returns> ���ܺ�Ľ�� </returns>
		public byte[] DESEncrypt(byte[] Data)
		{
			//�������ݲ�����byte[]���
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
		/// ������Ϣ�����ؽ��ܽ��
		/// </summary>
		/// <param name="Data">δ������Ϣ</param>
		/// <returns>���ܺ�Ľ��</returns>
		public byte[] DESDecrypt(byte[] Data)
		{
			//�������ݲ�����byte[]�����
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
