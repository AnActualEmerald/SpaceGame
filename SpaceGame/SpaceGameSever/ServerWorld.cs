/*
 * Created by SharpDevelop.
 * User: kgauthier16
 * Date: 12/2/2014
 * Time: 2:01 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using ServerParts;

namespace SpaceGameSever
{
	/// <summary>
	/// Description of ServerWorld.
	/// </summary>
	public class ServerWorld
	{
		private World physWorld;
		
		public ServerWorld(S_Point gravity)
		{
			physWorld = new World(new Vector2(gravity.x, gravity.y));
		}
		
		public void step()
		{
			physWorld.Step(1/60);
		}
		
		public void AddBody(Body b)
		{
			physWorld.AddBody(b);
		}
		
		public void RemoveBody(Body b)
		{
			physWorld.RemoveBody(b);
		}
		
		
	}
}
