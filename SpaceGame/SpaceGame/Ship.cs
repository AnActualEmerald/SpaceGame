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
		
		public Ship(String name)
		{
			shipFile = new DataFile("./ships/" + name + ".shp");		
			String s;
			if(shipFile.ReadFile(out s))
				shipFile.Parse(s, out tiles);
			else
				shipFile.SaveTileData(new TileData(), false);	
			
			render_tiles = tiles != null ? buildShip(tiles) : new List<TileBasic>();
			init_thrust(render_tiles);
			
			body = new PhysicsBody(world.GetWorld(), 
			                       new Vector2(0, 0), 
			                       0, null, this);
			
			
			
			s_fixture = body.CreateFixture(new PolygonShape());
			
			AddComponent(body);
		}
		
		protected void init_thrust(List<TileBasic> t)
		{
			foreach(TileBasic tb in t)
			{
				if(tb.name == "s_thrust" || tb.name == "l_thrust")
					max_thrust += tb.data;
			}
		}
		
		public Vertices FindVerts()
		{
			return null;
		}
		
		public List<TileBasic> buildShip(List<TileData> tiles)
		{
			List<TileBasic> cc = new List<TileBasic>();
			
			foreach(TileData td in tiles)
			{
				cc.Add(AddTile(td));
			}

			return cc;
		}
		
		private TileBasic AddTile(TileData t)
		{
			TileBasic tb = new TileBasic((String)t.GetProperty("name"), this);
			Object v = (String)t.GetProperty("vert");
			String[] xy = ((String)v).Split(';');
			tb.pos = new Vector2(float.Parse(xy[0]), float.Parse(xy[1]));
			v = t.GetProperty("data");
			tb.data = (int)v;
			
			return tb;
		}

	}
}
