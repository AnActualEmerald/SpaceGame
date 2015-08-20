/*
 * Created by SharpDevelop.
 * User: Burrito119
 * Date: 6/1/2015
 * Time: 9:43 AM
 * 
 */
using System;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace SpaceGameSever
{
	/// <summary>
	/// Description of PhysicsWorld.
	/// </summary>
	public class PhysicsWorld
	{
		World _world;
		Dictionary<string, Fixture> fixtures = new Dictionary<string, Fixture>();
		
		public PhysicsWorld(Vector2 gravity, float visualPerSim)
		{
			_world = new World(gravity);
			ConvertUnits.SetDisplayUnitToSimUnitRatio(visualPerSim);
		}
		
		public void Step()
		{
			_world.Step(1/60);
		}
		
		public void AddClient(Client c)
		{
			Body c_body = BodyFactory.CreateBody(_world);
			c_body.Position = c.Pos;
			c_body.Rotation = c.Rot;
			Fixture c_fix = c_body.CreateFixture(c.Ship_Shape);
			//_world.AddBody(c_body);
            Console.WriteLine("Name: " + c.Name);
			fixtures.Add(c.Name, c_fix);
		}
		
		public Fixture GetFixture(string clientName)
		{
			try{
				return fixtures[clientName];
			}catch(KeyNotFoundException e){
				Console.Error.WriteLine("Key not found in Fixtures");
				Console.Error.WriteLine(e.Message);
				Console.Error.WriteLine(e.StackTrace);
				return null;
			}
		}
		
		public Body GetBody(string clientName){
			try{
				return fixtures[clientName].Body;
			}catch(KeyNotFoundException e){
				Console.Error.WriteLine("Key not found in Fixtures");
				Console.Error.WriteLine(e.Message);
				Console.Error.WriteLine(e.StackTrace);
				return null;
			}
		}
	}
}
