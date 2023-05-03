using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagement : MonoBehaviour
{
    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("GameManager");
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Application.targetFrameRate = 60;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public static void LoadGameScene()
    {
        Debug.Log("Load Game Scene");
        //SceneManager.LoadScene(1);
    }

    public static void LoadStartMenu()
    {
        Debug.Log("Load start menu");
        //SceneManager.LoadScene(0);
    }
}
