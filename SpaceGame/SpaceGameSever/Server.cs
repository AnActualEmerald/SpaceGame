/*
 * Created by SharpDevelop.
 * User: Burrito119
 * Date: 3/19/2015
 * Time: 1:37 PM
 * 
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Util;
using FarseerPhysics.Common;
using Microsoft.Xna.Framework;
using SpaceGameSever.Udp;
using System.Net;

namespace SpaceGameSever
{
	/// <summary>
	/// Description of Server.
	/// </summary>
	public class Server
	{
		#region private members
		private UdpListner _server;
		private UdpListner _connecter;
		private Thread connect_thread;
		private bool running = false;
		private Vector2 spawn_pos;
		#endregion
		
		public List<Client> clients = new List<Client>();
		
		/// <summary>
		/// Creates Server object and readies it for use
		/// </summary>
		/// <param name="toStart">Switch to decide if the Server should start once constructed</param>
		public Server(bool toStart = false)
		{
			//TODO finish constructor implementation
			_server = new UdpListner(IPAddress.Any, 28889, false);
			connect_thread = new Thread(new ThreadStart(CheckForConn));
			if(toStart)
				Start();
		}
		
		public void Start()
		{
			Console.WriteLine("Server starting...");
			running = true;
			long lastTime = Utilities.GetTimeMillis(DateTime.Now);
			long timeNow;
			float delta = 0.0f;
			int tps = 0;
			long epsilon = Utilities.GetTimeMillis(DateTime.Now);
			long update_now;
			connect_thread.Start();
			while(running)
			{
				timeNow = Utilities.GetTimeMillis(DateTime.Now);
				delta += timeNow - lastTime;
				lastTime = timeNow;
				//get info from clients(input, misc)
				update_now = Utilities.GetTimeMillis(DateTime.Now);
				
				while(epsilon < update_now){
					Input();
					//step physics
					//update clients
					Update();
					epsilon+=1000/60;
					tps++;
				}
				
				if(delta >= 1000)
				{
					Console.WriteLine("TPS: "+tps);
					tps = 0;
					delta = 0;
				}
			}
		}
		
		private async void CheckForConn()
		{
			Console.WriteLine("Connection thread started succesfully");
				Console.WriteLine("Listening for packet...");
				Packet r = await _server.Listen();
				Console.WriteLine("SS: packet received");
				Client c = new Client(r.endpoint, r.message);
				InitClient(c);
		}
		
		/// <summary>
		/// Will initialize and add client to server
		/// </summary>
		/// <param name="c">The client object that needs to be initialized</param>
		private async void InitClient(Client c)
		{
			byte[] tex;
			Vertices verts;
			_server.Send("sendtex", c.RemoteEP);
			Packet r = await _server.ListenTo(c.RemoteEP);
			tex = Encoding.ASCII.GetBytes(r.message);
			_server.Send("sendverts", c.RemoteEP);
			r = await _server.ListenTo(c.RemoteEP);
			verts = ParseClientVerts(r.message);
			_server.Send("start", c.RemoteEP);
			c.Init(tex, verts, spawn_pos);
			clients.Add(c);
		}
		
		private async Task<Packet> GetPacketFrom(IPEndPoint ep)
		{	
			do{
				Packet p = await _server.Listen();
				if(p.endpoint == ep)
					return p;
			}while(true);
		}
		
		private Vertices ParseClientVerts(String vertstring)
		{
			Vertices v = new Vertices();
			String[] points = vertstring.Split(';');
			foreach(String p in points)
			{
				String[] xy = p.Split(',');
				v.Add(new Vector2(float.Parse(xy[0]), float.Parse(xy[1])));
			}
			
			return v;
		}
		
		private void Input()
		{
			
			foreach(Client c in clients)
			{
				_server.Send("sendinput", c.RemoteEP);	
				Packet p = _server.ListenToSync(c.RemoteEP);
				ProcessClientInput(p, c);
			}
		}
		
		private void ProcessClientInput(Packet p, Client c)
		{
			String[] msg = p.message.Split(';');
			for(int i = 0; i < msg.Length; i++)
			{
				switch (msg[i]) {
					case "W_DOWN":
						c.input[(int)Inputs.W] = true;
						continue;
					case "W_UP":
						c.input[(int)Inputs.W] = false;
						continue;
					case "A_DOWN":
						c.input[(int)Inputs.A] = true;
						continue;
					case "A_UP":
						c.input[(int)Inputs.A] = false;
						continue;
					case "S_DOWN":
						c.input[(int)Inputs.S] = true;
						continue;
					case "S_UP":
						c.input[(int)Inputs.S] = false;
						continue;
					case "D_DOWN":
						c.input[(int)Inputs.D] = true;
						continue;
					case "D_UP":
						c.input[(int)Inputs.D] = false;
						continue;
					case "L_SHIFT_DOWN":
						c.input[(int)Inputs.L_SHIFT] = true;
						continue;
					case "L_SHIFT_UP":
						c.input[(int)Inputs.L_SHIFT] = false;
						continue;
					case "SPACE_DOWN":
						c.input[(int)Inputs.SPACE] = true;
						continue;
					case "SPACE_UP":
						c.input[(int)Inputs.SPACE] = false;
						continue;
					default:
						Console.WriteLine("Unkown input");
						break;
				}
			}
		}
		
		private void Update()
		{
		}
		

	}
}
