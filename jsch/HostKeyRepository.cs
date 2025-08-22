	/* -*-mode:java; c-basic-offset:2; -*- */
	/*
	
	*/

using System;

namespace SSFTP.SharpSsh.jsch
{
	public abstract class HostKeyRepository
	{
		internal const int OK=0;
		internal const int NOT_INCLUDED=1;
		internal const int CHANGED=2;

		public abstract int check(String host, byte[] key);
		public abstract void add(String host, byte[] key, UserInfo ui);
		public abstract void remove(String host, String type);
		public abstract void remove(String host, String type, byte[] key);
		public abstract String getKnownHostsRepositoryID();
		public abstract HostKey[] getHostKey();
		public abstract HostKey[] getHostKey(String host, String type);
	}
}
