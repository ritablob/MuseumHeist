using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;
using System;
using Unity.VisualScripting;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    public Button menuButton;
    [SerializeField] private PlayerMovement player;
    GameManager gameManager;

    SerialPort sp = new SerialPort("COM4", 9600);
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
        gameManager = FindObjectOfType<GameManager>();
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
                Debug.Log(value);
                //player.Rotate(int.Parse(value));
                
                switch (value)
                {
                    case "Light Barrier Open":
                        Debug.Log("no longer invisible");
                        player.Invisibility(false);
                        break;
                    case "Light Barrier Closed":
                        Debug.Log("invisible wuaaaah");
                        player.Invisibility(true);
                        break;
                    case "Button Move Pressed":
                        if (gameManager.UIMode)
                        {
                            gameManager.MenuButtonClick();
                            // ui input
                        }
                        else
                        {
                            player.Movement(true);
                        }
                        break;
                    case "Button Move Released":
                        if (gameManager.UIMode)
                        {
                            // ui input
                        }
                        else
                        {
                            player.Movement(false);
                        }
                        break;
                    default: // default is the rotatry encoder from which we get the actual value of rotation
                        if (gameManager.UIMode)
                        {
                            // ui input
                        }
                        else
                        {
                            player.Rotate(int.Parse(value));
                        }
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
