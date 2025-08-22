using System;

namespace SSFTP.SharpSsh.jsch.jce
{
	/* -*-mode:java; c-basic-offset:2; -*- */
	/*

	*/

	public class TripleDESCBC : SSFTP.SharpSsh.jsch.Cipher
	{
		private const int ivsize=8;
		private const int bsize=24;
		private System.Security.Cryptography.TripleDES triDes; 
		private System.Security.Cryptography.ICryptoTransform cipher;
		//private javax.crypto.Cipher cipher;    
		public override int getIVSize(){return ivsize;} 
		public override int getBlockSize(){return bsize;}
		public override void init(int mode, byte[] key, byte[] iv) 
		{
			triDes = new System.Security.Cryptography.TripleDESCryptoServiceProvider();
			triDes.Mode=System.Security.Cryptography.CipherMode.CBC;
			triDes.Padding=System.Security.Cryptography.PaddingMode.None;
			//String pad="NoPadding";      
			//if(padding) pad="PKCS5Padding";
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
				//      cipher=javax.crypto.Cipher.getInstance("DESede/CBC/"+pad);
				/*
					  // The following code does not work on IBM's JDK 1.4.1
					  SecretKeySpec skeySpec = new SecretKeySpec(key, "DESede");
					  cipher.init((mode==ENCRYPT_MODE?
						   javax.crypto.Cipher.ENCRYPT_MODE:
						   javax.crypto.Cipher.DECRYPT_MODE),
						  skeySpec, new IvParameterSpec(iv));
				*/
				//      DESedeKeySpec keyspec=new DESedeKeySpec(key);
				//      SecretKeyFactory keyfactory=SecretKeyFactory.getInstance("DESede");
				//      SecretKey _key=keyfactory.generateSecret(keyspec);
				//      cipher.init((mode==ENCRYPT_MODE?
				//		   javax.crypto.Cipher.ENCRYPT_MODE:
				//		   javax.crypto.Cipher.DECRYPT_MODE),
				//		  _key, new IvParameterSpec(iv));
				cipher = (mode==ENCRYPT_MODE? 
					triDes.CreateEncryptor(key, iv):
					triDes.CreateDecryptor(key, iv));
			}
			catch(Exception e)
			{
				Console.WriteLine(e);
				cipher=null;
			}
		}
		public override void update(byte[] foo, int s1, int len, byte[] bar, int s2)
		{
			// cipher.update(foo, s1, len, bar, s2);
			cipher.TransformBlock(foo, s1, len, bar, s2);
		}
		public override string ToString()
		{
			return "3des-cbc";
		}

	}

}
