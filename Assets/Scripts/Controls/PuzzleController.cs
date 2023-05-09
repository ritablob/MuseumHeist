using System.Collections;
using UnityEngine;

public class PuzzleController : MonoBehaviour, IArduinoInput
{
    private PuzzleManager gridManager;
    public int lastSavedRotation = 0;
    bool canRotate = true;
    float rotCooldown = 0.2f;
    bool canClick = true;
    float clickCooldown = 0.2f;

    private void Start()
    {
        EventManager.Instance.AddEventListener("CONTROLLER", ControllerListener);
        gridManager = GetComponent<PuzzleManager>();

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
                    gridManager.MoveTile();
                    StartCoroutine(ClickCooldown());
                }
                break;
            case "Button Move Released":
                break;
            default: // default is the rotatry encoder from which we get the actual value of rotation
                if (canRotate)
                {
                    bool up = false;
                    if (int.Parse(message) > lastSavedRotation)
                    {
                        up = true;
                    }
                    lastSavedRotation = int.Parse(message);
                    StartCoroutine(RotCooldown());
                    gridManager.SelectTile(up);
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
