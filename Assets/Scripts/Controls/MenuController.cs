using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour, IArduinoInput
{
    Controller controller;
    public void DataFromArduino(string message)
    {
        throw new System.NotImplementedException();
    }

    public void SendDataToArduino(string message)
    {
        if (controller != null) { controller.SendToArduino(message); }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
