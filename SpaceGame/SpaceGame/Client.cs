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

namespace Networking
{
	/// <summary>
	/// Description of Client.
	/// </summary>
	public class Client
	{
	 	IPEndPoint target;
	 	Socket _socket;
		
	 	private Vector2 pos;
	 	
		public Client(String IPTarget, int port)
		{
			target = new IPEndPoint(new IPAddress(Encoding.ASCII.GetBytes(IPTarget)), port);
			
			_socket = new Socket(SocketType.Dgram, ProtocolType.Udp);
			
		}
		
		public void Connect(int attempts)
		{
			int i = 0;
			do{
				_socket.Connect(target);
				
				if(_socket.Connected)
				{
					finishConnect();
					loop();
					break;
				}
				
				i++;
				
				
			}while(i < attempts);
		}
		
		private void finishConnect()
		{
			byte[] b = new byte[1024];
			byte[] bb = new byte[1024];
			String s = String.Empty;
			String send;
			do{
				_socket.Receive(b, b.Length, SocketFlags.None);
				
				s = Encoding.ASCII.GetString(b);
				
				if(s.Equals("getpos"))
				{
					send = pos.X+";"+pos.Y;
					_socket.Send(Encoding.ASCII.GetBytes(send), send.Length, SocketFlags.None);
				}//this will be finished later
				else{
					Console.WriteLine("Asked for "+s);
				}
				
			}while(s != "start");
			
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
