/*
 * Created by SharpDevelop.
 * User: Burrito119
 * Date: 3/25/2015
 * Time: 3:44 PM
 * 
 */
using System;
using System.Collections.Generic;

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

	}
}