using System;
using System.Collections.Generic;
using Game;
using OpenTK.Graphics.OpenGL;
using FarseerPhysics.Common;
using OpenTK;

namespace Core.Graphics
{
	public class RenderMask : Component
	{
		protected int tex_id;
		protected RenderingEngine engine;
		private String engine_s;
		
		private RenderMask _instance;
		private int vao;
		private int[] vbo;
		private float[] verts = new float[] {
			0.0f, 0.5f, 0,
			0.0f, -0.5f, 0,
			0.5f, -0.5f, 0,
			0f, 0f, 0
		};
		//	0.5f, 0.0f, 0 };

		private float[] tex_coords = new float[]{
			0.0f, 0.0f, 
			1.0f, 0.0f, 
			1.0f, 1.0f, 
			0.0f, 1.0f};

		private static ushort[] index = new ushort[]{0, 1, 2, 3};

		public RenderMask (GameObject parent, String engine, int tex_id = 0)
		{
			this.parent = parent;
			this.tex_id = tex_id;
			engine_s = engine;
			vbo = new int[2];
		}

		public int GetTextureId()
		{
			return tex_id;
		}

		public void SetTextureId(int id)
		{

			GL.DeleteTexture (tex_id);
			this.tex_id = id;
		}

		public void SetVerts(Vertices verts)
		{
			List<float> f_v = new List<float>();
			List<float> f_c = new List<float> ();
			for (int i = 0, maxLength = verts.ToArray().Length; i < maxLength; i++) {
				Microsoft.Xna.Framework.Vector2 v = verts.ToArray()[i];
				Console.WriteLine ("Here is vert to set: " + v);
				f_v.Add(v.X);
				if (v.X > 0)
					f_c.Add (1);
				else
					f_c.Add (0);
				f_v.Add(v.Y);
				if (v.Y > 0)
					f_c.Add (1);
				else
					f_c.Add (0);
				f_v.Add (0);

			}
			this.verts = f_v.ToArray();
			this.tex_coords = f_c.ToArray ();
		}

		public void Rotate(float angle)
		{
				 
		}

		public void init_vbo()
		{
			GL.GenVertexArrays (1, out vao);
			GL.BindVertexArray (vao);

			GL.GenBuffers (2, vbo);

			GL.EnableVertexAttribArray (MainClass.vertAttrib);

			GL.BindBuffer (BufferTarget.ArrayBuffer, vbo[0]);
			GL.BufferData (BufferTarget.ArrayBuffer, 
			               new IntPtr(verts.Length * 3),
			               verts, BufferUsageHint.StaticDraw);

			GL.VertexAttribPointer (MainClass.vertAttrib, 3, VertexAttribPointerType.Float, false, 0, 0);

			GL.EnableVertexAttribArray (MainClass.posAttrib);

			GL.BindBuffer (BufferTarget.ArrayBuffer, vbo[1]);
			GL.BufferData (BufferTarget.ArrayBuffer,
				new IntPtr (tex_coords.Length * 3),
				tex_coords, BufferUsageHint.DynamicDraw);


			GL.VertexAttribPointer (MainClass.posAttrib, 2, VertexAttribPointerType.Float, false, 0, 0);


			GL.BindVertexArray (0);
		}

		public override void Render()
		{
			if (engine == null) {
				Console.Error.WriteLine ("Oh dear, render engine null");
				Console.Error.Flush ();
				throw new NullReferenceException ("Render engine for engine under \"" + engine_s + "\" was null");
			}
			engine.MakeRequest(new RenderRequest(ref _instance));
			
		}
			
		public void Draw()
		{
			GL.BindVertexArray (vao);       

			//GL.Uniform1 (MainClass.textureUniform, tex_id);

			GL.DrawArrays(PrimitiveType.Quads, 0, verts.Length * 8);

			GL.BindVertexArray (0);
				
		}

		public override void Update ()
		{
			//_instance = this;


				
		}

		public override void Input ()
		{

		}
		
		public override void init()
		{
			init_vbo ();

			engine = parent.GetWorld().GetEngine(engine_s);
			_instance = this;
		}

	}
}

