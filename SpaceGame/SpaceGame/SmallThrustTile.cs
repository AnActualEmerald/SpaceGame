/*
 * Created by SharpDevelop.
 * User: kgauthier16
 * Date: 11/21/2014
 * Time: 1:49 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Files;
	
namespace ShipBuild
{
	/// <summary>
	/// Description of SmallThrustTile.
	/// </summary>
	public class SmallThrustTile : TileBasic
	{
		
		private int Texture_id;
		
		public SmallThrustTile(String name) : base(name)
		{
			try{
				Texture_id = ResLoader.GetTextureId(ResLoader.LoadImage("./res/tiles/" + name));
			}catch(Exception c){
				Console.Error.WriteLine("Tile ini failed for " + name + "... error loading texture?");
				Console.Error.WriteLine(c.Message);
				Console.Error.WriteLine(c.StackTrace);
				Console.Error.WriteLine("Can't have missing textures 'round here, exiting program...");
				Environment.Exit(-18);
			}
 		}
	}
}
