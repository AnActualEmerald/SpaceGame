/*
 * Created by SharpDevelop.
 * User: Burrito119
 * Date: 1/13/2015
 * Time: 3:04 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using OpenTK.Input;
using Microsoft.Xna.Framework;

namespace Core
{
	/// <summary>
	/// Description of Input.
	/// </summary>
	public static class CoreInput
	{		
		public static Vector2 GetMousePos()
		{
			return new Vector2(Mouse.GetCursorState().X, Mouse.GetCursorState().Y);
		}
	}
}
