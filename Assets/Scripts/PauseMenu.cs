using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                GameManagement.guardsActive = false;
            }
        }
    }

    public void ResumeButton()
    {
        pauseMenu.SetActive(false);
        GameManagement.guardsActive = true;
    }

    public void MenuButton()
    {
        GameManagement.LoadStartMenu();
    }
}
