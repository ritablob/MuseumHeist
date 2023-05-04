using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IArduinoInput
{
    void DataFromArduino(string message);
    void SendDataToArduino(string message);
}

public class GameplayController : MonoBehaviour, IArduinoInput
{
    [SerializeField] private PlayerMovement player;

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
        if (eventName == "IncomingDataArduino" && GameManagement.currentMode == GameManagement.GameMode.Gameplay)
        {
            DataFromArduino((string)param);
        }
    }

    public void DataFromArduino(string message)
    {
        switch (message)
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
                player.Movement(true);
                break;
            case "Button Move Released":
                player.Movement(false);
                break;
            default: // default is the rotatry encoder from which we get the actual value of rotation
                player.Rotate(int.Parse(message));
                break;
        }
    }

    public void SendDataToArduino(string message)
    {
        EventManager.Instance.EventGo("CONTROLLER", "OutgoingDataArduino", message);
    }

    public void SwitchLEDState(bool ledOn)
    {
        SendDataToArduino("L" + (ledOn ? "1" : "0"));
    }
}
