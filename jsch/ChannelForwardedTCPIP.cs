using System;
using System.IO;
using SSFTP.SharpSsh.java.util;
using SSFTP.SharpSsh.java.net;
using SSFTP.SharpSsh.java.lang;
using Str = SSFTP.SharpSsh.java.String;

namespace SSFTP.SharpSsh.jsch
{
	/* -*-mode:java; c-basic-offset:2; -*- */
	/*
	
	*/

	public class ChannelForwardedTCPIP : Channel
	{

		internal static java.util.Vector pool=new java.util.Vector();

		//  static private final int LOCAL_WINDOW_SIZE_MAX=0x20000;
		static private int LOCAL_WINDOW_SIZE_MAX=0x100000;
		static private int LOCAL_MAXIMUM_PACKET_SIZE=0x4000;

		internal SocketFactory factory=null;
		internal String target;
		internal int lport;
		internal int rport;

		internal ChannelForwardedTCPIP() : base()
		{			
			setLocalWindowSizeMax(LOCAL_WINDOW_SIZE_MAX);
			setLocalWindowSize(LOCAL_WINDOW_SIZE_MAX);
			setLocalPacketSize(LOCAL_MAXIMUM_PACKET_SIZE);
		}

		public override void init ()
		{
			try
			{ 
				io=new IO();
				if(lport==-1)
				{
					Class c=Class.forName(target);
					ForwardedTCPIPDaemon daemon=(ForwardedTCPIPDaemon)c.newInstance();
					daemon.setChannel(this);
					Object[] foo=getPort(session, rport);
					daemon.setArg((Object[])foo[3]);
					new Thread(daemon).start();
					connected=true;
					return;
				}
				else
				{
					Socket socket=(factory==null) ? 
						new Socket(target, lport) : 
						factory.createSocket(target, lport);
					socket.setTcpNoDelay(true);
					io.setInputStream(socket.getInputStream());
					io.setOutputStream(socket.getOutputStream());
					connected=true;
				}
			}
			catch(Exception e)
			{
				Console.WriteLine("target={0},port={1}",target,lport);
				Console.WriteLine(e);
			}
		}

		public override void run()
		{
			thread=Thread.currentThread();
			Buffer buf=new Buffer(rmpsize);
			Packet packet=new Packet(buf);
			int i=0;
			try
			{
				while(thread!=null && io!=null && io.ins!=null)
				{
					i=io.ins.Read(buf.buffer, 
						14, 
						buf.buffer.Length-14
						-32 -20 // padding and mac
						);
					if(i<=0)
					{
						eof();
						break;
					}
					packet.reset();
					if(_close)break;
					buf.putByte((byte)Session.SSH_MSG_CHANNEL_DATA);
					buf.putInt(recipient);
					buf.putInt(i);
					buf.skip(i);
					session.write(packet, this, i);
				}
			}
			catch(Exception e)
			{
                Console.WriteLine(e.ToString());
                //System.out.println(e);
            }

			//thread=null;
			//eof();
			disconnect();
		}
		internal override void getData(Buffer buf)
		{
			setRecipient(buf.getInt());
			setRemoteWindowSize(buf.getInt());
			setRemotePacketSize(buf.getInt());
			byte[] addr=buf.getString();
			int port=buf.getInt();
			byte[] orgaddr=buf.getString();
			int orgport=buf.getInt();

			/*
			System.out.println("addr: "+new String(addr));
			System.out.println("port: "+port);
			System.out.println("orgaddr: "+new String(orgaddr));
			System.out.println("orgport: "+orgport);
			*/

			lock(pool)
			{
				for(int i=0; i<pool.size(); i++)
				{
					Object[] foo=(Object[])(pool.elementAt(i));
					if(foo[0]!=session) continue;
					if(((Integer)foo[1]).intValue()!=port) continue;
					this.rport=port;
					this.target=(String)foo[2];
					if(foo[3]==null || (foo[3] is Object[])){ this.lport=-1; }
					else{ this.lport=((Integer)foo[3]).intValue(); }
					if(foo.Length>=5)
					{
						this.factory=((SocketFactory)foo[4]);
					}
					break;
				}
				if(target==null)
				{
					Console.WriteLine("??");
				}
			}
		}

		internal static Object[] getPort(Session session, int rport)
		{
			lock(pool)
			{
				for(int i=0; i<pool.size(); i++)
				{
					Object[] bar=(Object[])(pool.elementAt(i));
					if(bar[0]!=session) continue;
					if(((Integer)bar[1]).intValue()!=rport) continue;
					return bar;
				}
				return null;
			}
		}

		internal static String[] getPortForwarding(Session session)
		{
			java.util.Vector foo=new java.util.Vector();
			lock(pool)
			{
				for(int i=0; i<pool.size(); i++)
				{
					Object[] bar=(Object[])(pool.elementAt(i));
					if(bar[0]!=session) continue;
					if(bar[3]==null){ foo.addElement(bar[1]+":"+bar[2]+":"); }
					else{ foo.addElement(bar[1]+":"+bar[2]+":"+bar[3]); }
				}
			}
			String[] bar2=new String[foo.size()];
			for(int i=0; i<foo.size(); i++)
			{
				bar2[i]=(String)(foo.elementAt(i));
			}
			return bar2;
		}

		internal static void addPort(Session session, int port, String target, int lport, SocketFactory factory)
		{
			lock(pool)
			{
				if(getPort(session, port)!=null)
				{
					throw new JSchException("PortForwardingR: remote port "+port+" is already registered.");
				}
				Object[] foo=new Object[5];
				foo[0]=session; foo[1]=new Integer(port);
				foo[2]=target; foo[3]=new Integer(lport);
				foo[4]=factory;
				pool.addElement(foo);
			}
		}
		internal static void addPort(Session session, int port, String daemon, Object[] arg) 
		{
			lock(pool)
			{
				if(getPort(session, port)!=null)
				{
					throw new JSchException("PortForwardingR: remote port "+port+" is already registered.");
				}
				Object[] foo=new Object[4];
				foo[0]=session; foo[1]=new Integer(port);
				foo[2]=daemon; foo[3]=arg;
				pool.addElement(foo);
			}
		}
		internal static void delPort(ChannelForwardedTCPIP c)
		{
			delPort(c.session, c.rport);
		}
		internal static void delPort(Session session, int rport)
		{
			lock(pool)
			{
				Object[] foo=null;
				for(int i=0; i<pool.size(); i++)
				{
					Object[] bar=(Object[])(pool.elementAt(i));
					if(bar[0]!=session) continue;
					if(((Integer)bar[1]).intValue()!=rport) continue;
					foo=bar;
					break;
				}
				if(foo==null)return;
				pool.removeElement(foo);	
			}

			Buffer buf=new Buffer(100); // ??
			Packet packet=new Packet(buf);

			try
			{
				// byte SSH_MSG_GLOBAL_REQUEST 80
				// string "cancel-tcpip-forward"
				// boolean want_reply
				// string  address_to_bind (e.g. "127.0.0.1")
				// uint32  port number to bind
				packet.reset();
				buf.putByte((byte) 80/*SSH_MSG_GLOBAL_REQUEST*/);
				buf.putString(new Str("cancel-tcpip-forward").getBytes());
				buf.putByte((byte)0);
				buf.putString(new Str("0.0.0.0").getBytes());
				buf.putInt(rport);
				session.write(packet);
			}
			catch(Exception e)
			{
                Console.WriteLine(e.ToString());
                //    throw new JSchException(e.toString());
            }
		}
		internal static void delPort(Session session)
		{
			int[] rport=null;
			int count=0;
			lock(pool)
			{
				rport=new int[pool.size()];
				for(int i=0; i<pool.size(); i++)
				{
					Object[] bar=(Object[])(pool.elementAt(i));
					if(bar[0]==session) 
					{
						rport[count++]=((Integer)bar[1]).intValue();
					}
				}
			}
			for(int i=0; i<count; i++)
			{
				delPort(session, rport[i]);
			}
		}
		public int getRemotePort(){return rport;}
		void setSocketFactory(SocketFactory factory)
		{
			this.factory=factory;
		}
	}

}
