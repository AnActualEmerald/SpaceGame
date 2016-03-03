using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace Game
{
    public class GameObject
    {
        protected List<GameObject> children;
        protected List<Component> components;
        protected GameObject parent;
		protected bool is_root;
		protected CoreEngine world;

		protected bool been_init = false;

		public GameObject(GameObject parent, CoreEngine world)
        {
			_init_lists();
            this.parent = parent;
			if (parent == null)
				is_root = true;
			if(world != null)
				this.world = world;
        }

		public GameObject(GameObject parent = null, CoreEngine world = null,  IEnumerable<GameObject> rest = null)
        {
            _init_lists();
            this.parent = parent;

			if (parent == null)
				is_root = true;
			
			if(world != null)
				this.world = world;
			
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

        public virtual CoreEngine GetWorld()
        {
        	return world;
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

        public virtual void dispose()
        {
            foreach (GameObject g in children)
                g.dispose();
            foreach (Component c in components)
                c.Dispose();
        }


        private void _init_lists()
        {
        	children = new List<GameObject>();
            components = new List<Component>();
        }
        
        // Convinience method for multiple constructors
        public virtual void init()
        {
        	foreach(GameObject g in children)
        		g.init();
        	foreach(Component c in components)
        		c.init();

			been_init = true;
        }

		public bool BeenInit {
			get{ return been_init;}
			set{ been_init = value;}
		}
    }
}
