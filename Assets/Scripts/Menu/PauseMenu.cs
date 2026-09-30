using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // Required for the New Input System

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI; 

    void Start()
{
    GameIsPaused = false;
    Time.timeScale = 1f;
}

void Update()
    {

        if (LevelManager.instance != null)
        {
            if (LevelManager.instance.gameOverUI.activeSelf || LevelManager.instance.victoryUI.activeSelf)
            {
                return;
            }
        }

        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (GameIsPaused) Resume();
            else Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; 
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true); 
        Time.timeScale = 0f; 
        GameIsPaused = true;
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
        Application.Quit();
    }
}