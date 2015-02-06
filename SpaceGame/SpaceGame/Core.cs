using System.IO;
using Graphics;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using FarseerPhysics.Dynamics;
using System;
using System.Drawing;
using Game;
using Files;
using Core.Graphics;


namespace Core
{
    public class CoreEngine
    {
        private Display display;

        private int maxTicks, maxFrames;
        private long timeNowTicks, lastTimeTicks;
        private int frames, ticks;
        private double deltaTicks = 0.0;
				
        private RenderingEngine UIEngine;
        private TextEngine textRenderer;
        private RenderingEngine TileEngine;
        private RenderingEngine BackgroundEngine;
        private World p_world; 
        private StreamWriter errorLog;

		public Action<Object> load;

		public GameObject root;

        /*
         * Leave maxFrames 0 for unlimited
         * 
         * */
        public CoreEngine(string title, int maxTicks, int maxFrames)
        {
        	TileEngine = new RenderingEngine();
			UIEngine = new RenderingEngine();
			BackgroundEngine = new RenderingEngine();
            display = new Display(title);
            this.maxFrames = maxFrames;
            this.maxTicks = maxTicks;
			root = new GameObject ();
			p_world = new World(new Microsoft.Xna.Framework.Vector2(0, 0));
			errorLog = new StreamWriter("./error.err");
			Console.SetError(errorLog);
        }

		public void loadRes(Object sender, EventArgs e)
        {
			init_view ();

			load.Invoke (sender);

			//has to be the last thing done here
			root.init();
		}

        public virtual void start()
        {
            display.UpdateFrame += Update;
            display.RenderFrame += Render;
            display.Load += loadRes;
            display.VSync = VSyncMode.Off;
            lastTimeTicks = GetTimeMillis(DateTime.Now);
            if (maxFrames != 0)
                display.Run(maxTicks, maxFrames);
            else
                display.Run(maxTicks);
        }

        public virtual void Update(Object sender, FrameEventArgs e)
        {
            timeNowTicks = GetTimeMillis(DateTime.Now);
			//do updates here

			root.Update ();

			//end updates
            deltaTicks += timeNowTicks - lastTimeTicks;
            lastTimeTicks = timeNowTicks;
            ticks++;
            if (deltaTicks >= 1000)
            {
                Console.WriteLine("FPS: " + frames + " | TPS: " + ticks);
                ticks = 0;
                frames = 0;
                deltaTicks = 0;
            }
        }

        public virtual void Render(Object sender, FrameEventArgs e)
        {
			GL.Clear (ClearBufferMask.ColorBufferBit);
            frames++;
			root.Render ();
			UIEngine.Render();
			TileEngine.Render();
			BackgroundEngine.Render();
			display.SwapBuffers ();
			UIEngine.Clear();
			TileEngine.Clear();
			BackgroundEngine.Clear();
        }

        public virtual void SetClearColor(Color c)
        {
            GL.ClearColor(c);
        }
        
        private long GetTimeMillis(DateTime time)
        {
            return time.Ticks / TimeSpan.TicksPerMillisecond;
        }

		private void init_view()
		{
			GL.MatrixMode (MatrixMode.Projection);
			GL.LoadIdentity ();
		    GL.Ortho (0, display.Width, display.Height, 0, -1, 1);
		    //GL.MatrixMode (MatrixMode.Modelview);

		    GL.Enable (EnableCap.Texture2D);

		}
		
		public World GetWorld()
		{
			return p_world;
		}
		
		public RenderingEngine GetEngine(String t)
		{
			if(t == "ui")
				return UIEngine;
			if(t.ToLower().Equals("t"))
				return TileEngine;
			if(t == "b")
				return BackgroundEngine;
		
			return null;
		}
		
		public TextEngine GetTextRenderer()
		{
			return textRenderer;
		}
    }
}
