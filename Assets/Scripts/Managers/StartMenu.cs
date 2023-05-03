using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    [SerializeField] GameObject titleScreen;
    [SerializeField] GameObject chooseScreen;

    [SerializeField] private TMPro.TMP_InputField portNameField;

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
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.M)) GameManagement.LoadStartMenu();
    }

    public void InputPortName()
    {
        string portName = portNameField.text;
        if (portName != "" && portName.Contains("COM"))
        {
            EventManager.Instance.EventGo("CONTROLLER", "OpenConnection", portName);
            GameManagement.LoadGameScene();
        }
    }

    public void QuitButton()
    {
        Application.Quit();
        Debug.Log("application quit");
    }
}
