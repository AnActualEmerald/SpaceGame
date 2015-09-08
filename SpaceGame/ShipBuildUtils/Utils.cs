/*
 * Created by SharpDevelop.
 * User: Burrito119
 * Date: 3/25/2015
 * Time: 3:44 PM
 * 
 */
using System;
using System.Collections.Generic;
using OpenTK;
using System.Drawing;

namespace Util
{
	/// <summary>
	/// Description of MyClass.
	/// </summary>
	public static class Utilities
	{
		public static long GetTimeMillis(DateTime time)
		{
			return time.Ticks / TimeSpan.TicksPerMillisecond;
		}
		
		public static bool IsPointContained(Vector2[] container, Vector2 point)
		{
			if(point.X < container[0].X || point.Y < container[0].Y)
				return false;
			if(point.X > container[1].X)
				return false;
			if(point.Y > container[2].Y)
				return false;
			
			return true;
		}
	}
}