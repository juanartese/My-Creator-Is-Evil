using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public GameObject Player;
    float distance;
    public float activeDistance;
    public float attackRange;
    public float speed;
    public float Life;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 A = new Vector2(transform.position.x, transform.position.z);
        Vector2 B = new Vector2(Player.transform.position.x, Player.transform.position.z);
        distance = Vector2.Distance(A,B);
        if (distance < activeDistance && distance > attackRange)
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }
        if (Life <= 0)
        {
            Destroy(gameObject);
        }
    }
}
