using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;
using System;

/// <summary>
/// handles all comms between arduino and unity
/// opens & closes the port
/// hands over incoming data from arduino to other scripts via EventManager
/// sends data to arduino (receives info of what to send via EventManager)
/// the Arduino_Controller script sits on the GameManager object (which is don't destroy on load, same in every scene)
/// </summary>

public class Controller : MonoBehaviour
{
    private static string port = "COM3";

    SerialPort sp;
    bool isStreaming = false;

    void OpenConnection()
    {
        sp = new SerialPort(port, 9600);
        isStreaming = true;
        sp.ReadTimeout = 100;
        sp.Open();
    }

    void CloseConnection()
    {
        SendToArduino("L5");
        sp.Close();
    }

    string ReadSerialPort(int timeout = 50)
    {
        string message;
        sp.ReadTimeout = timeout;
        try
        {
            message = sp.ReadLine();
            return message;
        }
        catch (TimeoutException)
        {
            return null;
        }
    }

    void Start()
    {
        EventManager.Instance.AddEventListener("CONTROLLER", ControllerListener);
    }

    private void OnDestroy()
    {
        if (isStreaming) CloseConnection();
        EventManager.Instance.RemoveEventListener("CONTROLLER", ControllerListener);
    }

    void Update()
    {
        if (isStreaming)
        {
            string value = ReadSerialPort();
            if (value != null) 
            {
                Debug.Log(value);
                EventManager.Instance.EventGo("CONTROLLER", "IncomingDataArduino", value);
            }
        }
    }

    public void SendToArduino(string message)
    {
        if (isStreaming) sp.WriteLine(message);
    }

    void ControllerListener(string eventName, object param)
    {
        if (eventName == "OutgoingDataArduino")
        {
            SendToArduino((string)param);
        }
        else if (eventName == "OpenConnection")
        {
            port = (string)param;
            OpenConnection();
            GameManagement.portOpen = true;
        }
        else if (eventName == "CloseConnection")
        {
            if (isStreaming) CloseConnection();
        }
    }
}
