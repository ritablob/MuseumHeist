using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// central hub for scene loading and important variables
/// sits on game object GameManager which is don't destroy on load
/// bool for whether a port to arduino is already open
/// enum for which mode the game is currently in (wheter the player is currently walking around in museum, doing puzzle, etc)
/// bool for whether the gameplay is currently active (so the guard and player don't move in pause menu or during the puzzle)
/// </summary>

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
    public static bool gameplayActive;
    public static bool playWithKeys;

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
            gameplayActive = true;
            playWithKeys = false;
        }
    }

    public static void LoadGameScene()
    {
        Debug.Log("Load Game Scene");
        SceneManager.LoadScene(1);
        currentMode = GameMode.Gameplay;
        gameplayActive = true;
    }

    public static void LoadStartMenu()
    {
        Debug.Log("Load start menu");
        SceneManager.LoadScene(0);
        currentMode = GameMode.Menu;
    }

    private void Update()
    {
#if UNITY_EDITOR // for testing
        if (Input.GetKeyDown(KeyCode.O) && !portOpen && SceneManager.GetActiveScene().buildIndex != 0 && !playWithKeys)
        {
            EventManager.Instance.EventGo("CONTROLLER", "OpenConnection", defaultPortName);
        }
#endif
    }
}
