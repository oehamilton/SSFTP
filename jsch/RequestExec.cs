using System;
using Str = SSFTP.SharpSsh.java.String;

namespace SSFTP.SharpSsh.jsch
{
	/* -*-mode:java; c-basic-offset:2; -*- */
	/*
	
	*/

	internal class RequestExec : Request
	{
		private String command="";
		internal RequestExec(String foo)
		{
			this.command=foo;
		}
		public void request(Session session, Channel channel)
		{
			Packet packet=session.packet;
			Buffer buf=session.buf;
			// send
			// byte     SSH_MSG_CHANNEL_REQUEST(98)
			// uint32 recipient channel
			// string request type       // "exec"
			// boolean want reply        // 0
			// string command
			packet.reset();
			buf.putByte((byte) Session.SSH_MSG_CHANNEL_REQUEST);
			buf.putInt(channel.getRecipient());
			buf.putString(new Str("exec").getBytes());
			buf.putByte((byte)(waitForReply() ? 1 : 0));
			buf.putString(new Str(command).getBytes());
			session.write(packet);
		}
		public bool waitForReply(){ return false; }
	}
}
