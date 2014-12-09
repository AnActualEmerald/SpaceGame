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

namespace Networking
{
	/// <summary>
	/// Description of Client.
	/// </summary>
	public class Client
	{
	 	IPEndPoint target;
	 	Socket _socket;
		
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
					
					break;
				}
				
				i++;
				
				
			}while(i < attempts);
		}
		
		
	}
}
