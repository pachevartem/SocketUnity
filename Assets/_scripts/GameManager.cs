using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System;
using System.Text;
public class GameManager : MonoBehaviour {

	void Start () {
    Thread _server = new Thread(StartServer);
    _server.Start();
	}

  void StartServer()
  {
    ServerUnity _server = new ServerUnity();
    _server.StartSrever();
  }

}


public class ServerUnity
{
 public void StartSrever()
  {
    IPHostEntry ipHost = Dns.GetHostEntry("localhost");
    IPAddress ipAdr = ipHost.AddressList[0];
    IPEndPoint ipEndPoint = new IPEndPoint(ipAdr, 8181);


    Socket sListerner = new Socket(ipAdr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

    try
    {
      sListerner.Bind(ipEndPoint);
      sListerner.Listen(100);
      while(true)
      {
        Debug.Log("Начинаем слушать сокет: " + ipEndPoint);
        Socket handler = sListerner.Accept();
        string data = null;
        var bytes = new byte[1024];
        var byteRec = handler.Receive(bytes);

        data += Encoding.UTF8.GetString(bytes, 0, byteRec);

        //Debug.Log("Полученный текст от клиента: " + data);
        int Speed =  StringChange(data,3);
        string Answer;

        if(Speed != -9999)
        {
          Answer = "Thanks, train speed is - " + Speed.ToString();
          TrainController.Speed = Speed;
        }
        else
        {
          Answer = "Pls write only Speed Value, dont use Words";
        }
        byte[] msg = Encoding.UTF8.GetBytes(Answer);
        handler.Send(msg);


        handler.Shutdown(SocketShutdown.Both);
        handler.Close();

        //Debug.Log("Соккет звкрыт");

      }
    }
    catch(Exception e)
    {
      Debug.LogError(e.ToString());
    }
    finally
    {
      Debug.Log("У тебя ошибка - фикси и ищи"); 
    }
  }
  /// <summary>
  /// Передать строку, и величену знаков (пример: скрость до 999, значит значение capasiteSpeed = 3)
  /// </summary>
  /// <param name="_str"></param>
  /// <param name="capasitySpeed"></param>
  /// <returns></returns>
  int StringChange(string _str, int capasitySpeed)
  {
    string _change = _str.Replace(" ",string.Empty);

    if(_change[0] == '-')
    {
      if(_change.Length > capasitySpeed+1)
      {
        _change = _change.Remove(capasitySpeed+1);
      }
    }
    else
    {
      if(_change.Length > capasitySpeed)
      {
        _change = _change.Remove(capasitySpeed);
      }
    }


    int result=-99;
    bool isInt = Int32.TryParse(_change,out result);
    //Debug.Log(isInt);


    return result = isInt ? result:-9999;

  }
}
