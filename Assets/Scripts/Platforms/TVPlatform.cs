using UnityEngine;

public class TVPlatform : MonoBehaviour
{
    [Header("What to move")]
    public Transform tvRoot; 
    
    [Tooltip("Drag the object representing where the platform should travel to here")]
    public Transform targetPoint; 

    [Header("Settings")]
    public float travelSpeed = 5f; 
    public float returnSpeed = 2f; 
    
    [Tooltip("How many seconds to wait before returning?")]
    public float delayBeforeReturning = 1.0f;

    private Vector3 startPosition;
    private bool isPlayerOnTop = false;
    private float returnTimer = 0f;

    void Start()
    {
        startPosition = tvRoot.position;
    }

    void FixedUpdate()
    {
        if (isPlayerOnTop)
        {
            // Keep resetting the stopwatch as long as the player is standing on it
            returnTimer = delayBeforeReturning;
            
            // It now travels to the exact X, Y, and Z coordinates of the targetPoint
            tvRoot.position = Vector3.MoveTowards(tvRoot.position, targetPoint.position, travelSpeed * Time.fixedDeltaTime);
        }
        else
        {
            if (returnTimer > 0)
            {
                returnTimer -= Time.fixedDeltaTime;
            }
            else 
            {
                tvRoot.position = Vector3.MoveTowards(tvRoot.position, startPosition, returnSpeed * Time.fixedDeltaTime);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerOnTop = true;
            other.transform.SetParent(tvRoot);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerOnTop = false;
            other.transform.SetParent(null);
        }
    }
}