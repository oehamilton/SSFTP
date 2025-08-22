using System;

namespace SSFTP.SharpSsh.jsch.jce
{
	/* -*-mode:java; c-basic-offset:2; -*- */
	/*

	*/

	public class Random : SSFTP.SharpSsh.jsch.Random
	{
		private byte[] tmp=new byte[16];
		//private SecureRandom random;
		private System.Security.Cryptography.RNGCryptoServiceProvider rand;
		public Random()
		{
			//    random=null;
			//	  random = new SecureRandom();
			//	  
			//    try{ random=SecureRandom.getInstance("SHA1PRNG"); }
			//    catch(java.security.NoSuchAlgorithmException e){ 
			//      // System.out.println(e); 
			//
			//      // The following code is for IBM's JCE
			//      try{ random=SecureRandom.getInstance("IBMSecureRandom"); }
			//      catch(java.security.NoSuchAlgorithmException ee){ 
			//	System.out.println(ee); 
			//      }
			//    }
			rand = new System.Security.Cryptography.RNGCryptoServiceProvider();
		}
		static int times = 0;
		public void fill(byte[] foo, int start, int len)
		{
			try
			{
				if(len>tmp.Length){ tmp=new byte[len]; }
				//random.nextBytes(tmp);
				rand.GetBytes(tmp);
				Array.Copy(tmp, 0, foo, start, len);
			}
			catch(Exception e)
			{
                System.Console.WriteLine(e.ToString());
                times++;
				Console.WriteLine(times+") Array.Copy(tmp={0}, 0, foo={1}, {2}, {3}", tmp.Length, foo.Length, start, len);
				//Console.WriteLine(e.StackTrace);
			}
		}
	}

}
