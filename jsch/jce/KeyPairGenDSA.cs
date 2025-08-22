using System;

namespace SSFTP.SharpSsh.jsch.jce
{
	/* -*-mode:java; c-basic-offset:2; -*- */
	/*
	
	*/

	public class KeyPairGenDSA : SSFTP.SharpSsh.jsch.KeyPairGenDSA 
	{
		byte[] x;  // private
		byte[] y;  // public
		byte[] p;
		byte[] q;
		byte[] g;

		public void init(int key_size)
		{
//			KeyPairGenerator keyGen = KeyPairGenerator.getInstance("DSA");
//			keyGen.initialize(key_size, new SecureRandom());
//			KeyPair pair = keyGen.generateKeyPair();
//			PublicKey pubKey=pair.getPublic();
//			PrivateKey prvKey=pair.getPrivate();

			System.Security.Cryptography.DSACryptoServiceProvider dsa = new System.Security.Cryptography.DSACryptoServiceProvider(key_size);
			System.Security.Cryptography.DSAParameters DSAKeyInfo = dsa.ExportParameters(true);

//			x=((DSAPrivateKey)prvKey).getX().toByteArray();
//			y=((DSAPublicKey)pubKey).getY().toByteArray();
//
//			DSAParams _params=((DSAKey)prvKey).getParams();
//			p=_params.getP().toByteArray();
//			q=_params.getQ().toByteArray();
//			g=_params.getG().toByteArray();

			x = DSAKeyInfo.X;
			y = DSAKeyInfo.Y;
			p = DSAKeyInfo.P;
			q = DSAKeyInfo.Q;
			g = DSAKeyInfo.G;
		}
		public byte[] getX(){return x;}
		public byte[] getY(){return y;}
		public byte[] getP(){return p;}
		public byte[] getQ(){return q;}
		public byte[] getG(){return g;}
	}

}
