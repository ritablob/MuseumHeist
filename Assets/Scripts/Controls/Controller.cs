using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;
using System;

public class Controller : MonoBehaviour
{
    //[SerializeField] private PlayerMovement player;
    //GameManager gameManager;
    //public Button menuButton;

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
        //OpenConnection();

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
