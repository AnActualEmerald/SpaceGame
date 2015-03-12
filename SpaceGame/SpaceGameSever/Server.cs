/*
 * Created by SharpDevelop.
 * User: Burrito119
 * Date: 11/25/2014
 * Time: 11:57 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using Microsoft.Xna.Framework;
using ServerParts;
using System.Drawing;
using System.Drawing.Imaging;
using FarseerPhysics.Common;

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
		public const int VERTS_TYPE = 0x03;
		
		private byte[] _buffer = new byte[5120];
		private ServerWorld _world = new ServerWorld(new S_Point(0, 0));
		
		public void AcceptCall(IAsyncResult r)
		{
			Socket s = _serverSocket.EndAccept(r);
			Client c = new Client(s);
			GetClientInfo(s, c);
			clients.Add(c);
			
			s.BeginReceive(c.B_buffer, 0, c.B_buffer.Length, SocketFlags.None, new AsyncCallback(ClientLoopRecieve), s);
			_serverSocket.BeginAccept(new AsyncCallback(AcceptCall), null);
		}

		public void InitClient(Client c)
		{
			String response;
			
			foreach(Client cc in clients)
			{			
				SendText("new_c", c.socket);
				WaitForResponse("ready", c.socket);
				SendText(cc.ToString(), c.socket);
				WaitForResponse("got_c", c.socket);
				SendText("next_texture", c.socket);
				WaitForResponse("ready", c.socket);
				SendBytes(cc.GetTextureBytes(), c.socket);
				WaitForResponse("got_tex", c.socket);
			}
		}
		
		public void UpdateClients()
		{
			foreach(Client c in clients)
				foreach(Client cc in clients)
				{							
					SendText(cc.ToString(), c.socket);
				}	
		}
		
		public String WaitForResponse(String res, Socket target)
		{
			byte[] buff = new byte[res.Length * sizeof(byte)];
			target.Receive(buff);
			String got = Encoding.ASCII.GetString(buff);
			if(got.Contains(res))
				return got;
			
			return null;
		}
		
		public void SendBytes(byte[] data, Socket target)
		{
			target.BeginSend (data, 0, data.Length, SocketFlags.None, new AsyncCallback (ClientSendCall), target);
		}
		
		public void GetClientInfo(Socket s, Client c)
		{
			SendText("getpos", s);
			byte[] b = new byte[2048];
			S_Point pos;
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
			SendText("getverts", s);
			s.Receive(b);
			S_Point[] p;
			ParseShipVerts(b, out p);
			c.Ship_verts = p;
			SendText("start", s);
		}
		
		public void ClientLoopRecieve(IAsyncResult r)
		{
			try{
				Socket s = (Socket)r.AsyncState;
				Client c = GetClientBySocket(s);
				int i = s.EndReceive(r);
				byte[] d = new byte[i];
				Array.Copy(c.B_buffer, d, i);
				
				
				
				s.BeginReceive(c.B_buffer, 0, c.B_buffer.Length, SocketFlags.None, new AsyncCallback(ClientLoopRecieve), s);
			}catch(SocketException e){
				Console.WriteLine("Client disconnect");
			}
		}
		
		public void ParseClientInput()
		{
			foreach(Client c in clients)
			{
				String[] s = Encoding.ASCII.GetString(c.B_buffer).Split(';');
				String[] ss = s[0].Split(',');
				foreach(String key in ss)
				{
					if(key.Equals("w"))
						c.SetKeyState(Client.KEY_W, true);
					if(key.Equals("a"))
						c.SetKeyState(Client.KEY_A, true);
					if(key.Equals("s"))
						c.SetKeyState(Client.KEY_S, true);
					if(key.Equals("d"))
						c.SetKeyState(Client.KEY_D, true);
					if(key.Equals("shift"))
						c.SetKeyState(Client.KEY_LSHIFT, true);
				}
				
				ss = s[1].Split(',');
				foreach(String key in ss)
				{
					if(key.Equals("w"))
						c.SetKeyState(Client.KEY_W, false);
					if(key.Equals("a"))
						c.SetKeyState(Client.KEY_A, false);
					if(key.Equals("s"))
						c.SetKeyState(Client.KEY_S, false);
					if(key.Equals("d"))
						c.SetKeyState(Client.KEY_D, false);
					if(key.Equals("shift"))
						c.SetKeyState(Client.KEY_LSHIFT, false);
				}
			}
		}
		
		public static void ClientSendCall(IAsyncResult r)
		{
			Socket s = (Socket)r.AsyncState;
			s.EndSend (r);
		}
		
		public void loop()
		{
			//accept new clients in initialize them
			_serverSocket.Bind();
			_serverSocket.BeginAccept(new AsyncCallback(AcceptCall), null);
			while(true)
			{
				//take client intput
				//handled in async clinet loop
				//step physics 
				Update();
				//send results to clients
				UpdateClients();
				//???
				//profit		
			}
		}
		
		public void Update()
		{
			Vector2 force = new Vector2();
			
			foreach(Client c in clients)
			{
				if(c.GetKeyState(Client.KEY_W))
					force.Y = -5;
				if(c.GetKeyState(Client.KEY_A))
					force.X = -5;
				if(c.GetKeyState(Client.KEY_D))
					force.X = 5;
				if(c.GetKeyState(Client.KEY_S))
					force.Y = 5;
				
				c.Ship_body.ApplyForce(ref force);
			}
			
			_world.step();
		}
		
		private static void SendText(string text, Socket target){
			byte[] data = Encoding.ASCII.GetBytes (text);
			target.BeginSend (data, 0, data.Length, SocketFlags.None, new AsyncCallback (ClientSendCall), target);
		}
		
		private void ParseClientinfo(byte[] b, int type, out S_Point pos, out int rot, out Bitmap tex)
		{
			String s = Encoding.ASCII.GetString(b);
			String[] sa = s.Split(' ');
			if(type == POINT_TYPE)
			{
				String[] p = sa[1].Split(';');
				pos = new S_Point(float.Parse(p[0]), float.Parse(p[1]));
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
				tex = new Bitmap(int.Parse(p[0]), int.Parse(p[1]), 0, PixelFormat.Alpha, IntPtr.Zero);
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
				
			}else
			{
				pos = null;
				rot = 0;
				tex = null;
			}
			
		}
		
		public void ParseShipVerts(byte[] b, out S_Point[] verts)
		{
			String s = Encoding.ASCII.GetString(b);
			String[] v = s.Split(';');
			List<S_Point> points = new List<S_Point>();
			
			foreach(String vert in v)
			{
				String[] xy = vert.Split(':');
				points.Add(new S_Point(_world.ConvertToSim(float.Parse(xy[0])), _world.ConvertToSim(float.Parse(xy[1]))));
			}
			
			verts = points.ToArray();
		}
		
		public Client GetClientBySocket(Socket s)
		{
			foreach(Client c in clients)
				if(c.socket.RemoteEndPoint.Equals(s.RemoteEndPoint))
					return c;
			
			return null;
		}
	}
}
