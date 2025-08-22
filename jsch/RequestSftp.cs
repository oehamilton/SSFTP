using System;

namespace SSFTP.SharpSsh.jsch
{
	/* -*-mode:java; c-basic-offset:2; -*- */
	/*
	
	*/

	public class RequestSftp : Request
	{
		public void request(Session session, Channel channel)
		{
			Buffer buf=new Buffer();
			Packet packet=new Packet(buf);

			bool reply=waitForReply();
			if(reply)
			{
				channel.reply=-1;
			}

			packet.reset();
			buf.putByte((byte)Session.SSH_MSG_CHANNEL_REQUEST);
			buf.putInt(channel.getRecipient());
			buf.putString(Util.getBytes("subsystem"));
			buf.putByte((byte)(waitForReply() ? 1 : 0));
			buf.putString(Util.getBytes("sftp"));
			session.write(packet);

			if(reply)
			{
				while(channel.reply==-1)
				{
					try{System.Threading.Thread.Sleep(10);}
					catch//(Exception ee)
					{
					}
				}
				if(channel.reply==0)
				{
					throw new JSchException("failed to send sftp request");
				}
			}
		}
		public bool waitForReply(){ return true; }
	}

}
