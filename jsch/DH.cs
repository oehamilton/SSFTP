using System;

namespace SSFTP.SharpSsh.jsch
{
	/* -*-mode:java; c-basic-offset:2; -*- */
	/*
	
	*/

	public interface DH
	{
		void init();
		void setP(byte[] p);
		void setG(byte[] g);
		byte[] getE();
		void setF(byte[] f);
		byte[] getK();
	}
}
