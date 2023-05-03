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
    //[SerializeField] private PlayerMovement player;
    //GameManager gameManager;
    //public Button menuButton;


    public enum GameMode
    {
        Gameplay,
        Puzzle,
        UI,
        Menu
    }
    public GameMode currentMode = GameMode.Gameplay;
    [SerializeField] private GameplayController gameplayController;

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
        //gameManager = FindObjectOfType<GameManager>();
        //OpenConnection();
    }

    private void OnDestroy()
    {
        CloseConnection();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O) && !isStreaming) OpenConnection();

        if (isStreaming)
        {
            string value = ReadSerialPort();
            if (value != null) 
            {
                Debug.Log(value);
                //player.Rotate(int.Parse(value));
                
                switch (currentMode)
                {
                    case GameMode.Gameplay:
                        if (gameplayController != null)
                            gameplayController.DataFromArduino(value);
                        break;
                    case GameMode.Menu:
                        break;
                    case GameMode.UI:
                        break;
                    case GameMode.Puzzle:
                        break;
                    default:
                        break;
                }

                /*
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
                */
            }
        }
    }

    public void SwitchLEDState(bool ledOn)
    {
        sp.WriteLine("L" + (ledOn ? "1" : "0"));
    }
}
