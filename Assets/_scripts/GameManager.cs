using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System;
using System.Runtime.InteropServices;
using System.Text;
using DataSt;
using UnityEditor.VersionControl;

public class GameManager : MonoBehaviour
{
	private ServerUnity server;

	void Start()
	{

		//Data _d = new Data();
		//_d.currentSpeed = 91;
		//_d.coordinate = 1;
		//_d.nameRailRoad = "asd";

		//var t = Data.Serialize(_d);
		//Debug.Log(t);

		//var tt = Data.Deserialize(t);
		//Debug.Log(tt);

		server = new ServerUnity();
		server.StartServer();
	}
}

public class ServerUnity
{
	byte[] buffer = new byte[512];

	public void StartServer()
	{
		try {
			IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8181);

			var tcpListener = new TcpListener(ipEndPoint);
			tcpListener.Start();
			tcpListener.BeginAcceptSocket(AsyncAcceptSocket, tcpListener);

			Debug.Log("Start listen");
    
		} catch (Exception e) {
			Debug.LogError(e.ToString());
		} finally {
		}
	}

	private void AsyncAcceptSocket(IAsyncResult ar)
	{
		var tcpListener = (TcpListener)ar.AsyncState;
		var socket = tcpListener.EndAcceptSocket(ar);

		socket.NoDelay = true;

		socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, AsyncRecieve, socket);

		Debug.Log("AsyncAcceptSocket");
	}

	private void AsyncRecieve(IAsyncResult ar)
	{
		var socket = (Socket)ar.AsyncState;
		var byteCount = socket.EndReceive(ar);
		var msg = Data.Deserialize(buffer);
		Debug.Log(msg.ToString());
		Debug.Log("AsyncRecieve " + byteCount);

		socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, AsyncRecieve, socket);
	}
}
