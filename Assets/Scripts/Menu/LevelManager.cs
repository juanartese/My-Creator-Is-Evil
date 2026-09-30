using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [Header("UI Panels")]
    public GameObject gameOverUI;
    public GameObject victoryUI;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        gameOverUI.SetActive(false);
        victoryUI.SetActive(false);
        Time.timeScale = 1f; 
    }

public void GameOver()
    {
        gameOverUI.SetActive(true);
        Time.timeScale = 0f; 
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Victory()
    {
        victoryUI.SetActive(true);
        Time.timeScale = 0f; 
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("MainMenu"); 
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}