/*
 * Created by SharpDevelop.
 * User: Burrito119
 * Date: 12/8/2014
 * Time: 1:18 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
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
	 	UdpClient cl_in;
	 	UdpClient cl_out;
	 	
		
	 	
	 	protected Dictionary<string, ClientObj> clients = new Dictionary<string, ClientObj>();
	 	private Vector2 pos;
	 	
	 	public NetworkObj(String IPTarget, int main_port)
		{
	 		target = new IPEndPoint(IPAddress.Parse(IPTarget), main_port);
			cl_in = new UdpClient();
			
		}
	 	
	 	public NetworkObj(long IPTarget, int main_port)
		{
	 		
			target = new IPEndPoint(new IPAddress(IPTarget), main_port);
			cl_in = new UdpClient();
		}
	 	
	 	public NetworkObj(bool local)
	 	{
	 		target = new IPEndPoint(IPAddress.Loopback, 28888);
	 		conn = new IPEndPoint(IPAddress.Loopback, 28889);
	 		cl_in = new UdpClient(target);
	 		cl_out = new UdpClient();
	 	}
		
	 	public void Connect()
	 	{
	 		Console.WriteLine("Connecting to server");
	 		cl_out.Connect(conn);
	 		cl_out.SendAsync(Encoding.ASCII.GetBytes("Burrito119"), 10);
	 		Console.WriteLine("Listening for respopnse");
	 		finishConnect();
		}
		
		private async void finishConnect()
		{
			while(true){
				UdpReceiveResult p = await cl_in.ReceiveAsync();
				String msg = Encoding.ASCII.GetString(p.Buffer);
				//Console.WriteLine("Got response from server: " + msg);
				if(msg == "sendtex"){
					Console.WriteLine("CS: Sending texture to server");
					byte[] tex = ResLoader.loadTextureFile("./res/tiles/l_hull.png");
					cl_out.Send(tex, tex.Length);
				}
				if(msg == "sendverts"){
					Console.WriteLine("CS: Sending verts to server");
					byte[] buff = Encoding.ASCII.GetBytes("0,0;4,5;6,6;1,3");
					cl_out.Send(buff, buff.Length);
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
			//Console.WriteLine("Got: "+ s);
			if(s == "sendinput"){
				byte[] buff = Encoding.ASCII.GetBytes("W_DOWN;S_UP;A_DOWN");
				cl_out.Send(buff, buff.Length);
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
				string tex = cl_parts[3].Split(':')[1];
				Console.WriteLine("Added client " + name + "with pos " + pos);
				
				clients.Add(name, ClientObj.CreateClientObj(name, ClientObj.ParseVerts(pos),Encoding.ASCII.GetBytes(tex), this));
				Console.WriteLine("Added Client");
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
				Console.WriteLine("Key: "+name);
				clients[name].Pos = ClientObj.ParseVerts(pos);
			}
			return null;
				
		}
		
		private async void loop()
		{
			
			while(true)
			{
				Console.WriteLine("Listening");
				UdpReceiveResult r = await cl_in.ReceiveAsync();
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

		public Vector2 Pos {
			get {
				return pos;
			}
			set {
				pos = value;
			}
		}
		public static ClientObj CreateClientObj(string name, Vector2 pos, byte[] texture, GameObject parent)
		{
			ClientObj co = null;
			Console.Write("Bytes: ");
			Console.WriteLine();
			//MemoryStream tex = new MemoryStream();
			//tex.Write(texture, 0, texture.Length);
			//tex.Close();
			ResLoader.WriteTempFile(name + "_texture.png", texture);
			//Console.WriteLine(tex.CanSeek);
			try{
			co = new ClientObj(name, pos, parent);
			RenderMask msk = new RenderMask(co, "t", 
			                                ResLoader.GetTextureId(new System.Drawing.Bitmap(64, 64)//"./temp/"+name+"_texture.png")  //new System.Drawing.Bitmap(tex, true)
			                                	));
			co.AddComponent(msk);
			}
			catch(Exception c)
			{
				Console.Error.WriteLine(c.Message);
				Console.Error.WriteLine(c.StackTrace);
				Console.Error.Close();
				Environment.Exit(-90);
			}
			return co;
		}
		
		public static Vector2 ParseVerts(String verts)
		{
			string[] v = verts.Split(',');
			return new Vector2(float.Parse(v[0]), float.Parse(v[1]));
		}
	}
}
