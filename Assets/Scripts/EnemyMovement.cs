using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform Player;
    float distance;
    public float activeDistance;
    public float attackRange;
    public float speed;

    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        distance = Vector3.Distance(transform.position, Player.position);
        if (distance < activeDistance)
        {
            Vector3 direction = (Vector3)Player.position - (Vector3)transform.position;
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, angle, 0);
            if (distance > attackRange)
            {
                transform.position += transform.forward * speed * Time.deltaTime;
            }
        }
    }
}
