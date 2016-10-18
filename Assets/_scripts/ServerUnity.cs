
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Класс описывающий соединение с сервером
/// </summary>
public class ServerUnity : MonoBehaviour
{

	/// <summary>
	/// Создание нового потока
	/// </summary>
	Thread StartRequest;

	/// <summary>
	/// Ссылка на класс осуществляющий соединение с сервером
	/// </summary>
	private SocketClientProgram client;


	/// <summary>
	/// Метод зарезирвированый UNITY3D выполняется, при запуске программы
	/// </summary>
	void Start ()
	{
		client = new SocketClientProgram ();
		StartRequest = new Thread (client.Start);
		StartRequest.Start ();
	}
	/// <summary>
	/// Деструктор класса, в нем принудительно выключается созданный поток.
	/// </summary>
	~ServerUnity ()
	{
		StartRequest.Abort ();
	}
}


/// <summary>
/// Класс в котором описывается соединение с сервером
/// </summary>
class SocketClientProgram
	{
		/// <summary>
		/// Метод запускающий соединение. (синхронный сокет-клиент)
		/// </summary>
		public void Start ()
		{
		//попытка соединения с сервером.
			try {
				// размер буфера 
				byte[] buffers = new byte[1024];
				// конечная точка для подключения состоит из адреса и порта, данные беруться с Json файла
				IPEndPoint ipEndPoint = new IPEndPoint (SettingsGame.ipServer, SettingsGame.portServer);
				// Создание сокета по протоколу TCP\IP
				Socket sender = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				Debug.Log ("Попытка соединения с - " + SettingsGame.ipServer + " порт " + SettingsGame.portServer);
				
				// попытка отправить сообщение VideoSystem
				try {
					//Соединяемся с сокетом
					sender.Connect (ipEndPoint);
					//Отправляем массив байт (ковертирование)
					sender.Send (Encoding.ASCII.GetBytes ("VideoSystem"));
					//НАчинаем получать входящие данные
					while (true) {
						// Записываетм данные в буфер
						sender.Receive (buffers);
						// Десереализируем структуру данных 
						DataSt.Data _d = DataSt.Data.Deserialize (buffers);
						// Выводим на экран данные (при билде программы необходимо поставить галочку Development Build)
						Debug.LogError (_d.coordinate);
						// Передаем данные скрипту управляющему локомотивом.
						TrainController.CurentPosition = (float)_d.coordinate;
						// очищаем буфер
						buffers = new byte[1024];
					}
				} catch (Exception) {

					Debug.Log ("Exeptio");
				}
			} catch (Exception ex) {
				Debug.Log ("Exception");
			} finally {
				Debug.Log ("FinalyBlog");
       
			}
		}
	}

