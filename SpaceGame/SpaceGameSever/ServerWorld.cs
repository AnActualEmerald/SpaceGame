﻿/*
 * Created by SharpDevelop.
 * User: kgauthier16
 * Date: 12/2/2014
 * Time: 2:01 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
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
		
		public void AddClient(Client c)
		{
			Body bod = BodyFactory.CreateBody(physWorld, c.Ship_pos.AsXNAVector(), c.Ship_rot, c.Name);
			bod.CreateFixture(c.Ship_shape);
			c.Ship_body = bod;
		}
		
		public void RemoveClient(Client c)
		{
			physWorld.RemoveBody(c.Ship_body);
		}

		public bool Contains(Client c)
		{
			return physWorld.BodyList.Contains(c.Ship_body);
		}
		
		public bool Contains(Body b)
		{
			return physWorld.BodyList.Contains(b);
		}
		
	}
}