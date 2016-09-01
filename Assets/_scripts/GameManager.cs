using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System;
using System.Text;

public class GameManager : MonoBehaviour
{
    private Thread _server;
    void Start()
    {
        _server = new Thread(StartServer);
        _server.Start();
    }

    void StartServer()
    {
        ServerUnity _server = new ServerUnity();
        _server.StartSrever();
    }


    public void OnDestroy()
    {
        _server.Abort();
    }

   

}


public class ServerUnity
{
    ~ServerUnity()
    {
        handler.Shutdown(SocketShutdown.Both);
        handler.Close();
        Debug.LogError("Соккет зaкрыт");
    }

    private Socket handler;

    public void StartSrever()
    {
        IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8181);
        Socket sListerner = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            sListerner.Bind(ipEndPoint);
            sListerner.Listen(100);
            while(true)
            {
                Debug.LogError("Начинаем слушать сокет: " + ipEndPoint);
                handler = sListerner.Accept();
                string data = null;
                var bytes = new byte[1024];
                var byteRec = handler.Receive(bytes);
                data += Encoding.UTF8.GetString(bytes, 0, byteRec);
                Debug.LogError("Полученный текст от клиента: " + data);
                string Answer;
                float Speed = StringChangeFloat(data);
                if(Speed == -9999)
                {
                    Answer = "Pls write only Speed Value, dont use Words";
                }
                else
                {
                    Answer = "Thanks, train speed is - " + Speed.ToString();
                    TrainController.Speed = Speed;
                }

                byte[] msg = Encoding.UTF8.GetBytes(Answer);
                handler.Send(msg);
                //handler.Shutdown(SocketShutdown.Both);
                //handler.Close();
                //Debug.LogError("Соккет зaкрыт");
            }
        }
        catch(Exception e)
        {
            Debug.LogError(e.ToString());
        }
        finally
        {
            Debug.LogError("У тебя ошибка - фикси и ищи");
            StartSrever();
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
        string _change = _str.Replace(" ", string.Empty);

        if(_change[0] == '-')
        {
            if(_change.Length > capasitySpeed + 1)
            {
                _change = _change.Remove(capasitySpeed + 1);
            }
        }
        else
        {
            if(_change.Length > capasitySpeed)
            {
                _change = _change.Remove(capasitySpeed);
            }
        }


        int result = -99;
        bool isInt = Int32.TryParse(_change, out result);
        //Debug.Log(isInt);


        return result = isInt ? result : -9999;
    }

    float StringChangeFloat(string _str)
    {
        _str = _str.Replace(" ", string.Empty);

        float result;
        bool isFloat = float.TryParse(_str, out result);

        if(isFloat && _str.Length > 6)
        {
            _str.Remove(6);
        }
        return result = isFloat ? result : -9999;
    }


}
