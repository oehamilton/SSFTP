using System;

namespace SSFTP.SharpSsh.jsch.jce
{
/* -*-mode:java; c-basic-offset:2; -*- */
/*

*/

public class SHA1 : SSFTP.SharpSsh.jsch.HASH{
  //MessageDigest md;
	internal System.Security.Cryptography.SHA1CryptoServiceProvider md;
	private System.Security.Cryptography.CryptoStream cs;

  public override int getBlockSize(){return 20;}
  public override void init(){
    try
	{ 
		//md=MessageDigest.getInstance("SHA-1");
		md=new System.Security.Cryptography.SHA1CryptoServiceProvider();
		cs = new System.Security.Cryptography.CryptoStream( System.IO.Stream.Null, md, System.Security.Cryptography.CryptoStreamMode.Write);
	}
    catch(Exception e){
      Console.WriteLine(e);
    }
  }
  public override void update(byte[] foo, int start, int len){
    //md.update(foo, start, len);
	  cs.Write(foo, start, len);
  }
  public override byte[] digest() {
    //return md.digest();
	  cs.Close();
	  byte[] result = md.Hash; 
	  md.Clear();
	  init(); //Reinitiazing hash objects

	  return result;
  }
}

}
