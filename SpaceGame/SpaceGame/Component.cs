using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public abstract class Component
    {
        protected GameObject parent;

        public abstract void Update();
        public abstract void Render();
        public abstract void Input();
        public abstract void init();
    }
}
