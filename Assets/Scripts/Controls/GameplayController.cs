using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// controller script during museum gameplay
/// receives data (string message) from controller script via event manager
/// calls corresponding functions in the player script for the different functionalities: rotation, forward movement and invisibility
/// </summary>

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
                player.LightBarrierInvisible(true);
                break;
            case "Light Barrier Closed":
                Debug.Log("invisible wuaaaah");
                player.LightBarrierInvisible(false);
                break;
            case "Button Move Pressed":
                player.Movement(true);
                break;
            case "Button Move Released":
                player.Movement(false);
                break;
            default: // default is the rotary encoder from which we get the actual value of rotation
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
