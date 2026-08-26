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
        if (distance < activeDistance && distance > attackRange)
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }
    }
}
