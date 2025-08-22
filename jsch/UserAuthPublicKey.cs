using System;
using System.IO;

namespace SSFTP.SharpSsh.jsch
{
	/* -*-mode:java; c-basic-offset:2; -*- */
	/*
	
	*/

	class UserAuthPublicKey : UserAuth
	{
		internal UserInfo userinfo;
		internal UserAuthPublicKey(UserInfo userinfo)
		{
			this.userinfo=userinfo;
		}

		public override bool start(Session session) 
		{
			//super.start(session);

			//Vector identities=JSch.identities;
			System.Collections.ArrayList identities=session.jsch.identities;

			Packet packet=session.packet;
			Buffer buf=session.buf;

			String passphrase=null;
			String username=session.username;

			byte[] _username=null;
			try{ _username= Util.getBytesUTF8( username); }
			catch
			{//(java.io.UnsupportedEncodingException e){
				_username=Util.getBytes(username);
			}

			for(int i=0; i<identities.Count; i++)
			{
				Identity identity=(Identity)(identities[i]);
				byte[] pubkeyblob=identity.getPublicKeyBlob();

				//System.out.println("UserAuthPublicKey: "+identity+" "+pubkeyblob);

				if(pubkeyblob!=null)
				{
					// send
					// byte      SSH_MSG_USERAUTH_REQUEST(50)
					// string    user name
					// string    service name ("ssh-connection")
					// string    "publickey"
					// boolen    FALSE
					// string    plaintext password (ISO-10646 UTF-8)
					packet.reset();
					buf.putByte((byte)Session.SSH_MSG_USERAUTH_REQUEST);
					buf.putString(_username);
					buf.putString(Util.getBytes("ssh-connection"));
					buf.putString(Util.getBytes("publickey"));
					buf.putByte((byte)0);
					buf.putString(Util.getBytes(identity.getAlgName()));
					buf.putString(pubkeyblob);
					session.write(packet);

				loop1:
					while(true)
					{
						// receive
						// byte      SSH_MSG_USERAUTH_PK_OK(52)
						// string    service name
						buf=session.read(buf);
						//System.out.println("read: 60 ? "+    buf.buffer[5]);
						if(buf.buffer[5]==Session.SSH_MSG_USERAUTH_PK_OK)
						{
							break;
						}
						else if(buf.buffer[5]==Session.SSH_MSG_USERAUTH_FAILURE)
						{
							//	System.out.println("USERAUTH publickey "+session.getIdentity()+
							//			   " is not acceptable.");
							break;
						}
						else if(buf.buffer[5]==Session.SSH_MSG_USERAUTH_BANNER)
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
							goto loop1;
						}
						else
						{
							//System.out.println("USERAUTH fail ("+buf.buffer[5]+")");
							//throw new JSchException("USERAUTH fail ("+buf.buffer[5]+")");
							break;
						}
					}
					if(buf.buffer[5]!=Session.SSH_MSG_USERAUTH_PK_OK)
					{
						continue;
					}
				}

				//System.out.println("UserAuthPublicKey: identity.isEncrypted()="+identity.isEncrypted());

				int count=5;
				while(true)
				{
					if((identity.isEncrypted() && passphrase==null))
					{
						if(userinfo==null) throw new JSchException("USERAUTH fail");
						if(identity.isEncrypted() &&
							!userinfo.promptPassphrase("Passphrase for "+identity.getName()))
						{
							throw new JSchAuthCancelException("publickey");
							//throw new JSchException("USERAUTH cancel");
							//break;
						}
						passphrase=userinfo.getPassphrase();
					}

					if(!identity.isEncrypted() || passphrase!=null)
					{
						//System.out.println("UserAuthPublicKey: @1 "+passphrase);
						if(identity.setPassphrase(passphrase))
							break;
					}
					passphrase=null;
					count--;
					if(count==0)break;
				}

				//System.out.println("UserAuthPublicKey: identity.isEncrypted()="+identity.isEncrypted());

				if(identity.isEncrypted()) continue;
				if(pubkeyblob==null) pubkeyblob=identity.getPublicKeyBlob();

				//System.out.println("UserAuthPublicKey: pubkeyblob="+pubkeyblob);

				if(pubkeyblob==null) continue;

				// send
				// byte      SSH_MSG_USERAUTH_REQUEST(50)
				// string    user name
				// string    service name ("ssh-connection")
				// string    "publickey"
				// boolen    TRUE
				// string    plaintext password (ISO-10646 UTF-8)
				packet.reset();
				buf.putByte((byte)Session.SSH_MSG_USERAUTH_REQUEST);
				buf.putString(_username);
				buf.putString(Util.getBytes("ssh-connection"));
				buf.putString(Util.getBytes("publickey"));
				buf.putByte((byte)1);
				buf.putString(Util.getBytes(identity.getAlgName()));
				buf.putString(pubkeyblob);

				//      byte[] tmp=new byte[buf.index-5];
				//      System.arraycopy(buf.buffer, 5, tmp, 0, tmp.length);
				//      buf.putString(signature);

				byte[] sid=session.getSessionId();
				uint sidlen=(uint)sid.Length;
				byte[] tmp=new byte[4+sidlen+buf.index-5];
				tmp[0]=(byte)(sidlen>>24);
				tmp[1]=(byte)(sidlen>>16);
				tmp[2]=(byte)(sidlen>>8);
				tmp[3]=(byte)(sidlen);
				Array.Copy(sid, 0, tmp, 4, sidlen);
				Array.Copy(buf.buffer, 5, tmp, 4+sidlen, buf.index-5);

				byte[] signature=identity.getSignature(session, tmp);
				if(signature==null)
				{  // for example, too long key length.
					break;
				}
				buf.putString(signature);

				session.write(packet);

			loop2:
				while(true)
				{
					// receive
					// byte      SSH_MSG_USERAUTH_SUCCESS(52)
					// string    service name
					buf=session.read(buf);
					//System.out.println("read: 52 ? "+    buf.buffer[5]);
					if(buf.buffer[5]==Session.SSH_MSG_USERAUTH_SUCCESS)
					{
						return true;
					}
					else if(buf.buffer[5]==Session.SSH_MSG_USERAUTH_BANNER)
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
						goto loop2;
					}
					else if(buf.buffer[5]==Session.SSH_MSG_USERAUTH_FAILURE)
					{
						buf.getInt(); buf.getByte(); buf.getByte(); 
						byte[] foo=buf.getString();
						int partial_success=buf.getByte();
						//System.out.println(new String(foo)+
						//                   " partial_success:"+(partial_success!=0));
						if(partial_success!=0)
						{
							throw new JSchPartialAuthException(Util.getString(foo));
						}
						break;
					}
					//System.out.println("USERAUTH fail ("+buf.buffer[5]+")");
					//throw new JSchException("USERAUTH fail ("+buf.buffer[5]+")");
					break;
				}
			}
			return false;
		}
	}

}
