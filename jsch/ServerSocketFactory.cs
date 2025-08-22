using System;
using SSFTP.SharpSsh.java.net;

namespace SSFTP.SharpSsh.jsch
{
	/// <summary>
	/// Summary description for ServerSocketFactory.
	/// </summary>
	public interface ServerSocketFactory
	{
		ServerSocket createServerSocket(int port, int backlog, InetAddress bindAddr);
	}
}
