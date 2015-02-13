/*
 * Created by SharpDevelop.
 * User: kgauthier16
 * Date: 11/18/2014
 * Time: 9:11 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FileManager;
using Game;
using Microsoft.Xna.Framework;
using FarseerPhysics.Factories;
using Core.Graphics;
using System.Drawing;
using FarseerPhysics.Collision;
using Files;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace ShipBuild
{
	/// <summary>
	/// Description of Ship.
	/// </summary>
	public class Ship : GameObject
	{
		private DataFile shipFile;
		private List<TileData> tiles = new List<TileData>();
		private List<TileBasic> render_tiles = new List<TileBasic>();
		private PhysicsBody body;
		private Fixture s_fixture;
		private RenderMask ship_mask;
		private Bitmap texture;

		private int max_thrust;
		private int ship_tex_id;
		private string name;
		private Matrix4 m_rot = Matrix4.CreateRotationZ(0);
		private Matrix4 m_scale = Matrix4.CreateScale(1);
		private Matrix4 m_trans = Matrix4.CreateTranslation (0, 0, 0);
		private OpenTK.Vector2 center;


		public Ship(String name, Core.CoreEngine world) : base(world.root, world)
		{
			this.name = name;
			shipFile = new DataFile("./ships/" + name + ".shp");		
			String s;
			if(shipFile.ReadFile(out s))
				shipFile.Parse(s, out tiles);
			else
				shipFile.SaveTileData(new TileData(), false);	
			
			render_tiles = buildShip(tiles);
			init_thrust(render_tiles);

		}

		public string Name {
			get{ return name;}
			set{ name = value;}
		}

		protected Bitmap init_mask (List<TileBasic> t, out Vertices v)
		{

			v = new Vertices ();
			if (t == null) {
				Console.Error.WriteLine ("HALP!!! Why is t null??");
				Console.Error.Flush ();
				Console.WriteLine ("No but really!");
			}
				

			foreach (TileBasic tb in t) 
			{
				if (tb.verts == null)
					continue;
				v.AddRange (tb.verts);
			}

			AABB aabb = v.GetAABB ();
			Bitmap im = new Bitmap ((int)aabb.Width, (int)aabb.Height);
			Console.WriteLine ("Im stuff = " + im.Width + ", " + im.Height);
			foreach (TileBasic tb in t) 
			{
				Bitmap tbx = tb.Texture;
				for (int x = 0; x < tbx.Width; x++)
					for (int y = 0; y < tbx.Height; y++)
					{
						Color c = tbx.GetPixel (x, y);
	
						im.SetPixel ((int)(x + tb.pos.X * 64), (int)(y + tb.pos.Y * 64), c);
					}
			}

			return im;
		}

		protected void init_thrust(List<TileBasic> t)
		{
			foreach(TileBasic tb in t)
			{
				if(tb.name == "s_thrust" || tb.name == "l_thrust")
					max_thrust += tb.data;
			}
		}

		private List<TileBasic> buildShip(List<TileData> _tiles)
		{

			Console.WriteLine ("len = " + _tiles.Count);
			List<TileBasic> cc = new List<TileBasic>();
			
			foreach(TileData td in _tiles)
			{
				TileBasic tb = AddTile (td);
				cc.Add(tb);
				AddChild (tb);
			}

			return cc;
		}
		
		private TileBasic AddTile(TileData t)
		{
			TileBasic tb = new TileBasic((String)t.GetProperty("name"), this);
			Object v = (String)t.GetProperty("vert");
			String[] xy = ((String)v).Split(';');
			tb.pos = new Microsoft.Xna.Framework.Vector2(float.Parse(xy[0]), float.Parse(xy[1]));
			if (tb.name != "s_thrust" || tb.name != "l_thrust") {
				tb.data = 0;
				return tb;
			}
			v = t.GetProperty("data");
			tb.data = (int)v;
			
			return tb;
		}

		private void Rotate(float a, OpenTK.Vector2 around)
		{
			m_rot = Matrix4.CreateRotationZ (a);
		}

		private void Translate(float off_x, float off_y)
		{
			m_trans = Matrix4.CreateTranslation (off_x, off_y, 0);
		}

		private void Scale(float sx, float sy)
		{
			m_scale = Matrix4.CreateScale(sx, sy, 1);
		}
			
		public override void init ()
		{

			base.init ();
			body = new PhysicsBody(world.GetWorld(), 
				new Microsoft.Xna.Framework.Vector2(0, 0), 
				0, null, this);

			foreach (TileBasic tb in render_tiles) {
				FixtureFactory.AttachPolygon (tb.verts, 1, body.body);
			}
			Vertices v;
			texture = init_mask (render_tiles, out v);
			ship_mask = new RenderMask(this, "t", ResLoader.GetTextureId(texture));
			ship_mask.SetVerts (v.GetAABB().Vertices);
			ship_mask.init ();
			AddComponent (ship_mask);
			AddComponent(body);

			center = new OpenTK.Vector2 (v.GetCentroid ().X, v.GetCentroid ().Y);
		}

		float i = 0.0f;
		public override void Update ()
		{
			base.Update ();
			Rotate ((float)(i), center);
			Translate (100, 0);
			//Scale (0.5f * i, 0.5f * i);
			i+=0.01f;

			Matrix4 mod = m_scale * m_rot * m_trans;
			Matrix4 m = mod * Matrix4.CreateOrthographic (world.GetHorRes (), world.GetVertRes (), -1, 1);
			GL.UniformMatrix4 (MainClass.mod_matUniform, false, ref m);
		}
			
	}
}
