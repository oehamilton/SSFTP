using System;
using System.IO;

namespace SSFTP.SharpSsh.jsch
{
	/* -*-mode:java; c-basic-offset:2; -*- */
	/*
	
	*/

	public class JSch
	{

		static System.Collections.Hashtable config;
	
		public static void Init()
		{
			config=new System.Collections.Hashtable();

			//  config.Add("kex", "diffie-hellman-group-exchange-sha1");
			config.Add("kex", "diffie-hellman-group1-sha1,diffie-hellman-group-exchange-sha1");
			config.Add("server_host_key", "ssh-rsa,ssh-dss");
			//config.Add("server_host_key", "ssh-dss,ssh-rsa");

			//			config.Add("cipher.s2c", "3des-cbc,blowfish-cbc");
			//			config.Add("cipher.c2s", "3des-cbc,blowfish-cbc");

			config.Add("cipher.s2c", "3des-cbc,aes128-cbc");
			config.Add("cipher.c2s", "3des-cbc,aes128-cbc");

			//			config.Add("mac.s2c", "hmac-md5,hmac-sha1,hmac-sha1-96,hmac-md5-96");
			//			config.Add("mac.c2s", "hmac-md5,hmac-sha1,hmac-sha1-96,hmac-md5-96");
			config.Add("mac.s2c", "hmac-md5,hmac-sha1");
			config.Add("mac.c2s", "hmac-md5,hmac-sha1");
			config.Add("compression.s2c", "none");
			config.Add("compression.c2s", "none");
			config.Add("lang.s2c", "");
			config.Add("lang.c2s", "");

			config.Add("diffie-hellman-group-exchange-sha1",
                "SSFTP.SharpSsh.jsch.DHGEX");
			config.Add("diffie-hellman-group1-sha1",
                "SSFTP.SharpSsh.jsch.DHG1");

			config.Add("dh", "SSFTP.SharpSsh.jsch.jce.DH");
			config.Add("3des-cbc", "SSFTP.SharpSsh.jsch.jce.TripleDESCBC");
            //config.Add("blowfish-cbc",  "SSFTP.SharpSsh.jsch.jce.BlowfishCBC");
            config.Add("hmac-sha1", "SSFTP.SharpSsh.jsch.jce.HMACSHA1");
			config.Add("hmac-sha1-96", "SSFTP.SharpSsh.jsch.jce.HMACSHA196");
			config.Add("hmac-md5", "SSFTP.SharpSsh.jsch.jce.HMACMD5");
			config.Add("hmac-md5-96", "SSFTP.SharpSsh.jsch.jce.HMACMD596");
			config.Add("sha-1", "SSFTP.SharpSsh.jsch.jce.SHA1");
			config.Add("md5", "SSFTP.SharpSsh.jsch.jce.MD5");
			config.Add("signature.dss", "SSFTP.SharpSsh.jsch.jce.SignatureDSA");
			config.Add("signature.rsa", "SSFTP.SharpSsh.jsch.jce.SignatureRSA");
			config.Add("keypairgen.dsa", "SSFTP.SharpSsh.jsch.jce.KeyPairGenDSA");
			config.Add("keypairgen.rsa", "SSFTP.SharpSsh.jsch.jce.KeyPairGenRSA");
			config.Add("random", "SSFTP.SharpSsh.jsch.jce.Random");

			config.Add("aes128-cbc", "SSFTP.SharpSsh.jsch.jce.AES128CBC");

			//config.Add("zlib",          "com.jcraft.jsch.jcraft.Compression");

			config.Add("StrictHostKeyChecking",  "ask");
		}
	
		internal SSFTP.SharpSsh.java.util.Vector pool=new SSFTP.SharpSsh.java.util.Vector();
		internal SSFTP.SharpSsh.java.util.Vector identities=new SSFTP.SharpSsh.java.util.Vector();
		//private KnownHosts known_hosts=null;
		private HostKeyRepository known_hosts=null;

		public JSch()
		{
			//known_hosts=new KnownHosts(this);
			if (config==null)
				Init();
		}

		public Session getSession(String username, String host)  { return getSession(username, host, 22); }
		public Session getSession(String username, String host, int port)  
		{
			Session s=new Session(this); 
			s.setUserName(username);
			s.setHost(host);
			s.setPort(port);
			pool.Add(s);
			return s;
		}

		internal bool removeSession(Session session)
		{
			lock(pool)
			{
				return pool.remove(session);
			}
		}

		public void setHostKeyRepository(HostKeyRepository foo)
		{
			known_hosts=foo;
		}
		public void setKnownHosts(String foo) 
		{
			if(known_hosts==null) known_hosts=new KnownHosts(this);
			if(known_hosts is KnownHosts)
			{
				lock(known_hosts)
				{
					((KnownHosts)known_hosts).setKnownHosts(foo); 
				}
			}
		}
		public void setKnownHosts(StreamReader foo) 
		{ 
			if(known_hosts==null) known_hosts=new KnownHosts(this);
			if(known_hosts is KnownHosts)
			{
				lock(known_hosts)
				{
					((KnownHosts)known_hosts).setKnownHosts(foo); 
				}
			}
		}
		/*
		HostKeyRepository getKnownHosts(){ 
			if(known_hosts==null) known_hosts=new KnownHosts(this);
			return known_hosts; 
		}
		*/
		public HostKeyRepository getHostKeyRepository()
		{ 
			if(known_hosts==null) known_hosts=new KnownHosts(this);
			return known_hosts; 
		}
		/*
		public HostKey[] getHostKey(){
			if(known_hosts==null) return null;
			return known_hosts.getHostKey(); 
		}
		public void removeHostKey(String foo, String type){
			removeHostKey(foo, type, null);
		}
		public void removeHostKey(String foo, String type, byte[] key){
			if(known_hosts==null) return;
			known_hosts.remove(foo, type, key); 
		}
		*/
		public void addIdentity(String foo) 
		{
			addIdentity(foo, (String)null);
		}
		public void addIdentity(String foo, String bar) 
		{
			Identity identity=new IdentityFile(foo, this);
			if(bar!=null) identity.setPassphrase(bar);
			identities.Add(identity);
		}
		internal String getConfig(String foo){ return (String)(config[foo]); }

		private System.Collections.ArrayList proxies;
		void setProxy(String hosts, Proxy proxy)
		{
			String[] patterns=Util.split(hosts, ",");
			if(proxies==null){proxies=new System.Collections.ArrayList();}
			lock(proxies)
			{
				for(int i=0; i<patterns.Length; i++)
				{
					if(proxy==null)
					{
						proxies[0] = null;
						proxies[0] = System.Text.Encoding.Default.GetBytes( patterns[i] );
					}
					else
					{
						proxies.Add( System.Text.Encoding.Default.GetBytes( patterns[i] ) );
						proxies.Add(proxy);
					}
				}
			}
		}
		internal Proxy getProxy(String host)
		{
			if(proxies==null)return null;
			byte[] _host= System.Text.Encoding.Default.GetBytes( host );
			lock(proxies)
			{
				for(int i=0; i<proxies.Count; i+=2)
				{
					if(Util.glob(((byte[])proxies[i]), _host))
					{
						return (Proxy)(proxies[i+1]);
					}
				}
			}
			return null;
		}
		internal void removeProxy()
		{
			proxies=null;
		}

		public static void setConfig(System.Collections.Hashtable foo)
		{
			lock(config)
			{
				System.Collections.IEnumerator e=foo.Keys.GetEnumerator();
				while(e.MoveNext())
				{
					String key=(String)(e.Current);
					config.Add(key, (String)(foo[key]));
				}
			}
		}
	}

}
