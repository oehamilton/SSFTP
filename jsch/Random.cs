using System;

namespace SSFTP.SharpSsh.jsch
{
	/* -*-mode:java; c-basic-offset:2; -*- */
	/*
	
	*/

	public interface Random
	{
		void fill(byte[] foo, int start, int len);
	}
}
