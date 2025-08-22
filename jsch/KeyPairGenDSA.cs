using System;

namespace SSFTP.SharpSsh.jsch
{
	/* -*-mode:java; c-basic-offset:2; -*- */
	/*
	
	*/

	public interface KeyPairGenDSA
	{
		void init(int key_size);
		byte[] getX();
		byte[] getY();
		byte[] getP();
		byte[] getQ();
		byte[] getG();
	}

}
