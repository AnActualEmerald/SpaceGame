using System;
using System.Collections.Generic;
using Game;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using FarseerPhysics.Common;

namespace Core.Graphics
{
	public class RenderMask : Component
	{
		protected int tex_id;
		protected RenderingEngine engine;
		private String engine_s;

		private Matrix4 m_rot = Matrix4.CreateRotationZ(0);
		private Matrix4 m_scale = Matrix4.CreateScale(1);
		private Matrix4 m_trans = Matrix4.CreateTranslation (0, 0, 0);

		private RenderMask _instance;
		private int vao;
		private int[] vbo;
		private float[] verts = new float[] {
			0.0f, 0f, 0, //0
			0f, 64f, 0, //1
			64f, 64f, 0, //2
			64, 0, 0 //3
		};
		//	0.5f, 0.0f, 0 };

		private float[] tex_coords = new float[]{
			0.0f, 0.0f, 
			1.0f, 0.0f, 
			1.0f, 1.0f, 
			0.0f, 1.0f};

		private int[] index = new int[]{0, 1, 2, 0, 3, 2};

		public RenderMask (GameObject parent, String engine, int tex_id = 0)
		{
			this.parent = parent;
			this.tex_id = tex_id;
			engine_s = engine;
			vbo = new int[3];
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

		public void SetVerts(List<Vector2> verts)
		{
			Vector2[] vv = new Vector2[4];
			foreach(Vector2 h in verts)
			{
				if (h.X == 0 && h.Y == 0)
					vv [0] = h;
				if (h.X == 0 && h.Y > 0)
					vv [1] = h;
				if (h.X > 0 && h.Y > 0)
					vv [2] = h;
				if (h.X > 0 && h.Y == 0)
					vv [3] = h;
			}


			List<float> f_v = new List<float>();
			List<float> f_c = new List<float> ();
			for (int i = 0; i < vv.Length; i++) {
				Vector2 v = vv[i];
				Console.WriteLine ("Here is vert to set: " + v);
				f_v.Add (v.X);
				if (v.X > 0)
					f_c.Add (1);
				else
					f_c.Add (0);
				f_v.Add (v.Y);
				if (v.Y > 0)
					f_c.Add (1);
				else
					f_c.Add (0);

				f_v.Add (0);

			}
			this.verts = f_v.ToArray();
			this.tex_coords = f_c.ToArray ();
			GenIndices (4, 4);
		}

		protected void GenIndices(int width, int height)
		{
			int[] indices = new int[64];
    		int i = 0;

		    for ( int row=0; row<height-1; row++ ) {
		        if ( (row&1)==0 ) { // even rows
		            for ( int col=0; col<width; col++ ) {
		                indices[i++] = col + row * width;
		                indices[i++] = col + (row+1) * width;
		            }
		        } else { // odd rows
		            for ( int col=width-1; col>0; col-- ) {
		                indices[i++] = col + (row+1) * width;
		                indices[i++] = col - 1 + + row * width;
		            }
		        }
		    }
		   // if ( (mHeight&1) && mHeight>2 ) {
		    //    mpIndices[i++] = (mHeight-1) * mWidth;
		    //}

			this.index = indices;
		}
			
		public void init_vbo()
		{
			GL.GenVertexArrays (1, out vao);
			GL.BindVertexArray (vao);

			GL.GenBuffers (3, vbo);

			GL.EnableVertexAttribArray (MainClass.vertAttrib);

			GL.BindBuffer (BufferTarget.ArrayBuffer, vbo[0]);
			GL.BufferData (BufferTarget.ArrayBuffer, 
			               new IntPtr(verts.Length * 6),
			               verts, BufferUsageHint.StaticDraw);

			GL.VertexAttribPointer (MainClass.vertAttrib, 3, VertexAttribPointerType.Float, false, 0, 0);

			GL.EnableVertexAttribArray (MainClass.posAttrib);

			GL.BindBuffer (BufferTarget.ArrayBuffer, vbo[1]);
			GL.BufferData (BufferTarget.ArrayBuffer,
				new IntPtr (tex_coords.Length * 4),
				tex_coords, BufferUsageHint.DynamicDraw);


			GL.VertexAttribPointer (MainClass.posAttrib, 2, VertexAttribPointerType.Float, false, 0, 0);

			GL.BindBuffer (BufferTarget.ElementArrayBuffer, vbo [2]);
			GL.BufferData (BufferTarget.ElementArrayBuffer, new IntPtr (index.Length * 2), index, BufferUsageHint.StaticDraw);

			GL.BindTexture (TextureTarget.Texture2D ,tex_id);
			GL.Uniform1 (MainClass.textureUniform, tex_id);

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
			Matrix4 mod = m_scale * m_rot * m_trans;
			Matrix4 m = mod * Matrix4.CreateOrthographic (parent.GetWorld().GetHorRes (), parent.GetWorld().GetVertRes (), -1, 1);
			GL.UniformMatrix4 (MainClass.mod_matUniform, false, ref m);

			GL.BindVertexArray (vao);       
	
			GL.DrawElements(PrimitiveType.TriangleStrip, index.Length * 2, DrawElementsType.UnsignedInt, 0);

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

			Console.WriteLine ("init done");
		}

		/// <summary>
		/// Rotate the specified a, point and pos.
		/// </summary>
		/// <param name="a">The angle.</param>
		/// <param name="point">The local point around which the mask rotates.</param>
		/// <param name="pos">The current position of the mask to get back to the origin.</param>
		public void Rotate(float a, Vector2 point, Vector3 pos)
		{
			m_rot = Matrix4.CreateRotationZ (a);
			m_rot = Matrix4.CreateTranslation (-pos) * Matrix4.CreateTranslation (new Vector3 (-point.X, -point.Y, 0)) * m_rot
			* Matrix4.CreateTranslation (new Vector3 (point.X, point.Y, 0)) * Matrix4.CreateTranslation (pos);
		}

		public void Translate(float off_x, float off_y)
		{
			m_trans = Matrix4.CreateTranslation (off_x, off_y, 0);
		}

		public void Scale(float sx, float sy)
		{
			m_scale = Matrix4.CreateScale(sx, sy, 1);
		}

		public static List<Vector2> ConvertToVector2(Vertices verts){
			List<Vector2> optk_verts = new List<Vector2>();
			foreach(Microsoft.Xna.Framework.Vector2 v in verts){
				optk_verts.Add(new Vector2(v.X, v.Y));
			}
			return optk_verts;
		}
		
	}
}

