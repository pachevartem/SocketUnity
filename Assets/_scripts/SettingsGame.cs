using UnityEngine;
using System.Collections;
using LitJson;
using System.IO;
using System.Net;
using System;

public class SettingsGame : MonoBehaviour {

  public static int wigthResolution;
  public static int heigthResolution;
  public static  IPAddress ipServer;
  public static int portServer;



  void Awake()
  {
    ReadFromJsonSettingsGame();
  }

  void Start()
  {
    Screen.SetResolution(wigthResolution, heigthResolution, true);
  }

  void ReadFromJsonSettingsGame()
  {
    var a = File.ReadAllText(Application.dataPath + "/StreamingAssets/Player.json");
    JsonData _settingsData = JsonMapper.ToObject(a);

    wigthResolution = System.Convert.ToInt32(_settingsData["wigthResolution"].ToString());
    heigthResolution = System.Convert.ToInt32(_settingsData["heigthResolution"].ToString());
    ipServer = IPAddress.Parse(_settingsData["IpAdressServer"].ToString());
    portServer = System.Convert.ToInt32(_settingsData["portServer"].ToString());

    Debug.Log(String.Format("Считано из Json файла: разерещение {0} x {1}, ip-адресс сервера - {2}, порт - {3}", wigthResolution, heigthResolution, ipServer, portServer));       
  }
}
