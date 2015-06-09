/*
 * Created by SharpDevelop.
 * User: Burrito119
 * Date: 3/23/2015
 * Time: 10:28 AM
 * 
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Text;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Common.TextureTools;
using Microsoft.Xna.Framework;

namespace SpaceGameSever
{
	/// <summary>
	/// Client object for containing info on clients that connect to the server
	/// </summary>
	public class Client
	{
		string name;
		IPEndPoint ep_out;
		IPEndPoint ep_in;
		Vector2 pos;
		byte[] tex_data;
		Vertices verts;
		Shape shipshape;
		Bitmap texture;
		float rot;
		
		public bool[] input = new bool[6];
		
		
		/// <summary>
		/// Creates Client object but does not initialize all the variables
		/// </summary>
		/// <param name="remoteEP">The IPEndPoint that the Client connected from</param>
		/// <param name="name">The name that the Client sent when it connected. Defaults to "non"</param>
		public Client(IPEndPoint remoteEP, string name = "non")
		{
			this.name = name;
			this.ep_out = remoteEP;
		}
				
		#region properties
		
		/// <summary>
		/// The name of the client that connected to server
		/// </summary>
		public string Name {
			get {
				return name;
			}
			set {
				name = value;
			}
		}

		/// <summary>
		/// The IPEndPoint that the client connected from
		/// </summary>
		public IPEndPoint RemoteEP {
			get {
				return ep_out;
			}
			set {
				ep_out = value;
			}
		}

		public IPEndPoint Ep_in {
			get {
				return ep_in;
			}
			set {
				ep_in = value;
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

		public float Rot {
			get {
				return rot;
			}
			set {
				rot = value;
			}
		}
		public byte[] Tex_data {
			get {
				return tex_data;
			}
			set {
				tex_data = value;
			}
		}
		
		public Shape Ship_Shape
		{
			get{
				return shipshape;
			}
		}

		public Vertices Verts {
			get {
				return verts;
			}
			set {
				verts = value;
			}
		}
		#endregion
		
		#region methods
		
		/// <summary>
		/// Initializes client object and returns that object for use. Does modify original object
		/// </summary>
		/// <returns>Copy of initialized client object</returns>
		public Client Init(byte[] texture_data, float rotation, Vector2 pos)
		{
			this.tex_data = texture_data;
			this.pos = pos;
			this.rot = rotation;
			texture = new Bitmap(new System.IO.MemoryStream(texture_data));
			return this;
		}
		
		public Client loadVertsFromTexture(){
			List<uint> tmp = new List<uint>();
			//int num_bytes = 0;
			List<byte> int_to_be = new List<byte>();
			MemoryStream mem = new MemoryStream();
			texture.Save(mem, ImageFormat.Bmp);
			byte[] bits = mem.ToArray();//(byte[]) new ImageConverter().ConvertTo(texture, typeof(byte[]));
			Console.WriteLine(bits.Length);
			for(int i = 1; i <= bits.Length; i++)
			{
				if(int_to_be.Count == 4){
					tmp.Add(BitConverter.ToUInt32(int_to_be.ToArray(), 0));
					int_to_be.Clear();
				}
				int_to_be.Add(bits[i-1]);
											
			}
			Vertices v;
			Console.WriteLine("TMP LEN: " + tmp.Count);
			Console.ReadLine();
			v = TextureConverter.DetectVertices(tmp.ToArray(), 64);
			verts = v;
			shipshape = new PolygonShape(v, 1.0f);
			return this;
		}
		
		#endregion
		
	}
	
	public enum Inputs
	{
		W = 0,
		A = 1,
		S = 2,
		D = 3,
		L_SHIFT = 4,
		SPACE = 5
	}
}
