using System;

namespace SSFTP.SharpSsh.jsch
{
	/* -*-mode:java; c-basic-offset:2; -*- */
	/*
	
	*/

	public abstract class HASH
	{
		public abstract void init();
		public abstract int getBlockSize();
		public abstract void update(byte[] foo, int start, int len);
		public abstract byte[] digest();
	}
}
