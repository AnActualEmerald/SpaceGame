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
		}

		public UdpListner(int port)
		{
			client = new UdpClient (port);
			listen_to = null;
		}
		
		public int RecTimeout
		{
			get{
				return client.Client.ReceiveTimeout;
			}
			set{
				client.Client.ReceiveTimeout = value;
			}
		}
		
		
		public int SendTimeout
		{
			get{
				return client.Client.SendTimeout;
			}
			set{
				client.Client.SendTimeout = value;
			}
		}
		
		public void Send(Packet p)
		{
			byte[] buff = new byte[0];
			try{
				buff = Encoding.ASCII.GetBytes((string)p.message);
			}
			catch(InvalidCastException){
				buff = (byte[])p.message;
			}
			client.BeginSend(buff, buff.Length, p.endpoint, new AsyncCallback(SendCall), client);
		}
		
		public void Send(string msg, IPEndPoint ep)
		{
			byte[] buff = Encoding.ASCII.GetBytes(msg);
			client.BeginSend(buff, buff.Length, ep, new AsyncCallback(SendCall), client);
		}

		public async Task<Packet> Listen()
		{
			var rec = await client.ReceiveAsync();
			return new Packet{
				endpoint = rec.RemoteEndPoint,
				message = rec.Buffer
			};
		}
		
		public async Task<Packet> ListenTo(IPEndPoint ep)
		{
			byte[] rec = client.Receive(ref ep);
			return new Packet{
				endpoint = ep,
				message = rec
			};
		}
		
		public Packet ListenToSync(IPEndPoint ep)
		{
			byte[] rec = client.Receive(ref ep);
			return new Packet{
				endpoint = ep,
				message = rec
			};
		}

		public UdpClient Client {
			get {
				return client;
			}
		}		
		
		#region callbacks
		
		private void SendCall(IAsyncResult r)
		{
			UdpClient client = (UdpClient)r.AsyncState;
			client.EndSend(r);
		}
				
		#endregion
		
		
	}

	public struct Packet
	{
		public IPEndPoint endpoint;
		public Object message;
	}
}
