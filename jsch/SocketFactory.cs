using System;
using SSFTP.SharpSsh.java.net;
using System.IO;

namespace SSFTP.SharpSsh.jsch
{
	/* -*-mode:java; c-basic-offset:2; -*- */
	/*
	

	*/

	public interface SocketFactory
	{
		Socket createSocket(String host, int port);
		Stream getInputStream(Socket socket);
		Stream getOutputStream(Socket socket);
	}
}
