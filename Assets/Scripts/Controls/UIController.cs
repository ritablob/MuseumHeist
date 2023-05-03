using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour, IArduinoInput
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
        if (eventName == "IncomingDataArduino")
        {
            DataFromArduino((string)param);
        }
    }

    public void DataFromArduino(string message)
    {
        throw new System.NotImplementedException();
    }

    public void SendDataToArduino(string message)
    {
        EventManager.Instance.EventGo("CONTROLLER", "OutgoingDataArduino", message);
    }
}
