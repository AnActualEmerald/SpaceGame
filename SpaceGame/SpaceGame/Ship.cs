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
		
		private int max_thrust;
		private int ship_tex_id;
		private string name;


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
			tb.pos = new Vector2(float.Parse(xy[0]), float.Parse(xy[1]));
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
				new Vector2(0, 0), 
				0, null, this);

			foreach (TileBasic tb in render_tiles) {
				FixtureFactory.AttachPolygon (tb.verts, 1, body.body);
			}

			AddComponent(body);
		}

		public override void Update ()
		{
			base.Update ();
			foreach (TileBasic t in render_tiles)
				if (t.name == "l_hull")
					t.Rotate (1f);

		}

	}
}
