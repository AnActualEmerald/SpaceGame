/*
 * Created by SharpDevelop.
 * User: kgauthier16
 * Date: 11/25/2014
 * Time: 11:57 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using ServerParts;
using System.Drawing;
using System.Drawing.Imaging;

namespace SpaceGameSever
{
	/// <summary>
	/// Description of Server.
	/// </summary>
	/// 
	
	public class Server
	{
		private List<Client> clients = new List<Client>();
		private Socket _serverSocket = new Socket(SocketType.Dgram, ProtocolType.Udp);
		
		public const int POINT_TYPE = 0x00;
		public const int ROT_TYPE = 0x01;
		public const int TEX_TYPE = 0x02;
		
		private byte[] _buffer = new byte[10000];
		
		public Server()
		{
			
		}
		
		
		
		public void AcceptCall(IAsyncResult r)
		{
			Socket s = _serverSocket.EndAccept(r);
			Client c = new Client(s);
			GetClientInfo(s, c);
			clients.Add(c);
			s.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ClientLoopRecieve), s);
			
		}

		public void GetClientInfo(Socket s, Client c)
		{
			SendText("getpos", s);
			byte[] b;
			Point pos;
			int rot;
			Bitmap tex;
			s.Receive(b);
			ParseClientinfo(b, POINT_TYPE, out pos, out rot, out tex);
			c.Ship_pos = pos;
			SendText("getrot", s);
			s.Receive(b);
			ParseClientinfo(b, ROT_TYPE, out pos, out rot, out tex);
			c.Ship_rot = rot;
			SendText("gettex", s);
			ParseClientinfo(b, TEX_TYPE, out pos, out rot, out tex);
			c.Ship_texture = tex;
			SendText("getname", s);
			s.Receive(b);
			c.Name = Encoding.ASCII.GetString(b);
			
		}
		
		public void ClientLoopRecieve(IAsyncResult r)
		{
			
		}
		
		public void ClientSend(IAsyncResult r)
		{
			
		}
		
		public void loop()
		{
			while(true)
			{
				foreach(Client c in clients)
				{
					Packet p = new Packet("butts", null);
					c.socket.BeginSend(p.Bytes.ToArray(), 0, p.Size, SocketFlags.None, , null);
				}
			}
		}
		
		private static void SendText(string text, Socket target){
			byte[] data = Encoding.ASCII.GetBytes (text);
			target.BeginSend (data, 0, data.Length, SocketFlags.None, new AsyncCallback (SendCall), target);
		}
		
		private static void ParseClientinfo(byte[] b, int type, out Point pos, out int rot, out Bitmap tex)
		{
			String s = Encoding.ASCII.GetString(b);
			String[] sa = s.Split(' ');
			if(type == POINT_TYPE)
			{
				String[] p = sa[1].Split(';');
				pos = new Point(float.Parse(p[0]), float.Parse(p[1]));
				rot = 0;
				tex = null;
			} else if(type == ROT_TYPE)
			{
				rot = int.Parse(sa[1]);
				pos = null;
				tex = null;
			}else if (type == TEX_TYPE)
			{
				String[] p = sa[1].Split(';');
				tex = new Bitmap(int.Parse(p[0]), int.Parse(p[1]), 0, PixelFormat.Alpha, 0);
				int x = 0;
				int y = 0;
				for(int i = 2; i < p.Length; i++)
				{
					String[] argb = p[i].Split(':');
					tex.SetPixel(x, y, Color.FromArgb(int.Parse(argb[0]),
					                                int.Parse(argb[1]),
					                                int.Parse(argb[2]),
					                                int.Parse(argb[3])));
					
					x++;
					if(x > tex.Width){
						y++;
						x = 0;
					}
				}
					
					pos = null;
					rot = 0;
				
			}
			
		}
	}
}
