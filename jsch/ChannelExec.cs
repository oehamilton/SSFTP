using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using SSFTP.SharpSsh.java.lang;

namespace SSFTP.SharpSsh.jsch
{
	/* -*-mode:java; c-basic-offset:2; -*- */
	/*
	
	*/

	public class ChannelExec : ChannelSession
	{
		bool xforwading=false;
		bool pty=false;
		String command="";
		/*
		ChannelExec(){
		  super();
		  type="session".getBytes();
		  io=new IO();
		}
		*/
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

				request=new RequestExec(command);
				request.request(session, this);
			}
			catch(Exception e)
			{
                Console.WriteLine(e.ToString());
                throw new JSchException("ChannelExec");
			}
			thread=new Thread(this);
			thread.setName("Exec thread "+session.getHost());
			thread.start();
		}
		public void setCommand(String foo){ command=foo;}
		public override void init()
		{
			io.setInputStream(session.In);
			io.setOutputStream(session.Out);
		}
		//public void finalize() throws java.lang.Throwable{ super.finalize(); }
		public void setErrStream(Stream Out)
		{
			setExtOutputStream(Out);
		}
		public Stream getErrStream() 
		{
			return getExtInputStream();
		}
	}

}
