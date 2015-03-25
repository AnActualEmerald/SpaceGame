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

namespace SpaceGameSever
{

	/// <summary>
	/// Description of UdpListener.
	/// </summary>
	public class UdpListner
	{
		UdpClient client;
		IPEndPoint listen_to;
		byte[] buff;

		public UdpListner (IPEndPoint target, int port)
		{
			client = new UdpClient (port);
			listen_to = target;
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

		public void Listen(IPEndPoint target, out byte[] result)
		{
			buff = client.Receive (ref target);
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

		public void BeginListenTo(IPEndPoint remoteEP, AsyncCallback callback)
		{
			client.BeginReceive (callback, remoteEP);
		}

		public Packet EndListenTo(IAsyncResult r)
		{
			byte[] rec = client.EndReceive (r, ref (IPEndPoint)r.AsyncState);
			return new Packet () {
				sender = (IPEndPoint)r.AsyncState,
				message = Encoding.ASCII.GetString (rec)
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

		public void Send(string message, IPEndPoint target)
		{
			byte[] send = Encoding.ASCII.GetBytes (message);
			client.Send (send, send.Length, target);
		}

		public void Send(Packet p)
		{
			byte[] send = Encoding.ASCII.GetBytes (p.message);
			client.Send (send, send.Length, p.sender);
		}

		private void ListenToCall(IAsyncResult r)
		{
			byte[] rec = client.EndReceive (r, ref (IPEndPoint)r.AsyncState);
		}

	}

	public struct Packet
	{
		public IPEndPoint sender;
		public String message;
	}
}
