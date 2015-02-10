/*
 * Created by SharpDevelop.
 * User: kgauthier16
 * Date: 11/18/2014
 * Time: 9:02 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Core.Graphics;
using Game;
using Microsoft.Xna.Framework;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using System.Collections.Generic;
using System.Drawing;

namespace ShipBuild
{
	/// <summary>
	/// Description of TileBasic.
	/// </summary>
	public class TileBasic : GameObject
	{
	
		protected String _name;
		protected int _data;
		protected Vector2 inship_pos;
		protected RenderMask mask;
		protected Ship parentShip;
		protected Shape _shape;
		protected Vertices _verts;
		protected Bitmap texture;

		public int id;
		
		public TileBasic(String name, Ship parent) : base(parent, parent.GetWorld())
		{
			this.name = name;
			this.parentShip = parent;
			
			try{
				texture = Files.ResLoader.LoadImage("./res/tiles/" + name + ".png");
				id = Files.ResLoader.GetTextureId(texture);
			}catch(Exception c){
				Console.Error.WriteLine("Tile ini failed for " + name + "... error loading texture?");
				Console.Error.WriteLine(c.Message);
				Console.Error.WriteLine(c.StackTrace);
				Console.Error.WriteLine("Can't have missing textures 'round here, exiting program...");
				Console.Error.Flush ();
				Environment.Exit(-18);
			}
			
			this.mask = new RenderMask(this, "t", id);
			//AddComponent (mask);

			Console.WriteLine (name + " for ship " + parentShip.Name + " was loaded");
		}

		public Bitmap Texture {
			get {
				return texture;
			}
		}
		
		public virtual int data{
			
			get { return _data; }
			set { _data = value; }
		}
		
		public virtual String name{
			get { return _name; }
			set { _name = value; }
		}

		public virtual Vector2 pos {
			get {
				return inship_pos;
			}
			set {
				inship_pos = value;
			}
		}

		public virtual Shape shape {
			get {
				return _shape;
			}
			set {
				_shape = value;
			}
		}

		public Vertices verts {
			get {
				return _verts;
			}
			set {
				_verts = value;
			}
		}

		public void Rotate (double s)
		{

		//	Bitmap b = new Bitmap (64, 64);
		//	System.Drawing.Graphics g = System.Drawing.Graphics.FromImage (b);
		//	g.TranslateTransform((float)texture.Width / 2, (float)texture.Height / 2);
		//	g.RotateTransform(s);
		//	g.TranslateTransform(-(float)texture.Width / 2, -(float)texture.Height / 2);
		//	g.DrawImage(texture, new Point(0, 0));

		//	texture.Dispose ();
		//	texture = b;

		//	mask.SetTextureId (Files.ResLoader.GetTextureId (texture));

			float xx;
			float yy;
			for(int i = 0; i < verts.Count; i++) {
				Vector2 v = verts.ToArray () [i];
				xx = (float)(v.X * Math.Cos (s) - v.Y * Math.Sin (s));
				yy = (float)(v.X * Math.Sin (s) + v.Y * Math.Cos (s));
				v.X = xx;
				v.Y = yy;
			}

		}
					
		public override void Update()
		{
			mask.SetVerts (verts);	
			base.Update ();
		}
		
		public override void Input()
		{
			
		}
		
		public override void init()
		{
			base.init ();
			Vertices v = new Vertices (new Vector2[] {
				new Vector2 (pos.X * 64, pos.Y * 64), 
				new Vector2 (pos.X * 64 + 64, pos.Y * 64), 
				new Vector2 (pos.X * 64 + 64, pos.Y * 64 + 64), 
				new Vector2 (pos.X * 64, pos.Y * 64 + 64)
			});
			_shape = new PolygonShape (v, 1);
			_verts = v;

		}

	}
}
