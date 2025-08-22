/* -*-mode:java; c-basic-offset:2; indent-tabs-mode:nil -*- */
/*

*/

using System;
using SSFTP.SharpSsh.java.net;
using SSFTP.SharpSsh.java.lang;

namespace SSFTP.SharpSsh.jsch
{
	public class ChannelSubsystem : ChannelSession
	{
		bool xforwading=false;
		bool pty=false;
		bool want_reply=true;
		String subsystem="";
		public override void setXForwarding(bool foo){ xforwading=true; }
		public void setPty(bool foo){ pty=foo; }
		public void setWantReply(bool foo){ want_reply=foo; }
		public void setSubsystem(String foo){ subsystem=foo; }
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
				request=new RequestSubsystem();
				((RequestSubsystem)request).request(session, this, subsystem, want_reply);
			}
			catch(Exception e)
			{
				if(e is JSchException){ throw (JSchException)e; }
				throw new JSchException("ChannelSubsystem");
			}
			Thread thread=new Thread(this);
			thread.setName("Subsystem for "+session.host);
			thread.start();
		}
		//public void finalize() throws Throwable{ super.finalize(); }
		public override void init()
		{
			io.setInputStream(session.In);
			io.setOutputStream(session.Out);
		}
		public void setErrStream(System.IO.Stream outs)
		{
			setExtOutputStream(outs);
		}
		public java.io.InputStream getErrStream() 
		{
			return getExtInputStream();
		}
	}

}
