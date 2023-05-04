using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void InputPortName()
    {
        string portName = portNameField.text;
        if (portName != "" && portName.Contains("COM"))
        {
            EventManager.Instance.EventGo("CONTROLLER", "OpenConnection", portName);
            titleScreen.SetActive(false);
            chooseScreen.SetActive(true);
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
