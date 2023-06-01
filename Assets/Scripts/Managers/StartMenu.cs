using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// handles functionality of the start menu
/// --> input field for which port to choose
/// --> (de-)activating the title screen and choose screen
/// --> telling game manager when to load the game scene
/// </summary>

public class StartMenu : MonoBehaviour
{
    [SerializeField] GameObject titleScreen;
    [SerializeField] GameObject chooseScreen;

    [SerializeField] private TMPro.TMP_InputField portNameField;

    bool canClick = false;

    void Start()
    {
        if (!GameManagement.portOpen)
        {
            titleScreen.SetActive(true);
            chooseScreen.SetActive(false);
        }
        else
        {
            titleScreen.SetActive(false); 
            chooseScreen.SetActive(true);
        }

        StartCoroutine(StartClick());
    }

    // called by button in menu
    public void InputPortName()
    {
        string portName = portNameField.text;
        if (portName.Contains("COM"))
        {
            EventManager.Instance.EventGo("CONTROLLER", "OpenConnection", portName);
            titleScreen.SetActive(false);
            chooseScreen.SetActive(true);
            GameManagement.playWithKeys = false;
        }
        else if (portName == "")
        {
            // play using just keys
            titleScreen.SetActive(false);
            chooseScreen.SetActive(true);
            GameManagement.playWithKeys = true;
        }
    }

    public void QuitButton()
    {
        if (!canClick) return;

        Application.Quit();
        Debug.Log("application quit");
    }

    public void PlayButton()
    {
        if (!canClick) return;
        GameManagement.LoadGameScene();
    }

    IEnumerator StartClick()
    {
        canClick = false;
        yield return new WaitForSeconds(2f);
        canClick = true;
    }
}
