using UnityEngine;

public class AreaMovement : MonoBehaviour
{
    public GameObject Player;
    float distance;
    public float activeDistance;
    public float attackRange;
    public float speed;
    public float Life;

    [Header("Drops")]
    public GameObject pickupPrefab; // Drag your new Pickup Prefab here in the Inspector

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        Vector2 A = new Vector2(transform.position.x, transform.position.z);
        Vector2 B = new Vector2(Player.transform.position.x, Player.transform.position.z);
        distance = Vector2.Distance(A, B);

        if (distance < activeDistance && distance > attackRange)
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }

        if (Life <= 0)
        {
            // Spawn the pickup slightly above the enemy so it doesn't clip into the floor
            if (pickupPrefab != null)
            {
                Vector3 spawnPos = transform.position + (Vector3.up * 1f);
                Instantiate(pickupPrefab, spawnPos, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }
}