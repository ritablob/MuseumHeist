using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IArduinoInput
{
    void DataFromArduino(string message);
    void SendDataToArduino();
}

public class GameplayController : MonoBehaviour, IArduinoInput
{
    [SerializeField] private PlayerMovement player;
    

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

    public void SendDataToArduino()
    {
        throw new System.NotImplementedException();
    }
}
