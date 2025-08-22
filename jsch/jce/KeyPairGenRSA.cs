using System;

namespace SSFTP.SharpSsh.jsch.jce
{
	/* -*-mode:java; c-basic-offset:2; -*- */
	/*
	
	*/

	public class KeyPairGenRSA : SSFTP.SharpSsh.jsch.KeyPairGenRSA
	{
		byte[] d;  // private
		byte[] e;  // public
		byte[] n;

		byte[] c; //  coefficient
		byte[] ep; // exponent p
		byte[] eq; // exponent q
		byte[] p;  // prime p
		byte[] q;  // prime q

		System.Security.Cryptography.RSAParameters RSAKeyInfo;

		public void init(int key_size)
		{
			//    KeyPairGenerator keyGen = KeyPairGenerator.getInstance("RSA");
			//    keyGen.initialize(key_size, new SecureRandom());
			//    KeyPair pair = keyGen.generateKeyPair();
			//
			//    PublicKey pubKey=pair.getPublic();
			//    PrivateKey prvKey=pair.getPrivate();

			System.Security.Cryptography.RSACryptoServiceProvider rsa = new System.Security.Cryptography.RSACryptoServiceProvider(key_size);
			RSAKeyInfo = rsa.ExportParameters(true);

			//    d=((RSAPrivateKey)prvKey).getPrivateExponent().toByteArray();
			//    e=((RSAPublicKey)pubKey).getPublicExponent().toByteArray();
			//    n=((RSAKey)prvKey).getModulus().toByteArray();
			//
			//    c=((RSAPrivateCrtKey)prvKey).getCrtCoefficient().toByteArray();
			//    ep=((RSAPrivateCrtKey)prvKey).getPrimeExponentP().toByteArray();
			//    eq=((RSAPrivateCrtKey)prvKey).getPrimeExponentQ().toByteArray();
			//    p=((RSAPrivateCrtKey)prvKey).getPrimeP().toByteArray();
			//    q=((RSAPrivateCrtKey)prvKey).getPrimeQ().toByteArray();

			d= RSAKeyInfo.D ;
			e=RSAKeyInfo.Exponent ;
			n=RSAKeyInfo.Modulus ;

			c=RSAKeyInfo.InverseQ ;
			ep=RSAKeyInfo.DP ;
			eq=RSAKeyInfo.DQ ;
			p=RSAKeyInfo.P ;
			q=RSAKeyInfo.Q ;
		}
		public byte[] getD(){return d;}
		public byte[] getE(){return e;}
		public byte[] getN(){return n;}
		public byte[] getC(){return c;}
		public byte[] getEP(){return ep;}
		public byte[] getEQ(){return eq;}
		public byte[] getP(){return p;}
		public byte[] getQ(){return q;}
		public System.Security.Cryptography.RSAParameters KeyInfo
		{
			get{return RSAKeyInfo;}
		}
	}
}
