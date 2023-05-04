using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinManager : MonoBehaviour
{
    public GameObject winCanvas;
    [HideInInspector] public bool hasArtefact;

    private void Start()
    {
        winCanvas.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        CheckWin();
    }
    public void CheckWin()
    {
        if (hasArtefact)
        {
            // win canvas visible
            // ui input viable
            // game input disabled 
            // game time to zero
            // display gameplay time 
            GameManagement.currentMode = GameManagement.GameMode.UIWin;
            winCanvas.SetActive(true);
            Time.timeScale = 0;
        }
    }
    public void MenuButtonClick()
    {
        GameManagement.LoadStartMenu();
    }
}
