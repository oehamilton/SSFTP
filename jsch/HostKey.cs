using System;

namespace SSFTP.SharpSsh.jsch
{
/* -*-mode:java; c-basic-offset:2; -*- */
/*

*/

	public class HostKey{
		private static byte[] sshdss= System.Text.Encoding.Default.GetBytes( "ssh-dss" );
		private static byte[] sshrsa= System.Text.Encoding.Default.GetBytes( "ssh-rsa" );

		public const int SSHDSS=0;
		public const int SSHRSA=1;
		public const int UNKNOWN=2;

		internal String host;
		internal int type;
		internal byte[] key;
		public HostKey(String host, byte[] key) 
		{
			this.host=host; this.key=key;
			if(key[8]=='d'){ this.type=SSHDSS; }
			else if(key[8]=='r'){ this.type=SSHRSA; }
			else { throw new JSchException("invalid key type");}
		}
		internal HostKey(String host, int type, byte[] key){
			this.host=host; this.type=type; this.key=key;
		}
		public String getHost(){ return host; }
		public String getType(){
			if(type==SSHDSS){ return System.Text.Encoding.Default.GetString(sshdss); }
			if(type==SSHRSA){ return System.Text.Encoding.Default.GetString(sshrsa);}
			return "UNKNOWN";
		}
		public String getKey(){
			return Convert.ToBase64String(key, 0, key.Length);
		}
		public String getFingerPrint(JSch jsch){
			HASH hash=null;
			try{
			hash=(HASH)Activator.CreateInstance(Type.GetType(jsch.getConfig("md5")));
			}
			catch(Exception e){ Console.Error.WriteLine("getFingerPrint: "+e); }
			return Util.getFingerPrint(hash, key);
		}
	}

}
