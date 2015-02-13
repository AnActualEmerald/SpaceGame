using System;
using Core;
using ShipBuild;
using FileManager;
using Core.Graphics;

namespace Game
{
    class MainClass
    {

		private static CoreEngine c = new CoreEngine("HELLO WORLD", 60, 200);
		private static Shader shader = new Shader ("./Shader/basic");

		public static int mod_matUniform;
		public static int textureUniform;
		public static int posAttrib;
		public static int vertAttrib;
		public static int offSetUniform;

        public static void Main(String[] args)
        {
			c.load += load;
			c.start ();
        }

		private static void load(object o)
		{
			Ship s = new Ship ("mino", c);

			shader.Post_init += shader_init;
			shader.init ();
			c.root.AddComponent (shader);

			c.root.AddChild (s);


		}

		private static void shader_init(Object sender)
		{
			mod_matUniform = shader.AddUniform ("mod_mat");
			textureUniform = shader.AddUniform ("texture");
			posAttrib = shader.AddAttribute ("pos");
			vertAttrib = shader.AddAttribute ("vertex");
			offSetUniform = shader.AddUniform("offset");
		}
    }
}
