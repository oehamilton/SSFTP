using System;

namespace SSFTP.SharpSsh.jsch
{
	/* -*-mode:java; c-basic-offset:2; -*- */
	/*
	
	*/

	public abstract class Cipher
	{
		internal static int ENCRYPT_MODE=0;
		internal static int DECRYPT_MODE=1;
		public abstract int getIVSize(); 
		public abstract int getBlockSize(); 
		public abstract void init(int mode, byte[] key, byte[] iv);
		public abstract void update(byte[] foo, int s1, int len, byte[] bar, int s2);
	}
}
