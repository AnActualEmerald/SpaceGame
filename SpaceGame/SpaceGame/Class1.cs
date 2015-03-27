using System;
using System.Net;
using System.Threading;
using SpaceGameSever;
using Core;
using Networking;
using ShipBuild;
using FileManager;
using Core.Graphics;
using GUI;

namespace Game
{
    class MainClass
    {

		private static CoreEngine c = new CoreEngine("HELLO WORLD", 60, 0);
		private static Shader shader;
		private static SceneManager sc;
		private static Thread serverThread;
		

		public static int mod_matUniform;
		public static int textureUniform;
		public static int posAttrib;
		public static int vertAttrib;
		public static int offSetUniform;

        public static void Main(String[] args)
        {        	
        	foreach(string s in args)
        		Console.WriteLine("Console Argument: " + s);
			string version = OpenTK.Graphics.OpenGL.GL.GetString(OpenTK.Graphics.OpenGL.StringName.Version);
			if(version.StartsWith("3") || version.StartsWith("4")){
				Console.WriteLine("OpenGL ok with version: " + version);
				shader = new Shader ("./Shader/basic");
			}
			else
			{
				Console.WriteLine("OpenGL not ok with version: "+ version);
				Console.WriteLine("Loading alternate shader");
				shader = new Shader("./Shader/old");
				
			}
			
			Server server_ = new Server();
			serverThread = new Thread(new ThreadStart(server_.Start));
			serverThread.Start();
			
			NetworkObj c = new NetworkObj(IPAddress.Loopback.Address, 25566, 25565);
			c.Connect();
			
			
			
			//c.load += load;
			//c.start ();
        }

		//Scene 0 = menu
		//Scene 1 = Main game
		//Scene 2 = server connection
		private static void load(object o)
		{			
			sc = new SceneManager (c.root, c, 3);

			GameObject scene1 = new GameObject (sc, c);
			Ship s = new Ship ("mino", c);
			scene1.AddChild (s);
			sc.AddScene (scene1, 1);

			GameObject scene0 = new GameObject (sc, c);
			UIButton start = new UIButton (new Microsoft.Xna.Framework.Vector2(0, 0), 128, 64, "./res/buttons/start", scene0);
			scene0.AddChild (start);
			sc.AddScene (scene0, 0);

			sc.SwitchToScene (0);

			shader.Post_init += shader_init;
			shader.init ();
			c.root.AddComponent (shader);
			c.root.AddChild (sc);
		}

		private static void shader_init(Object sender)
		{
			mod_matUniform = shader.AddUniform ("mod_mat");
			textureUniform = shader.AddUniform ("texture");
			posAttrib = shader.AddAttribute ("pos");
			vertAttrib = shader.AddAttribute ("vertex");
		}

		private static void ON_StartClick(Object sender)
		{
			sc.SwitchToScene (1);
		}
    }
}
