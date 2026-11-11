using System.Collections;
using UnityEngine;

// THE UPGRADE: Added IDamageable so the bullet can hurt it!
public class MonkeyAI : MonoBehaviour, IDamageable
{   

    [Header("Feedback")]
    public AudioSource hurtSound;

    [Header("Stats")]
    public int life = 1000;
    public float speed = 5f;
    public float turnSpeed = 8f;

    [Header("Targeting")]
    public Transform player;
    public float activeDistance = 25f;
    public float attackRange = 10f;

    [Header("Attack Sequence")]
    public GameObject attackType;
    public Transform shotPoint;
    public float jumpForce = 10f;
    public float delay = 2f;      // Time between attacks
    public float execTime = 0.5f; // Hang time before shooting
    public float duration = 1f;   // Recovery time after shooting

    [Header("Drops")]
    public GameObject pickupPrefab; 

    private Rigidbody rb;
    private Animator anim; // 1. Added Animator variable
    private bool isAttacking = false;
    private bool execOnce = true;
    public GameObject damageSparks;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>(); // 2. Grab the Animator component
        
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
            StartCoroutine(AttackRoutineB());
    }

    void FixedUpdate()
    {
        // If the monkey is currently in the middle of a jump attack, don't let it walk.
        if (isAttacking || player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < activeDistance)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0; 
            
            if (direction != Vector3.zero)
            {
                Quaternion targetRot = Quaternion.LookRotation(direction);
                rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRot, turnSpeed * Time.fixedDeltaTime));
            }

            // Walk towards player using Physics (replacing transform.position)
            if (distance > attackRange)
            {
                Vector3 moveVelocity = direction * speed;
                rb.linearVelocity = new Vector3(moveVelocity.x, rb.linearVelocity.y, moveVelocity.z);
            }
        }
            if (life <= 500)
            {
                if (execOnce == true)
                {
                    StopCoroutine(AttackRoutineB());
                    damageSparks.SetActive(true);
                    StartCoroutine(AttackRoutineA());
                    execOnce = false;
                }
            }
        
    }

    // This handles the timing of the jump and shoot sequence
    private IEnumerator AttackRoutineA()
    {
        while (life <= 500) // Infinite loop as long as the enemy is alive
        {
            // Wait for the cooldown
            yield return new WaitForSeconds(delay);
            if (player != null && Vector3.Distance(transform.position, player.position) < activeDistance)
            {

                // Lock movement
                isAttacking = true;

                if (anim != null) anim.CrossFade("MonkeyJump", 0.1f);

                // Jump
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);

                // Wait while in the air
                yield return new WaitForSeconds(execTime);

                // Shoot the projectile
                if (attackType != null && shotPoint != null)
                {
                    Instantiate(attackType, shotPoint.position, shotPoint.rotation);
                }

                // PUT ARMS BACK DOWN IMMEDIATELY AFTER SHOOTING!
                if (anim != null) anim.CrossFade("MonkeyIdle", 0.2f);

                // Wait to recover on the ground (with arms already down)
                yield return new WaitForSeconds(duration);

                // Unlock movement
                isAttacking = false;
            }
            
        }
    }
    private IEnumerator AttackRoutineB()
    {
        while (life>500) // Infinite loop as long as the enemy is alive
        {
            // Wait for the cooldown
            yield return new WaitForSeconds(delay);

            if (player != null && Vector3.Distance(transform.position, player.position) < activeDistance)
            {
                // Lock movement
                isAttacking = true;

                if (anim != null) anim.CrossFade("MonkeyStomp", 0.1f);

                // Wait while in the air
                yield return new WaitForSeconds(execTime);

                // Shoot the projectile
                if (attackType != null && shotPoint != null)
                {
                    Instantiate(attackType, shotPoint.position, shotPoint.rotation);
                }

                // PUT ARMS BACK DOWN IMMEDIATELY AFTER SHOOTING!
                if (anim != null) anim.CrossFade("MonkeyIdle", 0.2f);

                // Wait to recover on the ground (with arms already down)
                yield return new WaitForSeconds(duration);

                // Unlock movement
                isAttacking = false;
            }
        }
    }

    // The Bullet now triggers this automatically
    public void TakeDamage(int amount)
    {
        life -= amount;

        FlashOnHit flash = GetComponent<FlashOnHit>();
        if (flash != null) flash.TriggerFlash();
        
        // Audio Feedback
        if (hurtSound != null && hurtSound.clip != null) 
        {
            hurtSound.pitch = Random.Range(0.8f, 1.2f);
            
            //This layers the sound instead of restarting it
            hurtSound.PlayOneShot(hurtSound.clip);
        }

        if (life <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Spawn the double jump pickup slightly above the enemy
        if (pickupPrefab != null)
        {
            Vector3 spawnPos = transform.position + (Vector3.up * 1f);
            Instantiate(pickupPrefab, spawnPos, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
//to whoever reads this, make sure to add this two lines of code to the main enemy script to make this "flash on hit" script work, they must be on a "public void TakeDamage(int amount)": 
// FlashOnHit flash = GetComponent<FlashOnHit>();
        //if (flash != null) flash.TriggerFlash();