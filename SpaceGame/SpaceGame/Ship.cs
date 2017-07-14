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
using FarseerPhysics;

namespace ShipBuild
{
	/// <summary>
	/// Description of Ship.
	/// </summary>
	public class Ship : GameObject
	{
		public static float RAD_TO_DEG = 57.2957795f;

		private DataFile shipFile;
		private List<TileData> tiles = new List<TileData>();
		private List<TileBasic> render_tiles = new List<TileBasic>();
		private PhysicsBody body;
		private Fixture s_fixture;
		private RenderMask ship_mask;
		private Bitmap texture;

		private int max_thrust;
		private string name;
		private OpenTK.Vector2 center;
		private OpenTK.Vector3 Position;
		private float width, height;

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
						
		public override void init ()
		{

			base.init ();
            body = new PhysicsBody(world.GetWorld(),
                new Microsoft.Xna.Framework.Vector2(0, 0),
                0, null, this);
			body.body.BodyType = BodyType.Dynamic;
			Vertices v = new Vertices();
			foreach (TileBasic tb in render_tiles) {
				Vertices vvv = new Vertices();
				foreach (Microsoft.Xna.Framework.Vector2 vec in tb.verts) {
					vvv.Add (new Microsoft.Xna.Framework.Vector2 (ConvertUnits.ToSimUnits (vec.X), ConvertUnits.ToSimUnits (vec.Y)));
				}
				v.AddRange (vvv);
			}
			s_fixture = FixtureFactory.AttachPolygon (v, 1, body.body);
			texture = init_mask (render_tiles, out v);
            int gg = ResLoader.GetTextureId(texture);
            ship_mask = new RenderMask(this, "t", gg);
			ship_mask.SetVerts (RenderMask.ConvertToVector2(v.GetAABB().Vertices));
			ship_mask.init ();
			AddComponent (ship_mask);
			AddComponent(body);
			width = v.GetAABB ().Width;
			height = v.GetAABB ().Height;
			center = new OpenTK.Vector2 (v.GetCentroid ().X, v.GetCentroid ().Y);
		}

		float i = 0;
        float t = 0;
		public override void Update ()
		{
			base.Update ();
			ship_mask.Rotate (i, new OpenTK.Vector2(width / 2, height / 2), Position);
			i += 0.05f;
            ship_mask.Translate(t, 0);
            t += 0.5f;
		}
			
	}
}
