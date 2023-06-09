using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// controller script after the game is won
/// receives data (string message) from controller script via event manager
/// basically we just wait for the player to press the button again before returning to the main menu
/// </summary>

public class UIWinController : MonoBehaviour, IArduinoInput
{
    private void Start()
    {
        EventManager.Instance.AddEventListener("CONTROLLER", ControllerListener);
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveEventListener("CONTROLLER", ControllerListener);
    }

    void ControllerListener(string eventName, object param)
    {
        if (eventName == "IncomingDataArduino" && GameManagement.currentMode == GameManagement.GameMode.UIWin)
        {
            DataFromArduino((string)param);
        }
    }

    public void DataFromArduino(string message)
    {
        switch (message)
        {
            case "Light Barrier Open":
                break;
            case "Light Barrier Closed":
                break;
            case "Button Move Pressed":
                GetComponent<WinManager>().MenuButtonClick();
                break;
            case "Button Move Released":
                break;
            default: // default is the rotary encoder from which we get the actual value of rotation
                break;
        }
    }

    public void SendDataToArduino(string message)
    {
        EventManager.Instance.EventGo("CONTROLLER", "OutgoingDataArduino", message);
    }
}
