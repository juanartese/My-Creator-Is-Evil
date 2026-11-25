using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [Header("Teleport Settings")]
    public Transform destination;
    public bool matchRotation = true;

    [Header("Optional Feedback")]
    public AudioSource teleportSound;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || destination == null) return;

        // Reset physics momentum so the player doesn't fly off on arrival
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.position = destination.position;
        }

        other.transform.position = destination.position;

        if (matchRotation)
        {
            other.transform.rotation = destination.rotation;
        }

        if (teleportSound != null && teleportSound.clip != null)
        {
            teleportSound.PlayOneShot(teleportSound.clip);
        }
    }
}