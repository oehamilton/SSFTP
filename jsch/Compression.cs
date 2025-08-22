using System;

namespace SSFTP.SharpSsh.jsch
{
	/* -*-mode:java; c-basic-offset:2; -*- */
	/*
	
	*/

	public abstract class Compression
	{
		public const int INFLATER=0;
		public const int DEFLATER=1;
		public abstract void init(int type, int level);
		public abstract int compress(byte[] buf, int start, int len);
		public abstract byte[] uncompress(byte[] buf, int start, int[] len);
	}
}
