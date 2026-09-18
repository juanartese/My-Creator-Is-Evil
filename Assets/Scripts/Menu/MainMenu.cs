using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Ensure your target game level is added to the File > Build Settings
    public void PlayGame()
    {
        // Loads the next scene in the build queue
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); 
    }

    public void QuitGame()
    {
        Debug.Log("Game Exited!"); // This only prints in the Unity Editor
        Application.Quit(); // This shuts down the final built game
    }
}