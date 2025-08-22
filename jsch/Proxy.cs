using SSFTP.SharpSsh.java;
using System.IO;
using SSFTP.SharpSsh.java.net;

namespace SSFTP.SharpSsh.jsch
{
	/* -*-mode:java; c-basic-offset:2; -*- */
	/*
	
	
	*/
	public interface Proxy
	{
		void connect(SocketFactory socket_factory, String host, int port, int timeout);
		Stream getInputStream();
		Stream getOutputStream();
		Socket getSocket();
		void close();
	}
}
