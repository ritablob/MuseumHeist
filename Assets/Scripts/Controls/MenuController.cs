using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour, IArduinoInput
{
    [SerializeField] private List<UIElements> uiElements;
    [SerializeField] private int currentlySelectedElement;
    public int lastSavedRotation = 0;
    bool canRotate = true;
    float cooldown = 0.1f;

    private void Start()
    {
        EventManager.Instance.AddEventListener("CONTROLLER", ControllerListener);
        uiElements[currentlySelectedElement].SelectElement();
        canRotate = true;
        lastSavedRotation = PlayerPrefs.GetInt("LastRotation");
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveEventListener("CONTROLLER", ControllerListener);
        PlayerPrefs.SetInt("LastRotation", lastSavedRotation);
    }

    void ControllerListener(string eventName, object param)
    {
        if (eventName == "IncomingDataArduino" && GameManagement.currentMode == GameManagement.GameMode.Menu)
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
                uiElements[currentlySelectedElement].ClickElement();
                break;
            case "Button Move Released":
                break;
            default: // default is the rotatry encoder from which we get the actual value of rotation
                if (canRotate)
                {
                    uiElements[currentlySelectedElement].DeselectElement();
                    if (int.Parse(message) > lastSavedRotation)
                    {
                        currentlySelectedElement++;
                        if (currentlySelectedElement >= uiElements.Count) currentlySelectedElement = 0;
                    }
                    else
                    {
                        currentlySelectedElement--;
                        if (currentlySelectedElement < 0) currentlySelectedElement = uiElements.Count - 1;
                    }
                    lastSavedRotation = int.Parse(message);
                    uiElements[currentlySelectedElement].SelectElement();
                    StartCoroutine(Cooldown());
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

    IEnumerator Cooldown()
    {
        canRotate = false;
        yield return new WaitForSeconds(cooldown);
        canRotate = true;
    }
}
