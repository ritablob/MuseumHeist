using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinManager : MonoBehaviour
{
    public GameObject winCanvas;
    public GameObject loseCanvas;
    [HideInInspector] public bool hasArtefact;
    public bool canClickButton;
    float cooldown = 2f;

    private void Start()
    {
        winCanvas.SetActive(false);
        canClickButton = false;
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
            GameManagement.guardsActive = false;
            StartCoroutine(CanClick());
        }
    }
    public void CheckLose()
    {
        loseCanvas.SetActive(true);
        GameManagement.currentMode = GameManagement.GameMode.UIWin;
        GameManagement.guardsActive = false;
        StartCoroutine(CanClick());
    }

    public void MenuButtonClick()
    {
        if (canClickButton)
        {
            GameManagement.LoadStartMenu();
        }
    }

    IEnumerator CanClick()
    {
        canClickButton = false;
        yield return new WaitForSeconds(cooldown);
        canClickButton = true;
    }
}
