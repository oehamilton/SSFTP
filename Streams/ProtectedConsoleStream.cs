using System;
using System.IO;

/* 
 * ProtectedConsoleStream.cs
 * 
 * 
 * 
 **/
namespace SSFTP.Streams
{
	/// <summary>
	/// This class provide access to the console stream obtained by calling the
	/// Console.OpenStandardInput() and Console.OpenStandardOutput(), and prevents reading 
	/// into buffers to large for the Console Stream
	/// </summary>
	public class ProtectedConsoleStream : System.IO.Stream
	{
		Stream s;
		public ProtectedConsoleStream(Stream s)
		{
			if((s.GetType() != Type.GetType("System.IO.__ConsoleStream"))&&
				(s.GetType() != Type.GetType("System.IO.FileStream")))//for mono
			{
				throw new ArgumentException("Not ConsoleStream");
			}
			this.s=s;
		}

//		public static Stream Protect(Stream s)
//		{
//			if(s.GetType() == Console.
//		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			if(count > 256)
				count = 256;
			return s.Read(buffer, offset, count);
		}

		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return s.BeginRead (buffer, offset, count, callback, state);
		}

		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return s.BeginWrite (buffer, offset, count, callback, state);
		}
		
		public override bool CanRead
		{
			get
			{
				return s.CanRead;
			}
		}

		public override bool CanSeek
		{
			get
			{
				return s.CanSeek;
			}
		}
		public override bool CanWrite
		{
			get
			{
				return s.CanWrite;
			}
		}
		public override void Close()
		{
			s.Close ();
		}
		public override System.Runtime.Remoting.ObjRef CreateObjRef(Type requestedType)
		{
			return s.CreateObjRef (requestedType);
		}
		public override int EndRead(IAsyncResult asyncResult)
		{
			return s.EndRead (asyncResult);
		}
		public override void EndWrite(IAsyncResult asyncResult)
		{
			s.EndWrite (asyncResult);
		}
		public override bool Equals(object obj)
		{
			return s.Equals (obj);
		}
		public override void Flush()
		{
			s.Flush();
		}
		public override int GetHashCode()
		{
			return s.GetHashCode ();
		}
		public override object InitializeLifetimeService()
		{
			return s.InitializeLifetimeService ();
		}
		public override long Length
		{
			get
			{
				return s.Length;
			}
		}
		public override long Position
		{
			get
			{
				return s.Position;
			}
			set
			{
				s.Position = value;
			}
		}
		public override int ReadByte()
		{
			return s.ReadByte ();
		}
		public override long Seek(long offset, SeekOrigin origin)
		{
			return s.Seek(offset, origin);
		}
		public override void SetLength(long value)
		{
			s.SetLength(value);
		}
		public override string ToString()
		{
			return s.ToString ();
		}
		public override void Write(byte[] buffer, int offset, int count)
		{
			s.Write(buffer, offset, count);
		}
		public override void WriteByte(byte value)
		{
			s.WriteByte (value);
		}

	}
}
