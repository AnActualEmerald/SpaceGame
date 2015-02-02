/*
 * Created by SharpDevelop.
 * User: Burrito119
 * Date: 1/9/2015
 * Time: 10:03 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using Microsoft.Xna.Framework;
using Game;
using Core;
using Core.Graphics;

namespace GUI
{
	/// <summary>
	/// Description of UIButton.
	/// </summary>
	public class UIButton : GameObject
	{
		private RenderMask inactive;
		private RenderMask active;
		private RenderMask pressed;
		private ButtonState renderState;
		
		private Fixture rect;
		private RenderingEngine renderer;
		
		public event Action<Object> OnClick;
		public event Action<Object> OnRelease;
		
	
		public UIButton(Vector2 pos, float width, float height, String texture_loaction, CoreEngine c)
		{
			Body bod = new Body(c.GetWorld(), pos);
			Vertices verts = new Vertices();
			verts.Add(new Vector2(0, 0));
			verts.Add(new Vector2(0 + width, 0));
			verts.Add(new Vector2(0 + width, 0 + height));
			verts.Add(new Vector2(0, 0 + height));
			
			Shape s = new PolygonShape(verts, 0);
			rect = bod.CreateFixture(s);
			rect.Body.Position = pos;
			
			renderer = c.GetEngine("ui");
		}
				
		public override void Render()
		{
			base.Render();
			switch(renderState){
				case ButtonState.ACTIVE:
					//render using active texture
					throw new NotImplementedException();
				case ButtonState.INACTIVE:
					//render using inactive texture
					throw new NotImplementedException();
				case ButtonState.CLICKED:
					//render using clicked texture
					throw new NotImplementedException();
				default:
					throw new ArgumentException("renderState not acceptable");
			}
			
		}
		
		public override void Update()
		{
			base.Update();
			if(CheckPoint(CoreInput.GetMousePos(), this, false))
				renderState = ButtonState.ACTIVE;
			else if(CheckPoint(CoreInput.GetMousePos(), this, CoreInput.GetM1Down()))
				renderState = ButtonState.CLICKED;
			else
				renderState = ButtonState.INACTIVE;
		}
		
		public bool CheckPoint(Vector2 point, Object sender, bool shouldClick = true)
		{
			bool b =  rect.TestPoint(ref point);
			if(b && shouldClick)
				this.OnClick(sender);
			return b;
		}
		
		public void Release(object sender)
		{
			this.OnRelease(sender);
		}
	}
	
	public enum ButtonState
	{
		ACTIVE = 0x001,
		INACTIVE = 0x002,
		CLICKED = 0x003
	}
}
