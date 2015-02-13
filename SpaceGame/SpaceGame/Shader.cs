using System;
using Game;
using System.IO;
using OpenTK.Graphics.OpenGL4;

namespace Core.Graphics
{
	public class Shader : Component
	{
		public const int VERTEX = 0x01;
		public const int FRAGMENT = 0x02;

		protected string src_v;
		protected string src_f;
		protected int vert;
		protected int frag;
		protected int prog;

		public Action<Object> Post_init;

		/// <summary>
		/// Initializes a new instance of the <see cref="Core.Graphics.Shader"/> class.
		/// </summary>
		/// <param name="path">Path to the shader files, do not include file extesion</param>
		/// <param name="parent">Parent GameObject.</param>
		public Shader (String path, GameObject parent = null)
		{
			this.parent = parent;

			StreamReader file = new StreamReader (path + ".vert");
			src_v = file.ReadToEnd ();
			file = new StreamReader (path + ".frag");
			src_f = file.ReadToEnd ();
			vert = GL.CreateShader (ShaderType.VertexShader);
			frag = GL.CreateShader (ShaderType.FragmentShader);
			prog = GL.CreateProgram ();
		}

		public bool Compile()
		{
			GL.ShaderSource (vert, src_v);
			GL.ShaderSource (frag, src_f);

			GL.CompileShader (vert);

			string info;
			GL.GetShaderInfoLog (vert, out info);
			Console.WriteLine (info);

			int res;
			GL.GetShader (vert, ShaderParameter.CompileStatus, out res);
			if (res != 1) {
				Console.WriteLine ("Error in shader!");
				Console.WriteLine ("Abort! Abort! See Error.err for details!");
				Console.Error.WriteLine ("Well, something happened with this source: ");
				Console.Error.WriteLine (src_v);
				Console.Error.Flush ();
				return false;
			}

			GL.CompileShader (frag);

			GL.GetShaderInfoLog (vert, out info);
			Console.WriteLine (info);

			GL.GetShader (vert, ShaderParameter.CompileStatus, out res);
			if (res != 1) {
				Console.WriteLine ("Error in shader!");
				Console.WriteLine ("Abort! Abort! See Error.err for details!");
				Console.Error.WriteLine ("Well, something happened with this source: ");
				Console.Error.WriteLine (src_f);
				Console.Error.Flush ();
				return false;
			}
			return true;
		}

		public void Bind()
		{
			GL.UseProgram (prog);
		}

		/// <summary>
		/// Compiles and attaches shaders to shader program for use
		/// </summary>
		public override void init ()
		{
			Compile ();
			GL.AttachShader (prog, vert);
			GL.AttachShader (prog, frag);

			GL.LinkProgram(prog);

			// output link info log.
			string info;
			GL.GetProgramInfoLog(prog, out info);
			Console.WriteLine(info);

			GL.DeleteShader (vert);
			GL.DeleteShader (frag);

			Post_init.Invoke (this);

			Bind ();

		}

		public int AddUniform(string name, int t = 0)
		{
			return GL.GetUniformLocation (prog, name);
		}

		public int AddAttribute(string name)
		{
			int att = GL.GetAttribLocation (prog, name);
			return att;
		}

		public override void Input ()
		{

		}

		public override void Render ()
		{

		}

		public override void Update ()
		{
		}
	}
}

