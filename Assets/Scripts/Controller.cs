using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;
using System;

public class Controller : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;

    SerialPort sp = new SerialPort("COM3", 9600);
    bool isStreaming = false;

    void OpenConnection()
    {
        isStreaming = true;
        sp.ReadTimeout = 100;
        sp.Open();
    }

    void CloseConnection()
    {
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
        OpenConnection();
    }

    private void OnDestroy()
    {
        CloseConnection();
    }

    void Update()
    {
        if (isStreaming)
        {
            string value = ReadSerialPort();
            if (value != null) 
            {
                //Debug.Log(value);
                //player.Rotate(int.Parse(value));
                
                switch (value)
                {
                    case "Light Barrier Open":
                        Debug.Log("no longer invisible");
                        break;
                    case "Light Barrier Closed":
                        Debug.Log("invisible wuaaaah");
                        break;
                    case "Button Move Pressed":
                        // button pressed, move forward
                        break;
                    case "Button Move Released":
                        // button released, stop moving
                        break;
                    default: // default is the rotatry encoder from which we get the actual value of rotation
                        player.Rotate(int.Parse(value));
                        break;
                }
            }
        }
    }

    public void SwitchLEDState(bool ledOn)
    {
        sp.WriteLine("L" + (ledOn ? "1" : "0"));
    }
}
