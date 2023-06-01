using System.Collections;
using UnityEngine;

/// <summary>
/// controller script during puzzle game
/// receives data (string message) from controller script via event manager
/// holds reference to the puzzle manager and tells it when the rotary encoder was moved (up or down)
/// has cooldown for the rotary encoder so the movement is not too erratic
/// </summary>

public class PuzzleController : MonoBehaviour, IArduinoInput
{
    private PuzzleManager puzzleManager;
    public int lastSavedRotation = 0;
    bool canRotate = true;
    float rotCooldown = 0.2f;
    bool canClick = true;
    float clickCooldown = 0.2f;

    private void Start()
    {
        EventManager.Instance.AddEventListener("CONTROLLER", ControllerListener);
        puzzleManager = GetComponent<PuzzleManager>();

        canRotate = true;
        lastSavedRotation = PlayerPrefs.GetInt("LastRotation");
        canClick = true;
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveEventListener("CONTROLLER", ControllerListener);
    }

    void ControllerListener(string eventName, object param)
    {
        if (eventName == "IncomingDataArduino" && GameManagement.currentMode == GameManagement.GameMode.Puzzle)
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
                if (canClick) 
                {
                    puzzleManager.MoveTile();
                    StartCoroutine(ClickCooldown());
                }
                break;
            case "Button Move Released":
                break;
            default: // default is the rotary encoder from which we get the actual value of rotation
                if (canRotate)
                {
                    bool up = false;
                    if (int.Parse(message) > lastSavedRotation)
                    {
                        up = true;
                    }
                    lastSavedRotation = int.Parse(message);
                    StartCoroutine(RotCooldown());
                    puzzleManager.SelectTile(up);
                }
                else
                {
                    lastSavedRotation = int.Parse(message);
                }
                break;
        }
    }

    public void SendDataToArduino(string message)
    {
        EventManager.Instance.EventGo("CONTROLLER", "OutgoingDataArduino", message);
    }

    IEnumerator RotCooldown()
    {
        canRotate = false;
        yield return new WaitForSeconds(rotCooldown);
        canRotate = true;
    }

    IEnumerator ClickCooldown()
    {
        canClick = false;
        yield return new WaitForSeconds(clickCooldown);
        canClick = true;
    }
}
