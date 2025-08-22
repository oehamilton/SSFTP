using System;

namespace SSFTP.SharpSsh.jsch
{
	/* -*-mode:java; c-basic-offset:2; -*- */
	/*
	
	*/

	public class RequestWindowChange : Request
	{
		internal int width_columns=80;
		internal int height_rows=24;
		internal int width_pixels=640;
		internal int height_pixels=480;
		public void setSize(int row, int col, int wp, int hp)
		{
			this.width_columns=row; 
			this.height_rows=col; 
			this.width_pixels=wp;
			this.height_pixels=hp;
		}
		public void request(Session session, Channel channel)
		{
			Buffer buf=new Buffer();
			Packet packet=new Packet(buf);

			//byte      SSH_MSG_CHANNEL_REQUEST
			//uint32    recipient_channel
			//string    "window-change"
			//boolean   FALSE
			//uint32    terminal width, columns
			//uint32    terminal height, rows
			//uint32    terminal width, pixels
			//uint32    terminal height, pixels
			packet.reset();
			buf.putByte((byte) Session.SSH_MSG_CHANNEL_REQUEST);
			buf.putInt(channel.getRecipient());
			buf.putString(Util.getBytes("window-change"));
			buf.putByte((byte)(waitForReply() ? 1 : 0));
			buf.putInt(width_columns);
			buf.putInt(height_rows);
			buf.putInt(width_pixels);
			buf.putInt(height_pixels);
			session.write(packet);
		}
		public bool waitForReply(){ return false; }
	}

}
