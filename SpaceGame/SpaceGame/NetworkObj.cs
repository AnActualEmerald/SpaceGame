/*
 * Created by SharpDevelop.
 * User: Burrito119
 * Date: 12/8/2014
 * Time: 1:18 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.Xna.Framework;
using SpaceGameSever.Udp;
using Util;

namespace Networking
{
	/// <summary>
	/// Description of Client.
	/// </summary>
	public class NetworkObj
	{
	 	IPEndPoint target;
	 	IPEndPoint conn;
	 	UdpListner cl;
		
	 	private Vector2 pos;
	 	
	 	public NetworkObj(String IPTarget, int conn_port, int main_port)
		{
			target = new IPEndPoint(new IPAddress(Encoding.ASCII.GetBytes(IPTarget)), main_port);
			conn = new IPEndPoint(new IPAddress(Encoding.ASCII.GetBytes(IPTarget)), conn_port);
			cl = new UdpListner(conn.Address, conn_port);
		}
	 	
	 	public NetworkObj(long IPTarget, int conn_port, int main_port)
		{
			target = new IPEndPoint(new IPAddress(IPTarget), main_port);
			conn = new IPEndPoint(new IPAddress(IPTarget), conn_port);
			cl = new UdpListner(conn.Address, conn.Port);
		}
		
	 	public void Connect()
	 	{
	 		Console.WriteLine("Connecting to server");
	 		cl.Send("Burrito119", conn);
	 		//Console.WriteLine("Listening for respopnse");
	 		//cl.BeginListenTo(new IPEndPoint(IPAddress.Any, 25566), new AsyncCallback(finishConnect));
		}
		
		private void finishConnect(IAsyncResult r)
		{
			Packet p = cl.EndListenTo(r);
			Console.WriteLine("Got response from server: " + p.message);
			if(p.message == "sendtex"){
				Console.WriteLine("CS: Sending texture to server");
				cl.Send(Encoding.ASCII.GetString(new byte[]{0, 5, 24, 33, 1, 69, 3, 2, 9, 5}), conn);
			}
			if(p.message == "sendverts"){
				Console.WriteLine("CS: Sending verts to server");
				cl.Send("0,0;4,5;6,6;1,3", conn);
			}
			if(p.message == "start")
			{
				Console.WriteLine("Starting actual things");
				return;
			}
			cl.BeginListenTo(conn, finishConnect);
		}
		
		private Object ParseSeverRequest(String s)
		{
			if(s == "getpos")
				Console.WriteLine("Sadly, this isn't done yet");
			
			return null;
				
		}
		
		private void loop()
		{
			byte[] b = new byte[2048];
			
			while(true)
			{
				
			}
			
			
		}
		
		
		private void parse(byte[] bytes)
		{
			
		}
	}
}
