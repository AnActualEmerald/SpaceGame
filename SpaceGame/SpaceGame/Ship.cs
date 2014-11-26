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
		private SpaceWorld world;
		
		public Ship(String name, SpaceWorld world)
		{
			shipFile = new DataFile("./ships/" + name + ".shp");		
			String s;
			if(shipFile.ReadFile(out s))
				shipFile.Parse(s, out tiles);
			else
				shipFile.SaveTileData(new TileData(), false);	

			this.world = world;
		}
		
		public static Component[] buildShip(List<TileData> tiles)
		{
			List<Component> cc = new List<Component>();
			
			foreach(TileData td in tiles)
			{
				if(td.GetProperty("name").Equals("s_thruster"))
					cc.Add(TileBasic.smallThrust);
			}

			return cc.ToArray();
		}


	}
}
