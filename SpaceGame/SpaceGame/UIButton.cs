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
using FarseerPhysics;
using Files;

namespace GUI
{
	/// <summary>
	/// Description of UIButton.
	/// </summary>
	public class UIButton : GameObject
	{
		private int inactive, active, pressed;
		private float width, height;
		private ButtonState renderState;
		private string path;

		private Fixture rect;
		private RenderMask renderer;
		private Vertices verts;
		
		public event Action<Object> OnClick;
		public event Action<Object> OnRelease;
		
	
		public UIButton(Vector2 pos, float width, float height, String texture_loaction, GameObject parent) : base(parent, parent.GetWorld())
		{
			Body bod = new Body(parent.GetWorld().GetWorld(), new Vector2(ConvertUnits.ToSimUnits(pos.X), ConvertUnits.ToSimUnits(pos.Y)));
			float c_width = ConvertUnits.ToSimUnits (width);
			float c_height = ConvertUnits.ToSimUnits (height);
			verts = new Vertices();
			verts.Add(new Vector2(0, 0));
			verts.Add(new Vector2(0 + c_width, 0));
			verts.Add(new Vector2(0 + c_width, 0 + c_height));
			verts.Add(new Vector2(0, 0 + c_height));
			
			Shape s = new PolygonShape(verts, 1);
			rect = bod.CreateFixture(s);
			rect.Body.Position = pos;

			path = texture_loaction;
			this.width = width;
			this.height = height;

			renderState = ButtonState.INACTIVE;
		}
				
		public override void Render()
		{
			switch(renderState){
			case ButtonState.ACTIVE:
				renderer.SetTextureId (this.active);
				break;
			case ButtonState.INACTIVE:
				//render using inactive texture
				renderer.SetTextureId (this.inactive);
				break;
			case ButtonState.CLICKED:
				renderer.SetTextureId (this.pressed);
				break;
			default:
				throw new ArgumentException("renderState not acceptable");
			}
			base.Render ();
			
		}
		
		public override void Update()
		{
			renderer.Translate(ConvertUnits.ToDisplayUnits(rect.Body.Position.X), ConvertUnits.ToDisplayUnits(rect.Body.Position.Y));
			base.Update();
			if(CheckPoint(CoreInput.GetMousePos(), this, false))
				renderState = ButtonState.ACTIVE;
			if(CheckPoint(CoreInput.GetMousePos(), this, CoreInput.GetM1Down()))
				renderState = ButtonState.CLICKED;
			else
				renderState = ButtonState.INACTIVE;
		}
		
		public bool CheckPoint(Vector2 point, Object sender, bool shouldClick = true)
		{	
			point = new Vector2 (ConvertUnits.ToSimUnits(point.X), ConvertUnits.ToSimUnits(point.Y));
			bool b =  rect.TestPoint(ref point);
			if (b && shouldClick)
				OnClick.Invoke(sender);
			else if (b)
				Console.WriteLine ("I've been entered");
			return b;
		}
		
		public void Release(object sender)
		{
			this.OnRelease(sender);
		}

		public override void init ()
		{

				
			active = ResLoader.GetTextureId(ResLoader.LoadImage (path + "/inactive.png"));
			inactive = active;
			pressed = active;

			base.init ();

			verts.Clear ();
			verts.Add(new Vector2(0, 0));
			verts.Add(new Vector2(width, 0));
			verts.Add(new Vector2(width, height));
			verts.Add(new Vector2(0, height));

			renderer = new RenderMask (this, "ui", inactive);
			renderer.SetVerts (verts, false);
			renderer.init ();
			AddComponent(renderer);
		}
	}
	
	public enum ButtonState
	{
		ACTIVE = 0x001,
		INACTIVE = 0x002,
		CLICKED = 0x003
	}
}
