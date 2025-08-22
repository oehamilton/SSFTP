
using SSFTP.SharpSsh.java;
using SSFTP.SharpSsh.java.lang;

namespace SSFTP.SharpSsh.jsch
{
	/* -*-mode:java; c-basic-offset:2; indent-tabs-mode:nil -*- */
	/*
	
	
	*/

	public class RequestSubsystem : Request
	{
		private bool want_reply=true;
		private String subsystem=null;
		public void request(Session session, Channel channel, String subsystem, bool want_reply) 
		{
			this.subsystem=subsystem;
			this.want_reply=want_reply;
			this.request(session, channel);
		}
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
			buf.putString( new String( "subsystem" ).getBytes());
			buf.putByte((byte)(waitForReply() ? 1 : 0));
			buf.putString(subsystem.getBytes());
			session.write(packet);

			if(reply)
			{
				while(channel.reply==-1)
				{
					try{Thread.sleep(10);}
					catch//(System.Exception ee)
					{
					}
				}
				if(channel.reply==0)
				{
					throw new JSchException("failed to send subsystem request");
				}
			}
		}
		public bool waitForReply(){ return want_reply; }
	}

}
