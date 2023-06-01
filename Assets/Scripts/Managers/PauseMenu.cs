using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// handles pause menu --> accessed by pressing ESCAPE and only really needed during testing
/// </summary>

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

    private void Start()
    {
        pauseMenu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu.activeInHierarchy)
            {
                ResumeButton();
            }
            else
            {
                pauseMenu.SetActive(true);
                GameManagement.gameplayActive = false;
            }
        }
    }

    public void ResumeButton()
    {
        pauseMenu.SetActive(false);
        GameManagement.gameplayActive = true;
    }

    public void MenuButton()
    {
        GameManagement.LoadStartMenu();
    }
}
