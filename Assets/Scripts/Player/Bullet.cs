using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody rb;
    public float speed;
    public float duration;
    public int damage;
    public GameObject explosionPrefab;
    public GameObject sparkPrefab;

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
        //this is for the tnt, didn't know if i should change (don't know how the tnt script works)
        var tnt = collision.gameObject.GetComponent<TNTExplosiva>();
        if (tnt != null)
        {
            tnt.Explotar();
        }

        //this checks for the Idamagable script i made, easier than a bunch of if else statements :b
        IDamageable damageableTarget = collision.gameObject.GetComponent<IDamageable>();
        if (damageableTarget != null)
        {
            damageableTarget.TakeDamage(damage);
            GameObject sparks = Instantiate(sparkPrefab, transform.position, transform.rotation);
        }

        if (!collision.gameObject.CompareTag("Player"))
        {
            GameObject fx = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
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