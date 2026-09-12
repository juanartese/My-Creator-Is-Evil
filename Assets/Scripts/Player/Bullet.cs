using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody rb;
    public float speed;
    public float duration;
    public int damage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, duration);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.transform.position += rb.transform.forward*speed*Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        var enemy = collision.gameObject.GetComponent<EnemyMovement>();
        if (enemy)
        {
            if (enemy.Life > 0)
            {
                enemy.Life = enemy.Life - damage;
                Debug.Log("Golpeaste al enemigo, le queda " + enemy.Life + " de vida");
            }
        }
        if (collision.gameObject.CompareTag("Player"))
        {  }
        else
        {
            Destroy(gameObject);
        }
    }
}
