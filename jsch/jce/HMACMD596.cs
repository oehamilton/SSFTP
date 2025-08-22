using System;

namespace SSFTP.SharpSsh.jsch.jce
{
	/* -*-mode:java; c-basic-offset:2; -*- */
	/*
	

	*/

	public class HMACMD596 : MAC
	{
		private const String name="hmac-md5-96";
		private const int bsize=12;
		private Org.Mentalis.Security.Cryptography.HMAC mentalis_mac;
		private System.Security.Cryptography.CryptoStream cs;
		//private Mac mac;
		public int getBlockSize(){return bsize;}
		public void init(byte[] key) 
		{
			if(key.Length>16)
			{
				byte[] tmp=new byte[16];
				Array.Copy(key, 0, tmp, 0, 16);	  
				key=tmp;
			}
			//    SecretKeySpec skey=new SecretKeySpec(key, "HmacMD5");
			//    mac=Mac.getInstance("HmacMD5");
			//    mac.init(skey);
			mentalis_mac = new Org.Mentalis.Security.Cryptography.HMAC(new System.Security.Cryptography.MD5CryptoServiceProvider(), key);
			cs = new System.Security.Cryptography.CryptoStream( System.IO.Stream.Null, mentalis_mac, System.Security.Cryptography.CryptoStreamMode.Write);
		} 
		private byte[] tmp=new byte[4];
		public void update(int i)
		{
			tmp[0]=(byte)(i>>24);
			tmp[1]=(byte)(i>>16);
			tmp[2]=(byte)(i>>8);
			tmp[3]=(byte)i;
			update(tmp, 0, 4);
		}
		public void update(byte[] foo, int s, int l)
		{
			//mac.update(foo, s, l); 
			cs.Write(  foo, s, l);
		}
		private byte[] buf=new byte[12];
		public byte[] doFinal()
		{
			cs.Close();
			Array.Copy( mentalis_mac.Hash, 0, buf, 0, 12);
			return buf;
		}
		public String getName()
		{
			return name;
		}
	}
}
