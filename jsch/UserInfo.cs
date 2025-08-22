using System;

namespace SSFTP.SharpSsh.jsch
{
	/* -*-mode:java; c-basic-offset:2; -*- */
	/*
	
	*/

	public interface UserInfo
	{
		String getPassphrase();
		String getPassword();
		bool promptPassword(String message);
		bool promptPassphrase(String message);
		bool promptYesNo(String message);
		void showMessage(String message);
	}
}
