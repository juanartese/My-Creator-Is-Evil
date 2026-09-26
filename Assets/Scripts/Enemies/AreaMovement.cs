using UnityEngine;

public class AreaMovement : MonoBehaviour
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
        distance = Vector3.Distance(transform.position, Player.transform.position);
        if (distance < activeDistance && distance > attackRange)
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }
        if (Life <= 0)
        {

            Player.GetComponent<PlayerMovement>().unlockDoubleJump = true;
            Destroy(gameObject);
        }
    }
}
