using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// controller script during main menu
/// receives data (string message) from controller script via event manager
/// reference to every ui element in the scene
/// using data from rotary encoder to select which ui element is currently active
/// when button is pressed --> tells currently selected element it was clicked
/// </summary>

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
                // tell currently selected ui element it was clicked
                uiElements[currentlySelectedElement].ClickElement();
                EventManager.Instance.EventGo("AUDIO", "Click");
                break;
            case "Button Move Released":
                break;
            default: // default is the rotary encoder from which we get the actual value of rotation
                if (canRotate)
                {
                    int previous = currentlySelectedElement;
                    // check if current rotation is bigger or smaller than last saved
                    if (int.Parse(message) > lastSavedRotation)
                    {
                        // if it's bigger --> selected next ui element
                        currentlySelectedElement++;
                        if (currentlySelectedElement >= uiElements.Count) currentlySelectedElement = 0;
                    }
                    else
                    {
                        // if it's smaller --> select previous ui element
                        currentlySelectedElement--;
                        if (currentlySelectedElement < 0) currentlySelectedElement = uiElements.Count - 1;
                    }

                    // save current rotation to lastSavedRotation
                    lastSavedRotation = int.Parse(message);

                    //if the new selected element is not the previous one --> select new and de-select old one (turn on highlight etc)
                    if (previous != currentlySelectedElement) 
                    {
                        uiElements[currentlySelectedElement].SelectElement();
                        uiElements[previous].DeselectElement();
                        EventManager.Instance.EventGo("AUDIO", "Select");
                    }

                    //cooldown to make rotation less erratic
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
