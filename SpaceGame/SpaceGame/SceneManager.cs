using System;
using Game;

namespace Core
{
	public class SceneManager : GameObject
	{
		private GameObject[] scenes;

		/// <summary>
		/// Initializes a new instance of the <see cref="Core.SceneManager"/> class.
		/// </summary>
		/// <param name="parent">Parent.</param>
		/// <param name="world">World.</param>
		/// <param name="numScenes">Number of scenes.</param>
		public SceneManager (GameObject parent, CoreEngine world, int numScenes) : base(parent, world)
		{
			scenes = new GameObject[numScenes];
		}

		public void AddScene(GameObject scene, int index)
		{
			scenes [index] = scene;
		}

		public void SwitchToScene(int index)
		{
			children.Clear ();
			AddChild (scenes [index]);
		}

		public override void init ()
		{
			base.init ();

			foreach (GameObject g in scenes)
				if(g != null)
					g.init ();
		}
	}
}

