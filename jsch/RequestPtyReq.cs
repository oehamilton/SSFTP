using System;

namespace SSFTP.SharpSsh.jsch
{
	/* -*-mode:java; c-basic-offset:2; -*- */
	/*
	
	*/

	class RequestPtyReq : Request
	{
		void setCode(String cookie)
		{
		}
		public void request(Session session, Channel channel)
		{
			Buffer buf=new Buffer();
			Packet packet=new Packet(buf);

			packet.reset();
			buf.putByte((byte) Session.SSH_MSG_CHANNEL_REQUEST);
			buf.putInt(channel.getRecipient());
			buf.putString(Util.getBytes("pty-req"));
			buf.putByte((byte)(waitForReply() ? 1 : 0));
			buf.putString(Util.getBytes("vt100"));
			buf.putInt(80);
			buf.putInt(24);
			buf.putInt(640);
			buf.putInt(480);
			buf.putString(Util.getBytes(""));
			session.write(packet);
		}
		public bool waitForReply(){ return false; }
	}

}
