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

		public static SmallThrustTile smallThrust = new SmallThrustTile ("s_thrust");

		protected String _name;
		protected int _data;
		
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
		
		public override void Render()
		{
			
		}
		
		public override void Update()
		{
			
		}
		
		public override void Input()
		{
			
		}

	}
}
