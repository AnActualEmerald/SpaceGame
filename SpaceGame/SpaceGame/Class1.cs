using System;
using Core;
using Game;

namespace OpenTkTest
{
    class Class1
    {

		private static CoreEngine c = new CoreEngine("HELLO WORLD", 60, 0);

        public static void Main(String[] args)
        {
			c.load += load;
			c.start ();
        }

		private static void load(object o)
		{
			GameObject g = new GameObject();

			c.root.AddChild (g);

			Console.WriteLine (c.root.ToString());

		}
    }
}
