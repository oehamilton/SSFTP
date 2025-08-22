using System;
using SSFTP.SharpSsh.java.lang;

namespace SSFTP.SharpSsh.jsch
{
	/* -*-mode:java; c-basic-offset:2; -*- */
	/*
	
	*/

	public class ChannelShell : ChannelSession
	{
		internal bool xforwading=false;
		internal bool pty=true;
		public override void setXForwarding(bool foo){ xforwading=foo; }
		public void setPty(bool foo){ pty=foo; }
		public override void start()
		{
			try
			{
				Request request;
				if(xforwading)
				{
					request=new RequestX11();
					request.request(session, this);
				}
				if(pty)
				{
					request=new RequestPtyReq();
					request.request(session, this);
				}
				request=new RequestShell();
				request.request(session, this);
			}
			catch//(Exception e)
			{
				throw new JSchException("ChannelShell");
			}
			thread=new Thread(this);
			thread.setName("Shell for "+session.host);
			thread.start();
		}
		public override void init()
		{
			io.setInputStream(session.In);
			io.setOutputStream(session.Out);
		}

		public void setPtySize(int col, int row, int wp, int hp)
		{
			//if(thread==null) return;
			try
			{
				RequestWindowChange request=new RequestWindowChange();
				request.setSize(col, row, wp, hp);
				request.request(session, this);
			}
			catch(Exception e)
			{
				throw new JSchException("ChannelShell.setPtySize: "+e.ToString());
			}
		}
	}

}
