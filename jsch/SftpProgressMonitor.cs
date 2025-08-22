using System;

namespace SSFTP.SharpSsh.jsch
{
	/* -*-mode:java; c-basic-offset:2; -*- */
	/*
	
	*/

	public abstract class SftpProgressMonitor
	{
		public static int PUT=0;
		public static int GET=1;
		public abstract void init(int op, String src, String dest, long max);
		public abstract bool count(long count);
		public abstract void end();
	}
}
