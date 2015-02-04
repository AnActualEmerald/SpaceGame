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
using FileManager;
using Game;

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
		
		public Ship(String name)
		{
			shipFile = new DataFile("./ships/" + name + ".shp");		
			String s;
			if(shipFile.ReadFile(out s))
				shipFile.Parse(s, out tiles);
			else
				shipFile.SaveTileData(new TileData(), false);	
			
			render_tiles = tiles != null ? buildShip(tiles) : new List<TileBasic>();
		}
		
		public static List<TileBasic> buildShip(List<TileData> tiles)
		{
			List<TileBasic> cc = new List<TileBasic>();
			
			foreach(TileData td in tiles)
			{
				switch((String)td.GetProperty("name")){
					case "s_thrust":
						cc.Add(AddTile(td, TileBasic.smallThrust));
						continue;
					case "l_hull":
						cc.Add(AddTile(td, TileBasic.lightHull));
						continue;
					case "l_thrust":
						cc.Add(AddTile(td, TileBasic.largeThrust));
						continue;
					default:
						cc.Add(AddTile(td, TileBasic.space));
						continue;
				}
				
			}

			return cc;
		}
		
		private static TileBasic AddTile(TileData t, TileBasic tb)
		{
			Object v = (String)t.GetProperty("vert");
			String[] xy = ((String)v).Split(';');
			tb.pos = new Microsoft.Xna.Framework.Vector2(float.Parse(xy[0]), float.Parse(xy[1]));
			v = t.GetProperty("data");
			tb.data = (int)v;
			return tb;
		}

	}
}
