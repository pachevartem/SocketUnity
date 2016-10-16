using SocketClient;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;


public class ServerUnity : MonoBehaviour
{

  Thread StartRequest;
  private SocketClientProgram client;
  
  void Start()
  {
    client = new SocketClientProgram();
    StartRequest = new Thread(client.Start);
    StartRequest.Start();
  }

  ~ServerUnity()
  {
    StartRequest.Abort();
    Debug.Log("Killall");
  }
}


namespace SocketClient
{
  class SocketClientProgram
  {
    public void Start()
    {
      try
      {
        Debug.Log("Попытка соединения");
        byte[] bytes = new byte[1024];
        IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse("10.0.1.46"), 8181);
        Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

          try
          {
            sender.Connect(ipEndPoint);
            sender.Send(Encoding.UTF8.GetBytes("<VideoSystem>"));
            while(true)
            {
              int bytesRec = sender.Receive(bytes);
              DataSt.Data _d = DataSt.Data.Deserialize(bytes);
              Debug.Log(_d.currentSpeed);
              TrainController.CurentPosition = (float)_d.currentSpeed;
              bytes = new byte[1024];
            }
          }
          catch(Exception)
          {

            Debug.Log("Exeptio");
          }
      }
      catch(Exception ex)
      {
        Debug.Log("Exception");
      }
      finally
      {
        Debug.Log("FinalyBlog");
       
      }
    }
  }
}
