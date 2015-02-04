using System;
using System.Collections.Generic;
using Game;
using OpenTK.Graphics.OpenGL;
using FarseerPhysics.Common;

namespace Core.Graphics
{
	public class RenderMask : Component
	{
		protected int tex_id;
		protected uint vbo_id;
		protected uint tex_buff_id;
		protected uint index_id;
		protected RenderingEngine engine;
		private String engine_s;
		
		private RenderMask _instance;
		private float[] verts = new float[]{
			0.0f, 0.0f,
			0.0f, 256.0f,
			256.0f, 256.0f,
			256.0f, 0.0f };

		private static float[] tex_coords = new float[]{
			0.0f, 0.0f, 
			0.0f, 1.0f, 
			1.0f, 1.0f, 
			1.0f, 0.0f};

		private static ushort[] index = new ushort[]{0, 1, 2, 3};

		public RenderMask (GameObject parent, String engine, int tex_id = 0)
		{
			this.parent = parent;
			this.tex_id = tex_id;
			engine_s = engine;
			Console.WriteLine("ID is = " + tex_id);
			init_vbo ();
		}

		public int GetTextureId()
		{
			return tex_id;
		}

		public void SetTextureId(int id)
		{
			this.tex_id = id;
		}

		public void SetVerts(Vertices verts)
		{
			List<float> f_v = new List<float>();
			for (int i = 0, maxLength = verts.ToArray().Length; i < maxLength; i++) {
				Microsoft.Xna.Framework.Vector2 v = verts.ToArray()[i];
				f_v.Add(v.X);
				f_v.Add(v.Y);
			}
			this.verts = f_v.ToArray();
		}
		
		protected void init_vbo()
		{
			GL.GenBuffers (1, out vbo_id);
			GL.BindBuffer (BufferTarget.ArrayBuffer, vbo_id);
			GL.BufferData (BufferTarget.ArrayBuffer, 
			               new IntPtr(verts.Length * 8 * sizeof(float)),
			               verts, BufferUsageHint.StaticDraw);
			
			
			GL.GenBuffers (1, out tex_buff_id);
			GL.BindBuffer (BufferTarget.ArrayBuffer, tex_buff_id);
			GL.BufferData (BufferTarget.ArrayBuffer,
			               new IntPtr (tex_coords.Length * 8 * sizeof(float)),
			               tex_coords, BufferUsageHint.StaticDraw);
		}

		public override void Render()
		{
			if(engine == null)
				throw new ArgumentNullException();
			engine.MakeRequest(new RenderRequest(ref _instance));
			
		}
		
		public void Draw()
		{
			GL.EnableClientState (ArrayCap.TextureCoordArray);
			GL.EnableClientState(ArrayCap.VertexArray);   
			
			GL.BindBuffer (BufferTarget.ArrayBuffer, vbo_id);          
			GL.VertexPointer(2, VertexPointerType.Float, 0, 0); 

			GL.BindBuffer (BufferTarget.ArrayBuffer, tex_buff_id);
			GL.TexCoordPointer (2, TexCoordPointerType.Float, 0, 0);
			
			GL.BindTexture (TextureTarget.Texture2D, tex_id);
			
			GL.DrawArrays (PrimitiveType.Quads, 0, 4);

			GL.DisableClientState (ArrayCap.VertexArray);
			GL.DisableClientState (ArrayCap.TextureCoordArray);
		
		}

		public override void Update ()
		{
			_instance = this;
		}

		public override void Input ()
		{

		}
		
		public override void init()
		{
			engine = parent.GetWorld().GetEngine(engine_s);
			_instance = this;
		}

	}
}

