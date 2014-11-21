/*
 * Created by SharpDevelop.
 * User: kgauthier16
 * Date: 11/18/2014
 * Time: 9:02 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Game;

namespace ShipBuild
{
	/// <summary>
	/// Description of TileBasic.
	/// </summary>
	public class TileBasic : Component
	{
		
		protected String _name;
		protected int _data;
		protected bool enable_physics = true;
		
		public TileBasic(String name)
		{
			this.name = name;
		}
		
		public virtual int data{
			
			get { return _data; }
			set { _data = value; }
		}
		
		public virtual String name{
			get { return _name; }
			set { _name = value; }
		}
		
		public virtual void Render()
		{
			
		}
		
		public virtual void Tick()
		{
			
		}
		
		public virtual void Input()
		{
			
		}
		
		public static TileBasic GetTileByName()
		{
			
		}
	}
}
