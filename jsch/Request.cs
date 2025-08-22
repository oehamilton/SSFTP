using System;

namespace SSFTP.SharpSsh.jsch
{
	/* -*-mode:java; c-basic-offset:2; -*- */
	/*
	
	*/

	internal interface Request
	{
		bool waitForReply();
		void request(Session session, Channel channel);
	}
}
