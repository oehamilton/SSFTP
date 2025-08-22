using System;
using System.Security.Cryptography;

namespace SSFTP.SharpSsh.jsch.jce
{
	/* -*-mode:java; c-basic-offset:2; -*- */
	/*
	

	
	*/

	public class AES128CBC : Cipher
	{
		private int mode;
		private const int ivsize=16;
		private const int bsize=16;
		private System.Security.Cryptography.RijndaelManaged rijndael; 
		private ICryptoTransform cipher;
		public override int getIVSize(){return ivsize;} 
		public override int getBlockSize(){return bsize;}
		public override void init(int mode, byte[] key, byte[] iv) 
		{
			this.mode=mode;
			rijndael = new RijndaelManaged();
			rijndael.Mode = CipherMode.CBC;
			rijndael.Padding = PaddingMode.None;
			//String pad="NoPadding";      
			byte[] tmp;
			if(iv.Length>ivsize)
			{
				tmp=new byte[ivsize];
				Array.Copy(iv, 0, tmp, 0, tmp.Length);
				iv=tmp;
			}
			if(key.Length>bsize)
			{
				tmp=new byte[bsize];
				Array.Copy(key, 0, tmp, 0, tmp.Length);
				key=tmp;
			}

			try
			{
				//      SecretKeySpec keyspec=new SecretKeySpec(key, "AES");
				//		cipher=javax.crypto.Cipher.getInstance("AES/CBC/"+pad);
		
				//      cipher.init((mode==ENCRYPT_MODE?
				//		   javax.crypto.Cipher.ENCRYPT_MODE:
				//		   javax.crypto.Cipher.DECRYPT_MODE),
				//		  keyspec, new IvParameterSpec(iv));
				cipher = (mode==ENCRYPT_MODE? 
					rijndael.CreateEncryptor(key, iv):
					rijndael.CreateDecryptor(key, iv));
			}
			catch(Exception e)
			{
				Console.WriteLine(e);
				cipher=null;
			}
		}
		public override void update(byte[] foo, int s1, int len, byte[] bar, int s2) 
		{
			//cipher.update(foo, s1, len, bar, s2);
			cipher.TransformBlock(foo, s1, len, bar, s2);
		}

		public override string ToString()
		{
			return "aes128-cbc";
		}
	}

}
