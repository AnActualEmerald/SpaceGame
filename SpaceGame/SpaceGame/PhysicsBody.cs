/*
 * Created by SharpDevelop.
 * User: kgauthier16
 * Date: 11/20/2014
 * Time: 1:26 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace Game
{
	/// <summary>
	/// Description of PhysicsBody.
	/// </summary>
	public class PhysicsBody : Component
	{
		private Body _body;

		public PhysicsBody(World world, Vector2? position = null, float rotation = 0, object userdata = null, GameObject parent = null) 
		{
			this.parent = parent;
			_body = new Body (world, position, rotation, userdata);
		}
		
		public Fixture CreateFixture(Shape s)
		{
			return _body.CreateFixture(s);
		}

		public virtual Body body{
			get {return _body;}
			set {_body = value;}
		}

		public override void Input()
		{
			
		}
		
		public override void Update()
		{
			
		}
		
		public override void Render()
		{
			
		}
		
		public override void init()
		{
			
		}
	}
}
