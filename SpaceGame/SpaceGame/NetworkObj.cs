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
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SpaceGameSever.Udp;
using Util;
using Core.Graphics;
using Files;
using Game;

namespace Networking
{
	/// <summary>
	/// Description of Client.
	/// </summary>
	public class NetworkObj: Game.GameObject
	{
	 	IPEndPoint target;
	 	IPEndPoint conn;
	 	UdpClient cl;
		
	 	
	 	protected List<ClientObj> clients = new List<ClientObj>();
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
					byte[] tex = ResLoader.loadTextureFile("./res/tiles/l_hull.png");
					cl.Send(tex, tex.Length);
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
			if(s.ToLower().StartsWith("newclient"))
			{
				string[] cl_parts = s.Split(';');
				if(!cl_parts[1].StartsWith("name"))
				{
					Console.Error.WriteLine("Invalid Server Packet");
					Environment.Exit(-55);
				}
				string name = cl_parts[1].Split(':')[1];
				string pos = cl_parts[2].Split(':')[1];
			//	string tex = cl_parts[3].Split(':')[1];
				Console.WriteLine("Added client " + name + "with pos " + pos);
				
			}
			if(s.ToLower().StartsWith("update"))
			{
				string[] cl_parts = s.Split(';');
				if(!cl_parts[1].StartsWith("name"))
				{
					Console.Error.WriteLine("Invalid Server Packet");
					Environment.Exit(-54);
				}
				string name = cl_parts[1].Split(':')[1];
				string pos = cl_parts[2].Split(':')[1];
				Console.WriteLine("Updated client " + name + "with pos " + pos);
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
		
	}
	
	public class ClientObj:GameObject
	{
		private string name;
		private Vector2 pos;
		
		public ClientObj(string name, Vector2 position, GameObject parent):base(parent, parent.GetWorld())
		{
			this.name = name;
			this.pos = position;
		}

		public string Name {
			get {
				return name;
			}
			set {
				name = value;
			}
		}
		
		public static ClientObj CreateClientObj(string name, Vector2 pos, byte[] texture, GameObject parent)
		{
			ResLoader.WriteTempFile(name+"_texture.png", texture);
			ClientObj co = new ClientObj(name, pos, parent);
			RenderMask msk = new RenderMask(co, "t", 
			                                ResLoader.GetTextureId(
			                                	ResLoader.LoadImage("./temp/"+name+"_texture.png")));
			co.AddComponent(msk);
			return co;
		}
	}
}
