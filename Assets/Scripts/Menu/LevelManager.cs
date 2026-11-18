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
        if (gameOverUI != null) gameOverUI.SetActive(false);
        if (victoryUI != null) victoryUI.SetActive(false);
        Time.timeScale = 1f;
        PauseMenu.GameIsPaused = false;
    }

    public void GameOver()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }
        else
        {
            Debug.LogWarning("LevelManager: gameOverUI is not assigned in the Inspector!");
        }

        Time.timeScale = 0f;
        PauseMenu.GameIsPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Victory()
    {
        if (victoryUI != null) victoryUI.SetActive(true);
        Time.timeScale = 0f;
        PauseMenu.GameIsPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        PauseMenu.GameIsPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        PauseMenu.GameIsPaused = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}