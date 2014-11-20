using System;
using Game;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace Graphics
{
	public class RenderMask : Component
	{
		protected int tex_id;
		protected uint vbo_id;
		protected uint tex_buff_id;
		protected uint index_id;

		private static float[] verts = new float[]{
			0.0f, 0.0f,
			0.0f, 256.0f,
			256.0f, 256.0f,
			256.0f, 0.0f };

		private static float[] tex_coords = new float[]{
			0.0f, 0.0f, 
			0.0f, 1.0f, 
			1.0f, 1.0f, 
			1.0f, 0.0f};

		private static double[] colors = new double[]{
			0, 0, 0,
			1, 0, 0,
			0, 1, 0,
			0, 0, 1 };

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

		public static int CreateID(byte[] pix, int width, int height)
		{
			int id = 0;
			GL.GenTextures (1, out id);
			GL.BindTexture (TextureTarget.Texture2D, id);
			GL.TexImage2D (TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, width, height, 0, 
				PixelFormat.Rgb, PixelType.UnsignedByte, pix);
			return id;
		}

		int color_id;

		protected void init_vbo()
		{
			GL.GenBuffers (1, out vbo_id);
			GL.BindBuffer (BufferTarget.ArrayBuffer, vbo_id);
			GL.BufferData (BufferTarget.ArrayBuffer, new IntPtr(verts.Length * 8 * sizeof(float)), verts, BufferUsageHint.StaticDraw);

			GL.GenBuffers (1, out tex_buff_id);
			GL.BindBuffer (BufferTarget.ArrayBuffer, tex_buff_id);
			GL.BufferData (BufferTarget.ArrayBuffer, new IntPtr (tex_coords.Length * 8 * sizeof(float)), tex_coords, BufferUsageHint.StaticDraw);

			GL.GenBuffers (1, out color_id);
			GL.BindBuffer (BufferTarget.ArrayBuffer, color_id);
			GL.BufferData (BufferTarget.ArrayBuffer, new IntPtr (colors.Length * 12 * sizeof(double)), colors, BufferUsageHint.StaticDraw);

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

			//GL.BindBuffer (BufferTarget.ArrayBuffer, color_id);
			//GL.EnableClientState (ArrayCap.ColorArray);
			//GL.ColorPointer (3, ColorPointerType.Double, 0, 0);

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

