
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody rb;
    public float speed;
    public float duration;
    public int damage;
    public GameObject explosionPrefab;

    void Start()
    {
        Destroy(gameObject, duration);
    }

    void FixedUpdate()
    {
        rb.transform.position += rb.transform.forward * speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        var enemy1 = collision.gameObject.GetComponent<EnemyMovement>();
        var enemy2 = collision.gameObject.GetComponent<AreaMovement>();
        var enemy3 = collision.gameObject.GetComponent<EnemigoGancho>();

        // Detectar TNT explosiva
        var tnt = collision.gameObject.GetComponent<TNTExplosiva>();

        if (tnt != null)
        {
            tnt.Explotar();
        }

        if (enemy2 != null)
        {
            enemy2.Life = enemy2.Life - damage;
        }

        if (enemy1 != null)
        {
            enemy1.Life = enemy1.Life - damage;
        }

        if (enemy3 != null)
        {
            enemy3.Life = enemy3.Life - damage;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
        }
        else
        {
            GameObject fx = Instantiate(
                explosionPrefab,
                transform.position,
                Quaternion.identity
            );

            ParticleSystem ps = fx.GetComponent<ParticleSystem>();

            if (ps != null)
            {
                ps.Play();
                Destroy(fx, ps.main.duration + ps.main.startLifetime.constantMax);
            }

            Destroy(gameObject);
        }
    }
}
