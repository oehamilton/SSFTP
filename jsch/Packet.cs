using System;

namespace SSFTP.SharpSsh.jsch
{
	/* -*-mode:java; c-basic-offset:2; -*- */
	/*
	
	*/

	public class Packet
	{

		private static Random random=null;
		internal static void setRandom(Random foo){ random=foo;}

		internal Buffer buffer;
		internal byte[] tmp=new byte[4]; 
		public Packet(Buffer buffer)
		{
			this.buffer=buffer;
		}
		public void reset()
		{
			buffer.index=5;
		}
//		internal void padding()
//		{
//			uint len=(uint)buffer.index;
//			int pad=(int) ((-len)&7);
//			if(pad<8)
//			{
//				pad+=8;
//			}
//			len=(uint)(len+pad-4);
//			tmp[0]=(byte)(len>>24);
//			tmp[1]=(byte)(len>>16);
//			tmp[2]=(byte)(len>>8);
//			tmp[3]=(byte)(len);
//			Array.Copy(tmp, 0, buffer.buffer, 0, 4);
//			buffer.buffer[4]=(byte)pad;
//			lock(random)
//			{
//									random.fill(buffer.buffer, buffer.index, pad);
//								}
//			buffer.skip(pad);
//			//buffer.putPad(pad);
//			/*
//			for(int i=0; i<buffer.index; i++){
//			  System.out.print(Integer.toHexString(buffer.buffer[i]&0xff)+":");
//			}
//			System.out.println("");
//			*/
//		}

		internal void padding(int bsize)
		{
			uint len=(uint)buffer.index;
			int pad=(int)( (-len)&(bsize-1) );
			if(pad<bsize)
			{
				pad+=bsize;
			}
			len=(uint)(len+pad-4);
			tmp[0]=(byte)(len>>24);
			tmp[1]=(byte)(len>>16);
			tmp[2]=(byte)(len>>8);
			tmp[3]=(byte)(len);
			Array.Copy(tmp, 0, buffer.buffer, 0, 4);
			buffer.buffer[4]=(byte)pad;
			lock(random)
			{
				random.fill(buffer.buffer, buffer.index, pad);
			}
			buffer.skip(pad);
			//buffer.putPad(pad);
			/*
			for(int i=0; i<buffer.index; i++){
			System.out.print(Integer.toHexString(buffer.buffer[i]&0xff)+":");
			}
			System.out.println("");
			*/
		}

		internal int shift(int len, int mac)
		{
			int s=len+5+9;
			int pad=(-s)&7;
			if(pad<8)pad+=8;
			s+=pad;
			s+=mac;

			Array.Copy(buffer.buffer, 
				len+5+9, 
				buffer.buffer, s, buffer.index-5-9-len);
			buffer.index=10;
			buffer.putInt(len);
			buffer.index=len+5+9;
			return s;
		}
		internal void unshift(byte command, int recipient, int s, int len)
		{
			Array.Copy(buffer.buffer, 
				s, 
				buffer.buffer, 5+9, len);
			buffer.buffer[5]=command;
			buffer.index=6;
			buffer.putInt(recipient);
			buffer.putInt(len);
			buffer.index=len+5+9;
		}

	}

}
