using System;
using Core;
using Game;
using ShipBuild;
using FileManager;

namespace OpenTkTest
{
    class Class1
    {

		private static CoreEngine c = new CoreEngine("HELLO WORLD", 60, 200);

        public static void Main(String[] args)
        {
			c.load += load;
			c.start ();
        }

		private static void load(object o)
		{
			Ship s = new Ship ("mino", c);

			c.root.AddChild (s);
		}
    }
}
