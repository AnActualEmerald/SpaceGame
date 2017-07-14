/*
 * Created by SharpDevelop.
 * User: Burrito119
 * Date: 1/9/2015
 * Time: 10:03 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using OpenTK;
using Game;
using Core;
using Core.Graphics;
using Files;
using Util;
using System.Drawing;
using System.Collections.Generic;

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

		private RenderMask renderer;
		private Vector2[] corners;
        private Vector2 position;

		public event Action<Object> OnClick;
		public event Action<Object> OnRelease;
		
	
		public UIButton(Vector2 pos, float width, float height, String texture_loaction, GameObject parent) : base(parent, parent.GetWorld())
		{
			path = texture_loaction;
			this.width = width;
			this.height = height;
            position = pos;
            			corners = new Vector2[]{
                new Vector2(),
				new Vector2(width, 0),
				new Vector2(width, height),
				new Vector2(0, height)};
			
			
			renderState = ButtonState.INACTIVE;
		}
		
		public UIButton(Vector2 pos, float width, float height, 
		                Bitmap active_tex, Bitmap inactive_tex, Bitmap clicked_tex, 
		                GameObject parent) : base(parent, parent.GetWorld())
		{
			
			this.width = width;
			this.height = height;
			corners = new Vector2[]{
                new Vector2(),
				new Vector2(width, 0),
				new Vector2(width, height),
				new Vector2(0, height)};
			
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
			//TODO add x and y members for positioning
			//renderer.Translate();
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
			bool b;
            Point p = CoreEngine.display.PointToClient(new Point((int)point.X, (int)point.Y));
            Vector2 pp = new Vector2(p.X, p.Y);
            b = Utilities.IsPointContained(corners, pp);
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

            Console.WriteLine("Button ID: " +active);

			

			List<Vector2> verts = new List<Vector2>();
            foreach (Vector2 v in corners)
                verts.Add(v);
            

			renderer = new RenderMask (this, "ui", active);
			renderer.SetVerts (verts);
            renderer.Translate(position.X+CoreEngine.display.Width, position.Y+CoreEngine.display.Height);
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
