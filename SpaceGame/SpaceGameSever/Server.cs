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
			_server = new UdpListner(IPAddress.Any, 25565, false);
			_connecter = new UdpListner(IPAddress.Any, 25566, false);
			connect_thread = new Thread(new ThreadStart(CheckForConn));
			if(toStart)
				Start();
		}
		
		public void Start()
		{
			Console.WriteLine("Server starting...");
			running = true;
			connect_thread.Start();
			long lastTime = Utilities.GetTimeMillis(DateTime.Now);
			long timeNow;
			float delta = 0.0f;
			int tps = 0;
			long epsilon = Utilities.GetTimeMillis(DateTime.Now);
			long update_now;
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
			while(running){
				Console.WriteLine("Listening for packet...");
				Packet r = await _connecter.ListenAsync();
				Console.WriteLine("SS: packet received");
				Client c = new Client(r.sender, r.message);
				InitClient(c);
			}
		}
		
		/// <summary>
		/// Will initialize and add client to server
		/// </summary>
		/// <param name="c">The client object that needs to be initialized</param>
		private async void InitClient(Client c)
		{
			byte[] tex;
			Vertices verts;
			await _connecter.Send("sendtex", c.RemoteEP);
			Packet r = await _connecter.ListenTo(c.RemoteEP);
			tex = Encoding.ASCII.GetBytes(r.message);
			await _connecter.Send("sendverts", c.RemoteEP);
			r = await _connecter.ListenTo(c.RemoteEP);
			verts = ParseClientVerts(r.message);
			await _connecter.Send("start", c.RemoteEP);
			c.Init(tex, verts, spawn_pos);
			clients.Add(c);
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
			//TODO async?
			foreach(Client c in clients)
			{
				_server.BeginSend("sendinput", c.RemoteEP, new AsyncCallback(SendCall));	
				_server.BeginListenTo(new AsyncCallback(InputCall));
			}
		}
		
		private void Update()
		{
			
		}
		
		private void SendCall(IAsyncResult r)
		{
			_server.EndSend(r);
		}
		
		private void InputCall(IAsyncResult r)
		{
			IPEndPoint remoteEP = r.AsyncState as IPEndPoint;
			Packet rec = _server.EndListenTo(r);
		}
	}
}
