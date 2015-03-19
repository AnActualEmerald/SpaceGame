/*
 * Created by SharpDevelop.
 * User: Burrito119
 * Date: 3/19/2015
 * Time: 1:37 PM
 * 
 */
using System;
using System.Net.Sockets;
using System.Net;

namespace SpaceGameSever
{
	public struct Recieved
	{
		public IPEndPoint Sender;
		public String message;
	}
	
	/// <summary>
	/// Description of UdpListener.
	/// </summary>
	public class UdpListener
	{
		public UdpListener()
		{
		}
		
		
	}
}
