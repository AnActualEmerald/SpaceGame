/*
 * Created by SharpDevelop.
 * User: Burrito119
 * Date: 11/25/2014
 * Time: 12:48 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Sockets;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace ServerParts
{
	/// <summary>
	/// Description of Client.
	/// </summary>
	public class Client
	{
		public const int KEY_W = 0, KEY_A = 1, KEY_S = 2, KEY_D = 3, KEY_LSHIFT = 4;
		
		private Socket _socket;
		private S_Point ship_pos;
		private float ship_rot;
		private Bitmap ship_texture;
		private String name;
		private PolygonShape ship_shape;
		private Body ship_body;
		private S_Point[] ship_verts;
		private byte[] b_buffer;
		
		
		private bool[] keys = new bool[5];
		
		public Client(Socket socket, String name = null, S_Point ship_pos = null, float ship_rot = 0, Bitmap ship_tex = null)
		{
			_socket = socket;
			Ship_pos = ship_pos;
			Ship_rot = ship_rot;
			Ship_texture = ship_tex;
			this.name = name;
			
			Vector2[] v = new Vector2[ship_verts.Length];
			for(int i = 0; i < v.Length; i++)
			{
				v[i] = ship_verts[1].AsXNAVector();
			}
			
			Vertices vs = new Vertices(v);
			
			ship_shape = new PolygonShape(vs, 1);
			ship_body.Rotation = ship_rot;
			ship_body.Position = ship_pos.AsXNAVector();
		}

		public byte[] GetTextureBytes()
		{
			byte[] bb;
			
			List<byte> c = new List<byte>();
			for(int x  = 0; x < Ship_texture.Width; x++)
				for(int y = 0; y < Ship_texture.Height; y++)
			{
				c.Add((byte)Ship_texture.GetPixel(x,y).ToArgb());
			}
			
			bb = c.ToArray();
			return bb;
		}

		public bool GetKeyState(int index){
			return keys[index];
		}
		
		public void SetKeyState(int index, bool state)
		{
			keys[index] = state;
		}
		
		public Socket socket {
			get {
				return _socket;
			}
			set {
				_socket = value;
			}
		}

		public byte[] B_buffer {
			get {
				return b_buffer;
			}
			set {
				b_buffer = value;
			}
		}

		public S_Point Ship_pos {
			get {
				return ship_pos;
			}
			set {
				ship_pos = value;
			}
		}
		public float Ship_rot {
			get {
				return ship_body.Rotation;
			}
			set {
				ship_body.Rotation = value;
			}
		}
		public Bitmap Ship_texture {
			get {
				return ship_texture;
			}
			set {
				ship_texture = value;
			}
		}

		public String Name {
			get {
				return name;
			}
			set {
				name = value;
			}
		}

		public ServerParts.S_Point[] Ship_verts {
			get{ return ship_verts;}
			set{ ship_verts = value;}
		}

		public PolygonShape Ship_shape {
			get {
				return ship_shape;
			}
			set {
				ship_shape = value;
			}
		}

		public Body Ship_body {
			get {
				return ship_body;
			}
			set {
				ship_body = value;
			}
		}
		
		public override string ToString()
		{
			String res = "name:"+name+";pos:"+ship_pos.x+","+ship_pos.y+";rot:"+ship_rot+";";
			return res;
		}

	}
	
	public class S_Point{
		
		private float _x, _y;
		
		public S_Point(float x = 0, float y = 0)
		{
			this.x = x;
			this.y = y;
		}
		
		public float x{
			get{return _x;}
			set{_x = value;}
		}
		
		public float y{
			get{return _y;}
			set{_y = value;}
		}
		
		public Vector2 AsXNAVector()
		{
			return new Vector2(_x, _y);
		}
		
		
	}
	
	public class Packet
	{
		private bool is_request;
		private String s_data;
		private byte[] b_data;
		private List<byte> bytes = new List<byte>();
		private int size;
		
		public Packet(String s_data, byte[] b_data, bool request = false){
			this.s_data = s_data;
			this.b_data = b_data;
			this.is_request = request;
			
			
			foreach(byte b in b_data)
			{
				bytes.Add(b);
			}
			
			bytes.Add(byte.Parse(s_data));
			size = sizeof(byte) * bytes.Count;
			
		}

		public String S_data {
			get {
				return s_data;
			}
			set {
				s_data = value;
			}
		}

		public Byte[] B_data {
			get {
				return b_data;
			}
			set {
				b_data = value;
			}
		}

		public bool Is_request {
			get {
				return is_request;
			}
			set {
				is_request = value;
			}
		}
		public int Size {
			get {
				return size;
			}
			set {
				size = value;
			}
		}

		public List<byte> Bytes {
			get {
				return bytes;
			}
			set {
				bytes = value;
			}
		}
	}
	
}
