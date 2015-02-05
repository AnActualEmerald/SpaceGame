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

namespace ShipBuild
{
	/// <summary>
	/// Description of TileBasic.
	/// </summary>
	public class TileBasic : GameObject
	{

		public static TileBasic smallThrust = new TileBasic ("s_thrust", null);
		public static TileBasic lightHull = new TileBasic("l_hull", null);
		public static TileBasic largeThrust = new TileBasic("l_thrust", null);
		public static TileBasic space = new TileBasic("space", null);
		
		protected String _name;
		protected int _data;
		protected Vector2 inship_pos;
		protected RenderMask mask;
		protected Ship parentShip;
		
		public int id;
		
		public TileBasic(String name, Ship parent) : base(parent, parent.GetWorld())
		{
			this.name = name;
			this.parentShip = parent;
			
			try{
				id = Files.ResLoader.GetTextureId(Files.ResLoader.LoadImage("./res/tiles/" + name));
			}catch(Exception c){
				Console.Error.WriteLine("Tile ini failed for " + name + "... error loading texture?");
				Console.Error.WriteLine(c.Message);
				Console.Error.WriteLine(c.StackTrace);
				Console.Error.WriteLine("Can't have missing textures 'round here, exiting program...");
				Environment.Exit(-18);
			}
			
			this.mask = new RenderMask(parentShip, "t", id);
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
		public override void Render()
		{
			
		}
		
		public override void Update()
		{
			
		}
		
		public override void Input()
		{
			
		}
		
		public override void init()
		{
			AddComponent(mask);
		}

	}
}
