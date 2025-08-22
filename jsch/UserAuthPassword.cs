using System;

namespace SSFTP.SharpSsh.jsch
{
/* -*-mode:java; c-basic-offset:2; -*- */
/*

*/

class UserAuthPassword : UserAuth{
  internal UserInfo userinfo;
  internal UserAuthPassword(UserInfo userinfo){
   this.userinfo=userinfo;
  }

  public override bool start(Session session) {
//    super.start(session);
//System.out.println("UserAuthPassword: start");
    Packet packet=session.packet;
    Buffer buf=session.buf;
    String username=session.username;
    String password=session.password;
    String dest=username+"@"+session.host;
    if(session.port!=22){
      dest+=(":"+session.port);
    }

    while(true){
      if(password==null){
	if(userinfo==null){
	  //throw new JSchException("USERAUTH fail");
	  return false;
	}
	if(!userinfo.promptPassword("Password for "+dest)){
	  throw new JSchAuthCancelException("password");
	  //break;
	}
	password=userinfo.getPassword();
	if(password==null){
	  throw new JSchAuthCancelException("password");
	  //break;
	}
      }

      byte[] _username=null;
      try{ _username=Util.getBytesUTF8(username); }
      catch{//(java.io.UnsupportedEncodingException e){
	_username=Util.getBytes(username);
      }

      byte[] _password=null;
      try{ _password=Util.getBytesUTF8(password); }
      catch{//(java.io.UnsupportedEncodingException e){
	_password=Util.getBytes(password);
      }

      // send
      // byte      SSH_MSG_USERAUTH_REQUEST(50)
      // string    user name
      // string    service name ("ssh-connection")
      // string    "password"
      // boolen    FALSE
      // string    plaintext password (ISO-10646 UTF-8)
      packet.reset();
      buf.putByte((byte)Session.SSH_MSG_USERAUTH_REQUEST);
      buf.putString(_username);
      buf.putString(Util.getBytes("ssh-connection"));
      buf.putString(Util.getBytes("password"));
      buf.putByte((byte)0);
      buf.putString(_password);
      session.write(packet);

      loop:
      while(true){
	// receive
	// byte      SSH_MSG_USERAUTH_SUCCESS(52)
	// string    service name
	buf=session.read(buf);
	//System.out.println("read: 52 ? "+    buf.buffer[5]);
	if(buf.buffer[5]==Session.SSH_MSG_USERAUTH_SUCCESS){
	  return true;
	}
	if(buf.buffer[5]==Session.SSH_MSG_USERAUTH_BANNER){
	  buf.getInt(); buf.getByte(); buf.getByte();
	  byte[] _message=buf.getString();
	  byte[] lang=buf.getString();
	  String message=null;
	  try{ message=Util.getStringUTF8(_message); }
	  catch{//(java.io.UnsupportedEncodingException e){
	    message=Util.getString(_message);
	  }
	  if(userinfo!=null){
	    userinfo.showMessage(message);
	  }
	  goto loop;
	}
	if(buf.buffer[5]==Session.SSH_MSG_USERAUTH_FAILURE){
	  buf.getInt(); buf.getByte(); buf.getByte(); 
	  byte[] foo=buf.getString();
	  int partial_success=buf.getByte();
	  //System.out.println(new String(foo)+
	  //		 " partial_success:"+(partial_success!=0));
	  if(partial_success!=0){
	    throw new JSchPartialAuthException(Util.getString(foo));
	  }
	  break;
	}
	else{
//        System.out.println("USERAUTH fail ("+buf.buffer[5]+")");
//	  throw new JSchException("USERAUTH fail ("+buf.buffer[5]+")");
	  return false;
	}
      }
      password=null;
    }
    //throw new JSchException("USERAUTH fail");
    //return false;
  }
}

}
