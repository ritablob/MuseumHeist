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
    private GameplayController gameplayController;
    private MenuController menuController;
    private UIController uiController;
    private PuzzleController puzzleController;

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
        //OpenConnection();
    }

    private void OnDestroy()
    {
        if (isStreaming) CloseConnection();
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.O) && !isStreaming) OpenConnection();

        if (isStreaming)
        {
            string value = ReadSerialPort();
            if (value != null) 
            {
                //Debug.Log(value);
                
                switch (currentMode)
                {
                    case GameMode.Gameplay:
                        if (gameplayController != null)
                            gameplayController.DataFromArduino(value);
                        break;
                    case GameMode.Menu:
                        if (menuController != null)
                            menuController.DataFromArduino(value);
                        break;
                    case GameMode.UI:
                        if (uiController != null)
                            uiController.DataFromArduino(value);
                        break;
                    case GameMode.Puzzle:
                        if (puzzleController != null)
                            puzzleController.DataFromArduino(value);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public void SendToArduino(string message)
    {
        sp.WriteLine(message);
    }
}
