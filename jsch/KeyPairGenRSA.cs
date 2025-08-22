using System;

namespace SSFTP.SharpSsh.jsch
{
	/* -*-mode:java; c-basic-offset:2; -*- */
	/*
	
	*/

	public interface KeyPairGenRSA
	{
		void init(int key_size);
		byte[] getD();
		byte[] getE();
		byte[] getN();

		byte[] getC();
		byte[] getEP();
		byte[] getEQ();
		byte[] getP();
		byte[] getQ();
	}

}
