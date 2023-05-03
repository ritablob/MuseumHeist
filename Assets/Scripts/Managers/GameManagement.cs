using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagement : MonoBehaviour
{
    [SerializeField] string defaultPortName = "COM3";

    public enum GameMode
    {
        Gameplay,
        Puzzle,
        UIWin,
        Menu
    }

    public static GameMode currentMode = GameMode.Menu;
    public static bool portOpen = false;

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
            currentMode = GameMode.Menu;
            if (SceneManager.GetActiveScene().buildIndex == 1)
                currentMode = GameMode.Gameplay;
            portOpen = false;
        }
    }

    public static void LoadGameScene()
    {
        Debug.Log("Load Game Scene");
        SceneManager.LoadScene(1);
        currentMode = GameMode.Gameplay;
    }

    public static void LoadStartMenu()
    {
        Debug.Log("Load start menu");
        SceneManager.LoadScene(0);
        currentMode = GameMode.Menu;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O) && !portOpen && SceneManager.GetActiveScene().buildIndex != 0)
        {
            EventManager.Instance.EventGo("CONTROLLER", "OpenConnection", defaultPortName);
        }
    }
}
