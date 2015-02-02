using System;
using Game;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using FarseerPhysics.Common;
//using Microsoft.Xna.Framework;

namespace Core.Graphics
{
	public class RenderMask : Component
	{
		protected int tex_id;
		protected uint vbo_id;
		protected uint tex_buff_id;
		protected uint index_id;

		private float[] verts = new float[]{
			0.0f, 0.0f,
			0.0f, 64.0f,
			64.0f, 64.0f,
			64.0f, 0.0f };

		private static float[] tex_coords = new float[]{
			0.0f, 0.0f, 
			0.0f, 1.0f, 
			1.0f, 1.0f, 
			1.0f, 0.0f};

		private static ushort[] index = new ushort[]{0, 1, 2, 3};

		public RenderMask (GameObject parent, int tex_id = 0)
		{
			this.parent = parent;
			this.tex_id = tex_id;
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
			foreach(Vector2 v in verts.ToArray())
			{
				
			}
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
			GL.BindBuffer (BufferTarget.ArrayBuffer, vbo_id);
			GL.EnableClientState(ArrayCap.VertexArray);             
			GL.VertexPointer(2, VertexPointerType.Float, 0, 0); 

			GL.BindTexture (TextureTarget.Texture2D, tex_id);

			GL.BindBuffer (BufferTarget.ArrayBuffer, tex_buff_id);
			GL.EnableClientState (ArrayCap.TextureCoordArray);
			GL.TexCoordPointer (2, TexCoordPointerType.Float, 0, 0);
			
			GL.DrawArrays (PrimitiveType.Quads, 0, verts.Length * 8);

			GL.DisableClientState (ArrayCap.VertexArray);
			GL.DisableClientState (ArrayCap.TextureCoordArray);
		
		}

		public override void Update ()
		{

		}

		public override void Input ()
		{

		}

	}
}

