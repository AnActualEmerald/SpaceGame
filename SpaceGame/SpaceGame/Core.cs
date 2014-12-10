using Graphics;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using Game;
using Files;


namespace Core
{
    class CoreEngine
    {
        private Display display;

        private int maxTicks, maxFrames;
        private long timeNowTicks, lastTimeTicks;
        private int frames, ticks;
        private double deltaTicks = 0.0;
        


		public GameObject root;

        /*
         * Leave maxFrames 0 for unlimited
         * 
         * */
        public CoreEngine(string title, int maxTicks, int maxFrames)
        {
            display = new Display(title);
            this.maxFrames = maxFrames;
            this.maxTicks = maxTicks;
			root = new GameObject ();
        }

		public void loadRes(Object sender, EventArgs e)
        {
			init_view ();

			GameObject obj = new GameObject ();
			RenderMask msk = new RenderMask (obj, ResLoader.GetTextureId(
				ResLoader.LoadImage("C:/users/kgauthier16/pictures/lusitania.jpg")));
			
			obj.AddComponent (msk);
			root.AddChild (obj);

			SetClearColor (Color.AliceBlue);
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
			display.SwapBuffers ();
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
			GL.MatrixMode (MatrixMode.Modelview);		

			GL.Enable (EnableCap.Texture2D);

		}
    }
}
