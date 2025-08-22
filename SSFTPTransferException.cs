using System;

/* 
 * SshTransferException.cs
 * 
 * 
 * 
 **/
namespace SSFTP.SharpSsh
{
	/// <summary>
	/// Summary description for SshTransferException.
	/// </summary>
	public class SshTransferException : Exception
	{
		public SshTransferException(string msg):base(msg)
		{
		}
	}
}
