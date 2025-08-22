using System;

namespace SSFTP.SharpSsh.jsch
{
	/* -*-mode:java; c-basic-offset:2; -*- */
	/*
	
	*/

	public interface UIKeyboardInteractive
	{
		String[] promptKeyboardInteractive(String destination,
			String name,
			String instruction,
			String[] prompt,
			bool[] echo);
	}
}
