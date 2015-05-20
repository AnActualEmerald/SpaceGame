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
	 	UdpClient cl;
		
	 	private Vector2 pos;
	 	
	 	public NetworkObj(String IPTarget, int main_port)
		{
	 		target = new IPEndPoint(IPAddress.Parse(IPTarget), main_port);
			cl = new UdpClient();
		}
	 	
	 	public NetworkObj(long IPTarget, int main_port)
		{
	 		
			target = new IPEndPoint(new IPAddress(IPTarget), main_port);
			cl = new UdpClient();
		}
	 	
	 	public NetworkObj(bool local)
	 	{
	 		target = new IPEndPoint(IPAddress.Loopback, 28889);
	 		cl = new UdpClient();
	 	}
		
	 	public void Connect()
	 	{
	 		Console.WriteLine("Connecting to server");
	 		cl.Connect(target);
	 		cl.SendAsync(Encoding.ASCII.GetBytes("Burrito119"), 10);
	 		Console.WriteLine("Listening for respopnse");
	 		finishConnect();
		}
		
		private async void finishConnect()
		{
			while(true){
				UdpReceiveResult p = await cl.ReceiveAsync();
				String msg = Encoding.ASCII.GetString(p.Buffer);
				Console.WriteLine("Got response from server: " + msg);
				if(msg == "sendtex"){
					Console.WriteLine("CS: Sending texture to server");
					byte[] buff = new byte[]{0, 5, 24, 33, 1, 69, 3, 2, 9, 5};
					cl.Send(buff, buff.Length);
				}
				if(msg == "sendverts"){
					Console.WriteLine("CS: Sending verts to server");
					byte[] buff = Encoding.ASCII.GetBytes("0,0;4,5;6,6;1,3");
					cl.Send(buff, buff.Length);
				}
				if(msg == "start")
				{
					Console.WriteLine("Starting actual things");
					loop();
					return;
				}
			}
		}
		
		private Object ParseSeverRequest(byte[] b)
		{
			string s = Encoding.ASCII.GetString(b);
			Console.WriteLine("Got: "+ s);
			if(s == "sendinput"){
				byte[] buff = Encoding.ASCII.GetBytes("W_DOWN;S_UP;A_DOWN");
				cl.Send(buff, buff.Length);
			}
			return null;
				
		}
		
		private async void loop()
		{
			
			while(true)
			{
				UdpReceiveResult r = await cl.ReceiveAsync();
				ParseSeverRequest(r.Buffer);
			}
			
			
		}
		
		
		private void parse(byte[] bytes)
		{
			
		}
	}
}
