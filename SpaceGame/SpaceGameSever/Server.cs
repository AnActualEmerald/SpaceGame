/*
 * Created by SharpDevelop.
 * User: kgauthier16
 * Date: 11/25/2014
 * Time: 11:57 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace SpaceGameSever
{
	/// <summary>
	/// Description of Server.
	/// </summary>
	public class Server
	{
		private List<Socket> clients = new List<Socket>();
		
		public Server()
		{
			
		}
	}
}
