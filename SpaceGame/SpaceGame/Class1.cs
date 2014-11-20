using System;
using Core;
using System.Drawing;
using Game;
using Graphics;
using Files;

namespace OpenTkTest
{
    class Class1
    {

		private static CoreEngine c = new CoreEngine("HELLO WORLD", 60, 0);

        public static void Main(String[] args)
        {
			c.start ();
        }
    }
}
