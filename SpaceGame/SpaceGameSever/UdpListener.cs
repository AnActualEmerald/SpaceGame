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
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SpaceGameSever.Udp
{

	/// <summary>
	/// Description of UdpListener.
	/// </summary>
	public class UdpListner
	{
		UdpClient client;
		IPEndPoint listen_to;
		byte[] buff;

		public UdpListner (IPAddress target, int port, bool isLocal)
		{
			client = isLocal ? new UdpClient (new IPEndPoint(IPAddress.Loopback, port)) : new UdpClient (new IPEndPoint(IPAddress.Any, port));
			listen_to = new IPEndPoint(target, port);
			client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
			//client.Client.Bind(listen_to);
		}

		public UdpListner(int port)
		{
			client = new UdpClient (port);
			listen_to = null;
		}

		public void Listen(out byte[] result)
		{
			buff = client.Receive (ref listen_to);
			result = buff;
		}

		public async Task<Packet> ListenAsync()
		{
			var res = await client.ReceiveAsync ();
			return new Packet () {
				sender = res.RemoteEndPoint,
				message = Encoding.ASCII.GetString(res.Buffer)
			};
		}

		public void BeginListenTo(AsyncCallback callback, IPEndPoint ep = null)
		{
			//client.Client.
			//client.Client.Bind(ep == null ? listen_to : ep);
			client.BeginReceive (callback, null);
		}

		public Packet EndListenTo(IAsyncResult r)
		{
			byte[] rec = client.EndReceive (r, ref listen_to);
			return new Packet () {
				sender = (IPEndPoint)r.AsyncState,
				message = Encoding.ASCII.GetString (rec)
			};

		}
		
		public async Task<Packet> ListenTo(IPEndPoint ep)
		{
			byte[] rec = client.Receive(ref ep);
			return new Packet()
			{
				sender = ep,
				message = Encoding.ASCII.GetString(rec)
			};
		}
			
		public void BeginSend(string message, IPEndPoint target, AsyncCallback callback)
		{
			byte[] mes = Encoding.ASCII.GetBytes (message);
			client.BeginSend (mes, mes.Length, target, callback, null);
		}

		public int EndSend(IAsyncResult r)
		{
			return client.EndSend (r);
		}

		public async Task<int> Send(string message, IPEndPoint target)
		{
			byte[] send = Encoding.ASCII.GetBytes (message);
			return client.Send (send, send.Length, target);
		}

		public async Task<int> Send(Packet p)
		{
			byte[] send = Encoding.ASCII.GetBytes (p.message);
			return client.Send (send, send.Length, p.sender);
		}

		private void ListenToCall(IAsyncResult r)
		{
			byte[] rec = client.EndReceive (r, ref listen_to);
		}

		public void ChangeListenOn(IPEndPoint ep)
		{
			listen_to = ep;
			client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
			client.Client.Bind(listen_to);
		}
		
	}

	public struct Packet
	{
		public IPEndPoint sender;
		public String message;
	}
}
