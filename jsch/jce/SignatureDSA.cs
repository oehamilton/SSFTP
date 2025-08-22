using System;

namespace SSFTP.SharpSsh.jsch.jce
{
	/* -*-mode:java; c-basic-offset:2; -*- */
	/*

	*/

	public class SignatureDSA : SSFTP.SharpSsh.jsch.SignatureDSA
	{

		//java.security.Signature signature;
		//  KeyFactory keyFactory;
		System.Security.Cryptography.DSAParameters DSAKeyInfo;
		System.Security.Cryptography.SHA1CryptoServiceProvider sha1;
		System.Security.Cryptography.CryptoStream cs;

		public void init() 
		{
			sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
			cs = new System.Security.Cryptography.CryptoStream(System.IO.Stream.Null, sha1, System.Security.Cryptography.CryptoStreamMode.Write);
		}     
		public void setPubKey(byte[] y, byte[] p, byte[] q, byte[] g) 
		{
			DSAKeyInfo.Y =  Util.stripLeadingZeros( y );
			DSAKeyInfo.P =  Util.stripLeadingZeros( p ) ;
			DSAKeyInfo.Q =  Util.stripLeadingZeros( q );
			DSAKeyInfo.G =	Util.stripLeadingZeros( g ) ;
		}
		public void setPrvKey(byte[] x, byte[] p, byte[] q, byte[] g)
		{
			DSAKeyInfo.X =  Util.stripLeadingZeros( x );
			DSAKeyInfo.P =  Util.stripLeadingZeros( p );
			DSAKeyInfo.Q =  Util.stripLeadingZeros( q );
			DSAKeyInfo.G =  Util.stripLeadingZeros( g );
		}

		public byte[] sign() 
		{
			//byte[] sig=signature.sign();   
			cs.Close();
			System.Security.Cryptography.DSACryptoServiceProvider DSA = new System.Security.Cryptography.DSACryptoServiceProvider();
			DSA.ImportParameters(DSAKeyInfo);
			System.Security.Cryptography.DSASignatureFormatter DSAFormatter = new System.Security.Cryptography.DSASignatureFormatter(DSA);
			DSAFormatter.SetHashAlgorithm("SHA1");
	  
			byte[] sig =DSAFormatter.CreateSignature( sha1 );
			return sig;
		}
		public void update(byte[] foo) 
		{
			//signature.update(foo);
			cs.Write(  foo , 0, foo.Length);
		}
		public bool verify(byte[] sig)
		{			
			cs.Close();
			System.Security.Cryptography.DSACryptoServiceProvider DSA = new System.Security.Cryptography.DSACryptoServiceProvider();
			DSA.ImportParameters(DSAKeyInfo);
			System.Security.Cryptography.DSASignatureDeformatter DSADeformatter = new System.Security.Cryptography.DSASignatureDeformatter(DSA);
			DSADeformatter.SetHashAlgorithm("SHA1");

			long i=0;
			long j=0;
			byte[] tmp;

			//This makes sure sig is always 40 bytes?
			if(sig[0]==0 && sig[1]==0 && sig[2]==0)
			{
				long i1 = (sig[i++]<<24)&0xff000000;
				long i2 = (sig[i++]<<16)&0x00ff0000;
				long i3 = (sig[i++]<<8)&0x0000ff00;
				long i4 = (sig[i++])&0x000000ff;
				j = i1 | i2 | i3 | i4;

				i+=j;

				i1 = (sig[i++]<<24)&0xff000000;
				i2 = (sig[i++]<<16)&0x00ff0000;
				i3 = (sig[i++]<<8)&0x0000ff00;
				i4 = (sig[i++])&0x000000ff;
				j = i1 | i2 | i3 | i4;

				tmp=new byte[j]; 
				Array.Copy(sig, i, tmp, 0, j); sig=tmp;
			}
			bool res = DSADeformatter.VerifySignature(sha1, sig);
			return res;
		}
	}

}
