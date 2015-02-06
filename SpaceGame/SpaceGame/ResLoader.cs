using System;
using System.IO;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using System.Drawing.Imaging;

namespace Files
{
	public class ResLoader
	{
		protected static FileStream input;

		public static Bitmap LoadImage(String path)
		{
			Bitmap img = new Bitmap (path);

			return img;
		}

		public static int GetTextureId(Bitmap b)
		{
			int i;
			GL.GenTextures (1, out i);

			GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);

			GL.BindTexture (TextureTarget.Texture2D, i);

			BitmapData data = b.LockBits (new Rectangle (0, 0, b.Width, b.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

			GL.TexImage2D (TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, b.Width, b.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0); 

			b.UnlockBits (data);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) TextureMagFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) TextureWrapMode.Repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) TextureWrapMode.Repeat);
				
			return i;


		}

	}
}

