using System;
using Game;
using FarseerPhysics.Dynamics;

namespace ShipBuild
{
	public class SpaceWorld : GameObject
	{
		private  World world = new World(new Microsoft.Xna.Framework.Vector2(0, 0));



		public SpaceWorld ()
		{
		}
	}
}

