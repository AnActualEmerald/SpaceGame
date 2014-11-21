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

namespace Game
{
	/// <summary>
	/// Description of PhysicsBody.
	/// </summary>
	public class PhysicsBody : Body, Component
	{
		public PhysicsBody(World world, Vector2? position = null, float rotation = 0, object userdata = null, GameObject parent = null) 
			: base(world, position, rotation, userdata)
		{
			this.parent = parent;
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
	}
}
