using System;
using SSFTP.SharpSsh.jsch;
using System.Collections;

/* 
 * Sftp.cs
 * 
 * 
 * 
 **/

namespace SSFTP.SharpSsh
{
	public class Sftp : SshTransferProtocolBase
	{
		private MyProgressMonitor m_monitor;
		private bool cancelled = false;

		public Sftp(string sftpHost, string user, string password)
			: base(sftpHost, user, password)
		{
			Init();
		}

		public Sftp(string sftpHost, string user)
			: base(sftpHost, user)
		{
			Init();
		}

		private void Init()
		{
			m_monitor = new MyProgressMonitor(this);
		}

		protected override string ChannelType
		{
			get { return "sftp"; }
		}

		private ChannelSftp SftpChannel
		{
			get { return (ChannelSftp)m_channel; }
		}

		public override void Cancel()
		{
			cancelled = true;
		}

		//Get

		public void Get(string fromFilePath)
		{
			Get(fromFilePath, ".");
		}

		public void Get(string[] fromFilePaths)
		{
			for (int i = 0; i < fromFilePaths.Length; i++)
			{
				Get(fromFilePaths[i]);
			}
		}

		public void Get(string[] fromFilePaths, string toDirPath)
		{
			for (int i = 0; i < fromFilePaths.Length; i++)
			{
				Get(fromFilePaths[i], toDirPath);
			}
		}

		public override void Get(string fromFilePath, string toFilePath)
		{
			cancelled=false;
			SftpChannel.get(fromFilePath, toFilePath, m_monitor, ChannelSftp.OVERWRITE);
		}

		//Put

		public void Put(string fromFilePath)
		{
			Put(fromFilePath, ".");
		}

		public void Put(string[] fromFilePaths)
		{
			for (int i = 0; i < fromFilePaths.Length; i++)
			{
				Put(fromFilePaths[i]);
			}
		}

		public void Put(string[] fromFilePaths, string toDirPath)
		{
			for (int i = 0; i < fromFilePaths.Length; i++)
			{
				Put(fromFilePaths[i], toDirPath);
			}
		}

		public override void Put(string fromFilePath, string toFilePath)
		{
			cancelled=false;
			SftpChannel.put(fromFilePath, toFilePath, m_monitor, ChannelSftp.OVERWRITE);
        }

		//MkDir

		public override  void Mkdir(string directory)
		{
			SftpChannel.mkdir(directory);
		}

		//Ls

		public ArrayList GetFileList(string path)
		{
			ArrayList list = new ArrayList();
            string FileDetail = "";
			foreach(ChannelSftp.LsEntry entry in SftpChannel.ls(path))
			{

                FileDetail = entry.getFilename().ToString() + " " + entry.getAttrs().toString();
                //list.Add(entry.getFilename().ToString());
                list.Add(FileDetail);
                FileDetail = "";

			}
			return list;
		}

		#region ProgressMonitor Implementation

		private class MyProgressMonitor : SftpProgressMonitor
		{
			private long transferred = 0;
			private long total = 0;
			private int elapsed = -1;
			private Sftp m_sftp;
			private string src;
			private string dest;

			System.Timers.Timer timer;

			public MyProgressMonitor(Sftp sftp)
			{
				m_sftp = sftp;
			}

			public override void init(int op, String src, String dest, long max)
			{
				this.src=src;
				this.dest=dest;
				this.elapsed = 0;
				this.total = max;
				timer = new System.Timers.Timer(1000);
				timer.Start();
				timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);

				string note;
				if (op.Equals(GET))
				{
					note = "Downloading " + System.IO.Path.GetFileName( src ) + "...";
				}
				else
				{
					note = "Uploading " + System.IO.Path.GetFileName( src ) + "...";
				}
				m_sftp.SendStartMessage(src, dest, (int)total, note);
			}
			public override bool count(long c)
			{
				this.transferred += c;
				string note = ("Transfering... [Elapsed time: " + elapsed + "]");
				m_sftp.SendProgressMessage(src, dest, (int)transferred, (int)total, note);
				return !m_sftp.cancelled;
			}
			public override void end()
			{
				timer.Stop();
				timer.Dispose();
				string note = ("Done in " + elapsed + " seconds!");
				m_sftp.SendEndMessage(src, dest, (int)transferred, (int)total, note);
				transferred = 0;
				total = 0;
				elapsed = -1;
				src=null;
				dest=null;
			}

			private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
			{
				this.elapsed++;
			}
		}

		#endregion ProgressMonitor Implementation
	}	
}
