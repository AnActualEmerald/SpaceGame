/*
 * Created by SharpDevelop.
 * User: Burrito119
 * Date: 3/23/2015
 * Time: 10:28 AM
 * 
 */
using System;
using System.Net.Sockets;
using System.Net;
using FarseerPhysics.Common;
using Microsoft.Xna.Framework;

namespace SpaceGameSever
{
	/// <summary>
	/// Client object for containing info on clients that connect to the server
	/// </summary>
	public class Client
	{
		string name;
		IPEndPoint remoteEP;
		Vector2 pos;
		byte[] tex_data;
		Vertices verts;
		
		/// <summary>
		/// Creates Client object but does not initialize all the variables
		/// </summary>
		/// <param name="remoteEP">The IPEndPoint that the Client connected from</param>
		/// <param name="name">The name that the Client sent when it connected. Defaults to "non"</param>
		public Client(IPEndPoint remoteEP, string name = "non")
		{
			this.name = name;
			this.remoteEP = remoteEP;
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
				return remoteEP;
			}
			set {
				remoteEP = value;
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

		public byte[] Tex_data {
			get {
				return tex_data;
			}
			set {
				tex_data = value;
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
		public Client Init(byte[] texture_data, Vertices verts, Vector2 pos)
		{
			this.tex_data = texture_data;
			this.verts = verts;
			this.pos = pos;
			return this;
		}
		
		#endregion
		
	}
}
