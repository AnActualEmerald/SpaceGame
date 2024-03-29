﻿/*
 * Created by SharpDevelop.
 * User: Burrito119
 * Date: 3/19/2015
 * Time: 1:37 PM
 * 
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
		private UdpListner _server_out;
		private UdpListner _server_in;
		private Thread connect_thread;
		private bool running = false;
		private Vector2 spawn_pos;
		private PhysicsWorld world;
		#endregion
		
		public List<Client> clients = new List<Client>();
		
		/// <summary>
		/// Creates Server object and readies it for use
		/// </summary>
		/// <param name="toStart">Switch to decide if the Server should start once constructed</param>
		public Server(bool toStart = false)
		{
			//TODO finish constructor implementation
			_server_in = new UdpListner(IPAddress.Any, 28889, false);
			_server_out = new UdpListner(28888);
			_server_out.SendTimeout = 5500;
			_server_in.RecTimeout = 5500;
			connect_thread = new Thread(new ThreadStart(CheckForConn));
			world = new PhysicsWorld(new Vector2(0,0), 64);
            FarseerPhysics.Settings.MaxPolygonVertices = 128;
            Console.WriteLine("MAX VERTS: "+ FarseerPhysics.Settings.MaxPolygonVertices);
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
				Packet r = await _server_in.Listen();
				Console.WriteLine("SS: packet received");
				Client c = new Client(r.endpoint, Encoding.ASCII.GetString((byte[])r.message));
				c.Ep_in = new IPEndPoint(r.endpoint.Address, 28888);
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
			_server_out.Send("sendtex", c.Ep_in);
			Packet r = await _server_in.ListenTo(c.RemoteEP);
			tex = (byte[])r.message;
			_server_out.Send("start", c.Ep_in);
			c.Init(tex, 0, spawn_pos);
            Bitmap bit = new Bitmap(new MemoryStream(tex));
            Console.WriteLine("Got all the stuff for "+c.Name+", tex width " +bit.Width);
			try{
		    	c.loadVertsFromTexture();
			}catch(Exception e)
			{
				Console.WriteLine(e.Message);
				Console.WriteLine(e.StackTrace);
				Console.ReadLine();
			}
            Console.WriteLine("Done init of client: " + c.Name);
            world.AddClient(c);
            clients.Add(c);
			SendClientToAll(c, true);
		}
		
		private void SendClientToAll(Client c, bool client_new = false)
		{
			string clstring = client_new ? "newclient" : "update";
			Packet n_client = new Packet{
				endpoint = c.Ep_in,
				message = clstring+";name:" + c.Name
					+";pos:"+c.Pos.X +","
					+c.Pos.Y +";rot:"+c.Rot
			};
			
			
			Packet texture = new Packet{
				endpoint = c.Ep_in,
				message = c.Tex_data
			};
			
			Bitmap bit = new Bitmap(new MemoryStream((byte[])texture.message));
			Console.WriteLine("Still fine");
			
			foreach(Client cc in clients)
			{
				n_client.endpoint = cc.Ep_in;
				_server_out.Send(n_client);
				
				if(client_new)
				{			
					texture.endpoint = cc.Ep_in;
					_server_out.Send(texture);
				}
			}
			Console.WriteLine("Done Client Send");
		}
		
		private async Task<Packet> GetPacketFrom(IPEndPoint ep)
		{	
			do{
				Packet p = await _server_in.Listen();
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
		//	Console.WriteLine("Asking for input");
			foreach(Client c in clients)
			{
               // Console.WriteLine("Asking for input for: " + c.Name);
				_server_out.Send("sendinput", c.Ep_in);	
				Packet p = _server_in.ListenToSync(c.RemoteEP);
				ProcessClientInput(p, c);
			}
		}
		
		private void ProcessClientInput(Packet p, Client c)
		{
			String[] msg = (Encoding.ASCII.GetString((byte[])p.message)).Split(';');
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
          //  Console.WriteLine("Update start...");
			//step physics
			world.Step();
			
			foreach(Client c in clients){
				c.Rot = world.GetBody(c.Name).Rotation;
				c.Pos = world.GetBody(c.Name).Position;
			}
			
			//update clients
			for(int i = 0; i < clients.Count; i++){
				//Console.WriteLine("Updating Clients");
				SendClientToAll(clients[i]);
			}

         //   Console.WriteLine("Update end");
		}
		

	}
}
