using UnityEngine;

public class SpawnOnDeath : MonoBehaviour
{
    public GameObject deathEffectPrefab;
    public Vector3 spawnOffset = new Vector3(0f, 0.5f, 0f);

    private bool isQuitting = false;

    private void OnApplicationQuit()
    {
        isQuitting = true;
    }

    private void OnDestroy()
    {
        // Stops it from spawning explosions when you exit Play mode or restart the level
        if (isQuitting || !gameObject.scene.isLoaded) return;

        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position + spawnOffset, Quaternion.identity);
        }
    }
}