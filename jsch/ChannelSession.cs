using System;
using SSFTP.SharpSsh.java.lang;
using Str = SSFTP.SharpSsh.java.String;

namespace SSFTP.SharpSsh.jsch
{
	/* -*-mode:java; c-basic-offset:2; -*- */
	/*
	
	*/
	public class ChannelSession : Channel
	{
		private static byte[] _session=new Str("session").getBytes();
		public ChannelSession():base()
		{
			type=_session;
			io=new IO();
		}
  
		public override void run()
		{
			//System.out.println(this+":run >");
			/*
				if(thread!=null){ return; }
				thread=Thread.currentThread();
			*/

			//    Buffer buf=new Buffer();
			Buffer buf=new Buffer(rmpsize);
			Packet packet=new Packet(buf);
			int i=-1;
			try
			{
				while(isConnected() &&
					thread!=null && 
					io!=null && 
					io.ins!=null)
				{
					i=io.ins.Read(buf.buffer, 
						14,    
						buf.buffer.Length-14
						-32 -20 // padding and mac
						);
					if(i==0)continue;
					if(i==-1)
					{
						eof();
						break;
					}
					if(_close)break;
					packet.reset();
					buf.putByte((byte)Session.SSH_MSG_CHANNEL_DATA);
					buf.putInt(recipient);
					buf.putInt(i);
					buf.skip(i);
					session.write(packet, this, i);
				}
			}
			catch(Exception e)
			{
				Console.WriteLine("# ChannelSession.run");
				Console.WriteLine(e);
			}
			if(thread!=null)
			{
				//lock(thread){ System.Threading.Monitor.PulseAll(this);/*thread.notifyAll();*/ }
			}
			thread=null;
			//System.out.println(this+":run <");
		}
	}
}
