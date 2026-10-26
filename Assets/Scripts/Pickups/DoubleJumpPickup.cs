using UnityEngine;

public class DoubleJumpPickup : MonoBehaviour
{
    [Header("Floating Settings")]
    public float floatSpeed = 2f;
    public float floatHeight = 0.5f;
    public float rotationSpeed = 50f;

    [Header("Effects")]
    public GameObject pickupEffect; 
    public AudioClip pickupSound; 

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            
            if (player != null)
            {
                if (pickupSound != null)
                {
                    AudioSource.PlayClipAtPoint(pickupSound, transform.position);
                }

                if (pickupEffect != null)
                {
                    Instantiate(pickupEffect, transform.position, Quaternion.identity);
                }

                player.unlockDoubleJump = true;

                if (AbilityNotificationUI.Instance != null)
                {
                    AbilityNotificationUI.Instance.ShowUnlockNotification(
                        "DOUBLE JUMP UNLOCKED",
                        "Press [Space] in mid-air to jump again"
                    );
                }

                Destroy(gameObject);
            }
        }
    }
}