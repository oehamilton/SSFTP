using System;
using SSFTP.SharpSsh.java.lang;

namespace SSFTP.SharpSsh.jsch
{
	public interface ForwardedTCPIPDaemon : Runnable
	{
		void setChannel(ChannelForwardedTCPIP channel);
		void setArg(Object[] arg);
	}
}
