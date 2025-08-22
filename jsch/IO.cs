using System;
using System.IO;
using SSFTP.SharpSsh.java.io;

namespace SSFTP.SharpSsh.jsch
{
	/* -*-mode:java; c-basic-offset:2; -*- */
	/*
	
	*/

	public class IO
	{
		internal JStream ins;
		internal JStream outs;
		internal JStream outs_ext;

		private bool in_dontclose=false;
		private bool out_dontclose=false;
		private bool outs_ext_dontclose=false;

		public void setOutputStream(Stream outs)
		{
			if(outs!=null)
			{
				this.outs= new JStream(outs); 
			}
			else
			{
				this.outs=null;
			}
		}
		public void setOutputStream(Stream outs, bool dontclose)
		{
			this.out_dontclose=dontclose;
			setOutputStream(outs);
		}
		public void setExtOutputStream(Stream outs)
		{
			if(outs!=null)
			{
				this.outs_ext=new JStream(outs); 
			}
			else
			{
				this.outs_ext=null;
			}
		}
		public void setExtOutputStream(Stream outs, bool dontclose)
		{
			this.outs_ext_dontclose=dontclose;
			setExtOutputStream(outs);
		}
		public void setInputStream(Stream ins)
		{ 
			//ConsoleStream low buffer patch
			if(ins!=null)
			{
				if(ins.GetType() == Type.GetType("System.IO.__ConsoleStream"))
				{
					ins = new SSFTP.Streams.ProtectedConsoleStream(ins);
				}
				else if(ins.GetType() == Type.GetType("System.IO.FileStream"))
				{
					ins = new SSFTP.Streams.ProtectedConsoleStream(ins);
				}
				this.ins=new JStream(ins);
			}
			else
			{
				this.ins=null;
			}
		}
		public void setInputStream(Stream ins, bool dontclose)
		{
			this.in_dontclose=dontclose;
			setInputStream(ins);
		}

		public void put(Packet p)
		{
			outs.Write(p.buffer.buffer, 0, p.buffer.index);
			outs.Flush();
		}
		internal void put(byte[] array, int begin, int length)
		{
			outs.Write(array, begin, length);
			outs.Flush();
		}
		internal void put_ext(byte[] array, int begin, int length)
		{
			outs_ext.Write(array, begin, length);
			outs_ext.Flush();
		}

		internal int getByte()
		{
			int res = ins.ReadByte()&0xff;
			return res; 
		}

		internal void getByte(byte[] array)
		{
			getByte(array, 0, array.Length);
		}

		internal void getByte(byte[] array, int begin, int length)
		{
			do
			{
				int completed = ins.Read(array, begin, length);
				if(completed<=0)
				{
					throw new IOException("End of IO Stream Read");
				}
				begin+=completed;
				length-=completed;
			}
			while (length>0);
		}

		public void close()
		{
			try
			{
				if(ins!=null && !in_dontclose) ins.Close();
				ins=null;
			}
			catch(Exception ee){System.Console.WriteLine(ee.ToString());}
			try
			{
                
                if (outs!=null && !out_dontclose) outs.Close();
				outs=null;
			}
			catch(Exception ee)
            {
                System.Console.WriteLine(ee.ToString());
            }
			try
			{
                
                if (outs_ext!=null && !outs_ext_dontclose) outs_ext.Close();
				outs_ext=null;
			}
			catch(Exception eee)
            {
                System.Console.WriteLine(eee.ToString());
            }
		}

//		public void finalize()
//		{
//			try
//			{
//				if(ins!=null) ins.Close();
//			}
//			catch{}
//			try
//			{
//				if(outs!=null) outs.Close();
//			}
//			catch{}
//		}
	}

}
