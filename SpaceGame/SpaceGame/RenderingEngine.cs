/*
 * Created by SharpDevelop.
 * User: Burrito119
 * Date: 1/20/2015
 * Time: 3:00 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using OpenTK.Graphics.OpenGL;

namespace Core.Graphics
{
	/// <summary>
	/// Description of RenderingEngine.
	/// </summary>
	public class RenderingEngine
	{
		private List<RenderRequest> requests;
		
		public RenderingEngine()
		{
			requests = new List<RenderRequest>();
			
		}
		
		public void Render()
		{
			foreach(RenderRequest r in requests)
			{
				r.GetMask().Render();
			}
		}
		
		public void Clear()
		{
			requests.Clear();
		}
	}
	
	public class RenderRequest
	{
		private RenderMask mask;
		
		public RenderRequest(ref RenderMask mask)
		{
			this.mask = mask;
		}
		
		public RenderMask GetMask()
		{
			return mask;
		}
		
	}
	
}
