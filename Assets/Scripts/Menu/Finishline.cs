using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that touched the trigger has the tag "Player"
        if (other.CompareTag("Player"))
        {
            // Call the Victory screen using our shortcut
            if (LevelManager.instance != null)
            {
                LevelManager.instance.Victory();
            }
        }
    }
}