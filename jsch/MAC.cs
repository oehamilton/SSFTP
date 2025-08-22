using System;

namespace SSFTP.SharpSsh.jsch
{
	/* -*-mode:java; c-basic-offset:2; -*- */
	/*
	
	*/

	public interface MAC
	{
		String getName();
		int getBlockSize(); 
		void init(byte[] key);
		void update(byte[] foo, int start, int len);
		void update(int foo);
		byte[] doFinal();
	}

}
