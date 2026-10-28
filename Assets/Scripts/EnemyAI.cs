using UnityEngine;

// IDamageable script added right here
public class EnemyAI : MonoBehaviour, IDamageable
{

    [Header("Feedback")]
    public AudioSource hurtSound;

    [Header("Stats")]
    public float life = 100f;
    public float speed = 8f;
    public float turnSpeed = 8f;
    
    [Header("Targeting")]
    public Transform player;
    public float activeDistance = 25f;
    public float attackRange = 3f;

    [Header("Damage")]
    public int damageAmount = 25;
    public float damageCooldown = 1.5f;
    private float lastDamageTime = 0f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (player == null) 
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void FixedUpdate()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < activeDistance)
        {
            // 1. Look at Player
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0; // Keep the enemy from tilting up/down
            
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime));
            }

            // 2. Move towards Player 
            if (distance > attackRange)
            {
                Vector3 moveVelocity = direction * speed;
                rb.linearVelocity = new Vector3(moveVelocity.x, rb.linearVelocity.y, moveVelocity.z);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Time.time > lastDamageTime)
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.RecibirDanio(damageAmount);
                lastDamageTime = Time.time + damageCooldown;
            }
        }
    }

    //the IDamageable contract.
    public void TakeDamage(int amount)
    {
        life -= amount;

        // Visual Feedback
        FlashOnHit flash = GetComponent<FlashOnHit>();
        if (flash != null) flash.TriggerFlash();

        // Audio Feedback
        if (hurtSound != null && hurtSound.clip != null) 
        {
            hurtSound.pitch = Random.Range(0.8f, 1.2f);
            
            //This layers the sound instead of restarting it
            hurtSound.PlayOneShot(hurtSound.clip);
        }
        // Death Logic
        if (life <= 0)
        {
            Destroy(gameObject);
        }
    }
}