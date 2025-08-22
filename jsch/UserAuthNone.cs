using SSFTP.SharpSsh.java;

namespace SSFTP.SharpSsh.jsch
{
	/* -*-mode:java; c-basic-offset:2; -*- */
	/*
	
	*/

	class UserAuthNone : UserAuth
	{
		private String methods=null;
		private UserInfo userinfo;
		internal UserAuthNone(UserInfo userinfo)
		{
			this.userinfo=userinfo;
		}

		public override bool start(Session session)
		{
			base.start(session);
			//System.out.println("UserAuthNone: start");
			Packet packet=session.packet;
			Buffer buf=session.buf;
			String username=session.username;

			byte[] _username=null;
			try{ _username=Util.getBytesUTF8(username); }
			catch
			{//(java.io.UnsupportedEncodingException e){
				_username=Util.getBytes(username);
			}

			// send
			// byte      SSH_MSG_USERAUTH_REQUEST(50)
			// string    user name
			// string    service name ("ssh-connection")
			// string    "none"
			packet.reset();
			buf.putByte((byte)Session.SSH_MSG_USERAUTH_REQUEST);
			buf.putString(_username);
			buf.putString(Util.getBytes("ssh-connection"));
			buf.putString(Util.getBytes("none"));
			session.write(packet);

			loop:
				while(true)
				{
					// receive
					// byte      SSH_MSG_USERAUTH_SUCCESS(52)
					// string    service name
					buf=session.read(buf);
					//System.out.println("UserAuthNone: read: 52 ? "+    buf.buffer[5]);
					if(buf.buffer[5]==Session.SSH_MSG_USERAUTH_SUCCESS)
					{
						return true;
					}
					if(buf.buffer[5]==Session.SSH_MSG_USERAUTH_BANNER)
					{
						buf.getInt(); buf.getByte(); buf.getByte();
						byte[] _message=buf.getString();
						byte[] lang=buf.getString();
						String message=null;
						try{ message=Util.getStringUTF8(_message); }
						catch
						{//(java.io.UnsupportedEncodingException e){
							message=Util.getString(_message);
						}
						if(userinfo!=null)
						{
							userinfo.showMessage(message);
						}
						goto loop;
					}
					if(buf.buffer[5]==Session.SSH_MSG_USERAUTH_FAILURE)
					{
						buf.getInt(); buf.getByte(); buf.getByte(); 
						byte[] foo=buf.getString();
						int partial_success=buf.getByte();
						methods=Util.getString(foo);
						//System.out.println("UserAuthNONE: "+methods+
						//		   " partial_success:"+(partial_success!=0));
						//	if(partial_success!=0){
						//	  throw new JSchPartialAuthException(new String(foo));
						//	}
						break;
					}
					else
					{
						//      System.out.println("USERAUTH fail ("+buf.buffer[5]+")");
						throw new JSchException("USERAUTH fail ("+buf.buffer[5]+")");
					}
				}
			//throw new JSchException("USERAUTH fail");
			return false;
		}
		internal String getMethods()
		{
			return methods;
		}
	}

}
