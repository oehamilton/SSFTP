using System;

namespace SSFTP.SharpSsh.jsch
{
	/* -*-mode:java; c-basic-offset:2; -*- */
	/*
	
	*/

	public class Buffer
	{
		static byte[] tmp=new byte[4];
		internal byte[] buffer;
		internal int index;
		internal int s;
		public Buffer(int size)
		{
			buffer=new byte[size];
			index=0;
			s=0;
		}
		public Buffer(byte[] buffer)
		{
			this.buffer=buffer;
			index=0;
			s=0;
		}
		public Buffer():this(1024*10*2){ }
		public void putByte(byte foo)
		{
			buffer[index++]=foo;
		}
		public void putByte(byte[] foo) 
		{
			putByte(foo, 0, foo.Length);
		}
		public void putByte(byte[] foo, int begin, int length) 
		{
			Array.Copy(foo, begin, buffer, index, length);
			index+=length;
		}
		public void putString(byte[] foo)
		{
			putString(foo, 0, foo.Length);
		}
		public void putString(byte[] foo, int begin, int length) 
		{
			putInt(length);
			putByte(foo, begin, length);
		}
		public void putInt(int v) 
		{
			uint val = (uint)v;
			tmp[0]=(byte)(val >> 24);
			tmp[1]=(byte)(val >> 16);
			tmp[2]=(byte)(val >> 8);
			tmp[3]=(byte)(val);
			Array.Copy(tmp, 0, buffer, index, 4);
			index+=4;
		}
		public void putLong(long v) 
		{
			ulong val = (ulong)v;
			tmp[0]=(byte)(val >> 56);
			tmp[1]=(byte)(val >> 48);
			tmp[2]=(byte)(val >>40);
			tmp[3]=(byte)(val >> 32);
			Array.Copy(tmp, 0, buffer, index, 4);
			tmp[0]=(byte)(val >> 24);
			tmp[1]=(byte)(val >> 16);
			tmp[2]=(byte)(val >> 8);
			tmp[3]=(byte)(val);
			Array.Copy(tmp, 0, buffer, index+4, 4);
			index+=8;
		}
		internal void skip(int n) 
		{
			index+=n;
		}
		internal void putPad(int n) 
		{
			while(n>0)
			{
				buffer[index++]=(byte)0;
				n--;
			}
		}
		public void putMPInt(byte[] foo)
		{
			int i=foo.Length;
			if((foo[0]&0x80)!=0)
			{
				i++;
				putInt(i);
				putByte((byte)0);
			}
			else
			{
				putInt(i);
			}
			putByte(foo);
		}
		public int getLength()
		{
			return index-s;
		}
		public int getOffSet()
		{
			return s;
		}
		public void setOffSet(int s)
		{
			this.s=s;
		}
		public long getLong()
		{
			long foo = getInt()&0xffffffffL;
			foo = ((foo<<32)) | (getInt()&0xffffffffL);
			return foo;
		}
		public int getInt()
		{
			uint foo = (uint) getShort();
			foo = ((foo<<16)&0xffff0000) | ((uint)getShort()&0xffff);
			return (int)foo;
		}
		internal int getShort() 
		{
			int foo = getByte();
			foo = ((foo<<8)&0xff00)|(getByte()&0xff);
			return foo;
		}
		public int getByte() 
		{
			return (buffer[s++]&0xff);
		}
		public void getByte(byte[] foo) 
		{
			getByte(foo, 0, foo.Length);
		}
		void getByte(byte[] foo, int start, int len) 
		{
			Array.Copy(buffer, s, foo, start, len); 
			s+=len;
		}
		public int getByte(int len) 
		{
			int foo=s;
			s+=len;
			return foo;
		}
		public byte[] getMPInt() 
		{
			int i=getInt();
			byte[] foo=new byte[i];
			getByte(foo, 0, i);
			return foo;
		}
		public byte[] getMPIntBits() 
		{
			int bits=getInt();
			int bytes=(bits+7)/8;
			byte[] foo=new byte[bytes];
			getByte(foo, 0, bytes);
			if((foo[0]&0x80)!=0)
			{
				byte[] bar=new byte[foo.Length+1];
				bar[0]=0; // ??
				Array.Copy(foo, 0, bar, 1, foo.Length);
				foo=bar;
			}
			return foo;
		}
		public byte[] getString() 
		{
			int i=getInt();
			byte[] foo=new byte[i];
			getByte(foo, 0, i);
			return foo;
		}
		internal byte[] getString(int[]start, int[]len) 
		{
			int i=getInt();
			start[0]=getByte(i);
			len[0]=i;
			return buffer;
		}
		public void reset()
		{
			index=0;
			s=0;
		}
		public void shift()
		{
			if(s==0)return;
			Array.Copy(buffer, s, buffer, 0, index-s);
			index=index-s;
			s=0;
		}
		internal void rewind()
		{
			s=0;
		}

		/*

		*/

	}

}
