/*
 * Created by SharpDevelop.
 * User: kgauthier16
 * Date: 11/25/2014
 * Time: 12:48 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Net.Sockets;

namespace ServerParts
{
	/// <summary>
	/// Description of Client.
	/// </summary>
	public class Client
	{
		private Socket _socket;
		private Point ship_pos;
		private float ship_rot;
		private Bitmap ship_texture;
		
		public Client(Socket socket, Point ship_pos = null, float ship_rot = 0, Bitmap ship_tex = null)
		{
			_socket = socket;
			Ship_pos = ship_pos;
			Ship_rot = ship_rot;
			Ship_texture = ship_tex;
			
		}

		public Socket socket {
			get {
				return _socket;
			}
			set {
				_socket = value;
			}
		}
		public Point Ship_pos {
			get {
				return ship_pos;
			}
			set {
				ship_pos = value;
			}
		}
		public float Ship_rot {
			get {
				return ship_rot;
			}
			set {
				ship_rot = value;
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
	}
	
	public class Point{
		
		private float _x, _y;
		
		public Point(float x = 0, float y = 0)
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
		
	}
	
	public class Packet
	{
		private bool is_request;
		private String s_data;
		private Object[] o_data;
		private int size;
		
		public Packet(String s_data, Object[] o_data, bool request = false){
			this.s_data = s_data;
			this.o_data = o_data;
			this.is_request = request;
			size = sizeof(s_data) + sizeof(o_data);
		}

		public String S_data {
			get {
				return s_data;
			}
			set {
				s_data = value;
			}
		}

		public Object[] O_data {
			get {
				return o_data;
			}
			set {
				o_data = value;
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
	}
	
}
