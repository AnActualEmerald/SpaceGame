using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class GameObject
    {
        protected List<GameObject> children;
        protected List<Component> components;
        protected GameObject parent;
		protected bool is_root;

		public GameObject(GameObject parent)
        {
            init();
            this.parent = parent;
			if (parent == null)
				is_root = true;
        }

		public GameObject(GameObject parent = null, IEnumerable<GameObject> rest = null)
        {
            init();
            this.parent = parent;

			if (parent == null)
				is_root = true;

			if (rest == null)
				return;
            foreach(GameObject g in rest)
            {
                AddChild(g);
            }
        }

        public virtual void Update()
        {
            foreach (GameObject g in children)
                g.Update();

            foreach (Component c in components)
                c.Update();
        }

        public virtual void Input()
        {
            foreach (GameObject g in children)
                g.Input();

            foreach (Component c in components)
                c.Input();
        }

        public virtual void Render()
        {
            foreach (GameObject g in children)
                g.Render();

            foreach (Component c in components)
                c.Render();
        }

        // Add methods return this for easy constructing
        public virtual GameObject AddChild(GameObject g)
        {
            children.Add(g);
            return this;
        }

        public virtual GameObject AddComponent(Component c)
        {
            components.Add(c);
            return this;
        }

        // Convinience method for multiple constructors
        private void init()
        {
            children = new List<GameObject>();
            components = new List<Component>();
        }

    }
}
