using System;

namespace SSFTP.SharpSsh.jsch
{
	/* -*-mode:java; c-basic-offset:2; -*- */
	/*
	
	*/

	internal interface Identity
	{
		bool setPassphrase(String passphrase);
		byte[] getPublicKeyBlob();
		byte[] getSignature(Session session, byte[] data);
		bool decrypt();
		String getAlgName();
		String getName();
		bool isEncrypted();
	}
}
