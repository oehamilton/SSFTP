using System;

namespace SSFTP.SharpSsh.jsch.jce
{
	/* -*-mode:java; c-basic-offset:2; -*- */
	/*
	
	*/

	public class MD5 : SSFTP.SharpSsh.jsch.HASH
	{
		//MessageDigest md;
		internal System.Security.Cryptography.MD5CryptoServiceProvider md;
		private System.Security.Cryptography.CryptoStream cs;

		public override int getBlockSize(){return 16;}
		public override void init() 
		{
			try
			{ 
				//md=MessageDigest.getInstance("MD5"); 
				md = new System.Security.Cryptography.MD5CryptoServiceProvider();
				cs = new System.Security.Cryptography.CryptoStream( System.IO.Stream.Null, md, System.Security.Cryptography.CryptoStreamMode.Write);
			}
			catch(Exception e)
			{
				Console.WriteLine(e);
			}
		}
		public override void update(byte[] foo, int start, int len) 
		{
			//md.update(foo, start, len);
			cs.Write(foo, start, len);
		}
		public override byte[] digest() 
		{
			cs.Close();
			byte[] result = md.Hash; 
			md.Clear();//Reinitiazing hash objects
			init();

			return result;
		}
	}
}
