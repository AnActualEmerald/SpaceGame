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
using FarseerPhysics.Common;
using Microsoft.Xna.Framework;

namespace SpaceGameSever
{
	/// <summary>
	/// Description of Server.
	/// </summary>
	public class Server
	{
		#region private members
		private UdpListener _server;
		private UdpListener _connecter;
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
			_server = new UdpListener();
			_connecter = new UdpListener();
			connect_thread = new Thread(new ThreadStart(CheckForConn, 0));
			if(toStart)
				Start();
		}
		
		public void Start()
		{
			running = true;
			connect_thread.Start();
			while(running)
			{
				//get info from clients(input, misc)
				//step physics
				//update clients
			}
		}
		
		private async void CheckForConn()
		{
			while(running){
				Recieved r = await _connecter.ListenAsync();
				Client c = new Client(r.Sender, r.message);
				InitClient(ref c);
				clients.Add(c);
			}
		}
		
		private async void InitClient(ref Client c)
		{
			byte[] tex;
			Vertices verts;
			await _connecter.SendAsync(c.RemoteEP, "sendtex");
			Recieved r = await _connecter.ListenToAsync();
			tex = Encoding.ASCII.GetBytes(r.message);
			await _connecter.SendAsync(c.RemoteEP, "sendverts");
			r = await _connecter.ListenToAsync();
			verts = ParseClientVerts(r.message);
			c.Init(tex, verts, spawn_pos);
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
			//send request for input
			//recieve and handle response
		}
		
		private void Update()
		{
			
		}
	}
}
