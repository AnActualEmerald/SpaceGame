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
				r.GetMask().Draw();
			}
		}
		
		public void Clear()
		{
			requests.Clear();
		}
		
		public void MakeRequest(RenderRequest r)
		{
			this.requests.Add(r);
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
