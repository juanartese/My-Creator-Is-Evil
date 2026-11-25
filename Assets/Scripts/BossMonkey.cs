using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class BossMonkey : MonoBehaviour, IDamageable
{
    [Header("Feedback & Audio")]
    public AudioSource hurtSound;
    public AudioSource attackSound;
    public AudioSource roarSound;
    public GameObject damageSparks;

    [Header("Stats")]
    public int maxLife = 1500;
    private int life;
    public float speed = 6f;
    public float turnSpeed = 8f;

    [Header("Targeting")]
    public Transform player;
    public float activeDistance = 35f;
    public float attackRange = 12f;

    [Header("Attack Sequence")]
    public GameObject attackType;
    public Transform shotPoint;
    public float jumpForce = 40f;
    public float lungeForwardForce = 12f;
    public float chargeSpeed = 20f;      // Speed for the Bull-Rush attack
    public float delay = 2f;             // Base time between attacks
    public float execTime = 0.5f;        // Wind-up before stomp
    public float duration = 0.6f;        // Recovery time after attacking

    [Header("Boss Multi-Shot Upgrades")]
    public int phase1ShotCount = 2;
    public int phase2ShotCount = 3;
    public float timeBetweenShots = 0.35f;
    public float spreadAngle = 0f;

    [Header("Phase 2 (Enraged at 50% HP)")]
    public float enragedSpeedMultiplier = 1.4f;
    public float enragedDelayMultiplier = 0.65f;
    [Tooltip("Optional: Drag your small robot prefab here to spawn backup at 50% HP.")]
    public GameObject minionPrefab;
    public int minionSpawnCount = 2;

    [Header("Optional Boss UI & Room Unlock")]
    public Slider bossHealthBar;
    public UnityEvent onBossDefeated;

    [Header("Drops")]
    public GameObject pickupPrefab;

    private Rigidbody rb;
    private Animator anim;
    private bool isAttacking = false;
    private bool isEnraged = false;
    private Coroutine currentAttackRoutine;

    void Start()
    {
        life = maxLife;
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) player = playerObj.transform;
        }

        if (bossHealthBar != null)
        {
            bossHealthBar.maxValue = maxLife;
            bossHealthBar.value = life;
        }

        if (damageSparks != null)
        {
            damageSparks.SetActive(false);
        }

        currentAttackRoutine = StartCoroutine(BossAttackLoop());
    }

    void FixedUpdate()
    {
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

            if (distance > attackRange)
            {
                float currentSpeed = isEnraged ? speed * enragedSpeedMultiplier : speed;
                Vector3 moveVelocity = direction * currentSpeed;
                rb.linearVelocity = new Vector3(moveVelocity.x, rb.linearVelocity.y, moveVelocity.z);
            }
        }
    }

    private IEnumerator BossAttackLoop()
    {
        while (life > 0)
        {
            // Randomize the delay slightly so the player can't memorize a fixed rhythm
            float baseDelay = isEnraged ? delay * enragedDelayMultiplier : delay;
            float randomDelay = baseDelay * Random.Range(0.75f, 1.25f);
            yield return new WaitForSeconds(randomDelay);

            if (player != null && Vector3.Distance(transform.position, player.position) < activeDistance)
            {
                isAttacking = true;
                FacePlayer();

                // Pick a random attack based on current phase
                int roll = Random.Range(0, 100);

                if (!isEnraged)
                {
                    // Phase 1: 65% Stomp, 35% Bull-Rush Charge
                    if (roll < 65)
                        yield return StartCoroutine(StompAttack(phase1ShotCount));
                    else
                        yield return StartCoroutine(ChargeAttack());
                }
                else
                {
                    // Phase 2: 50% Jump Slam, 30% Bull-Rush Charge, 20% Fast Stomp
                    if (roll < 50)
                        yield return StartCoroutine(JumpSlamAttack(phase2ShotCount));
                    else if (roll < 80)
                        yield return StartCoroutine(ChargeAttack());
                    else
                        yield return StartCoroutine(StompAttack(phase2ShotCount));
                }

                yield return new WaitForSeconds(duration);
                isAttacking = false;
            }
        }
    }

    private IEnumerator StompAttack(int waves)
    {
        if (anim != null) anim.CrossFade("MonkeyStomp", 0.1f);
        yield return new WaitForSeconds(execTime);

        // Drop arms immediately as the waves start
        if (anim != null) anim.CrossFade("MonkeyIdle", 0.15f);
        yield return StartCoroutine(FireBurst(waves));
    }

    private IEnumerator JumpSlamAttack(int waves)
    {
        if (anim != null) anim.CrossFade("MonkeyJump", 0.1f);

        Vector3 jumpVelocity = (transform.forward * lungeForwardForce) + (Vector3.up * jumpForce);
        rb.linearVelocity = new Vector3(jumpVelocity.x, jumpVelocity.y, jumpVelocity.z);

        // Wait until he passes the peak and starts falling
        float airTimer = 0f;
        while (rb.linearVelocity.y > -0.5f && airTimer < 3f)
        {
            airTimer += Time.deltaTime;
            yield return null;
        }

        // Wait until he hits the floor
        while (rb.linearVelocity.y < -0.1f && airTimer < 5f)
        {
            airTimer += Time.deltaTime;
            yield return null;
        }

        // Drop arms the exact moment he slams down, then fire the waves
        if (anim != null) anim.CrossFade("MonkeyIdle", 0.1f);
        yield return StartCoroutine(FireBurst(waves));
    }

    private IEnumerator ChargeAttack()
    {
        Debug.Log("BOSS CHARGING!");

        if (anim != null) anim.CrossFade("MonkeyIdle", 0.1f);
        FacePlayer();
        yield return new WaitForSeconds(0.25f);

        FacePlayer();
        Vector3 chargeDir = transform.forward;
        float chargeTime = 0.75f;
        float elapsed = 0f;

        while (elapsed < chargeTime)
        {
            // MovePosition physically pushes the boss forward, ignoring floor friction/drag
            rb.MovePosition(rb.position + chargeDir * chargeSpeed * Time.fixedDeltaTime);
            elapsed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
        yield return StartCoroutine(FireBurst(1));
    }

    private void FacePlayer()
    {
        if (player == null) return;
        Vector3 lookDir = (player.position - transform.position).normalized;
        lookDir.y = 0;
        if (lookDir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(lookDir);
        }
    }

    private IEnumerator FireBurst(int count)
    {
        if (attackType == null || shotPoint == null) yield break;

        float totalSpread = spreadAngle * (count - 1);
        float startAngle = -totalSpread / 2f;

        for (int i = 0; i < count; i++)
        {
            if (attackSound != null && attackSound.clip != null)
            {
                attackSound.pitch = Random.Range(0.75f, 0.95f);
                attackSound.PlayOneShot(attackSound.clip);
            }

            float currentAngle = (count <= 1) ? 0f : startAngle + (spreadAngle * i);
            Quaternion spreadRot = shotPoint.rotation * Quaternion.Euler(0f, currentAngle, 0f);
            Instantiate(attackType, shotPoint.position, spreadRot);

            if (i < count - 1)
            {
                yield return new WaitForSeconds(timeBetweenShots);
            }
        }
    }

    private void EnterEnragedPhase()
    {
        isEnraged = true;

        if (damageSparks != null)
        {
            damageSparks.SetActive(true);
        }

        if (roarSound != null && roarSound.clip != null)
        {
            roarSound.PlayOneShot(roarSound.clip);
        }

        // Spawn backup robots to the left and right of the boss
        if (minionPrefab != null)
        {
            for (int i = 0; i < minionSpawnCount; i++)
            {
                float side = (i % 2 == 0) ? 1f : -1f;
                Vector3 spawnPos = transform.position + (transform.right * side * 3f) + (Vector3.up * 0.5f);
                Instantiate(minionPrefab, spawnPos, Quaternion.identity);
            }
        }
    }

    public void TakeDamage(int amount)
    {
        if (life <= 0) return;

        life -= amount;

        if (bossHealthBar != null)
        {
            bossHealthBar.value = Mathf.Max(0, life);
        }

        FlashOnHit flash = GetComponent<FlashOnHit>();
        if (flash != null) flash.TriggerFlash();

        if (hurtSound != null && hurtSound.clip != null)
        {
            hurtSound.pitch = Random.Range(0.65f, 0.85f);
            hurtSound.PlayOneShot(hurtSound.clip);
        }

        if (!isEnraged && life <= maxLife * 0.5f)
        {
            EnterEnragedPhase();
        }

        if (life <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (currentAttackRoutine != null) StopCoroutine(currentAttackRoutine);

        if (bossHealthBar != null)
        {
            bossHealthBar.gameObject.SetActive(false);
        }

        onBossDefeated?.Invoke();

        if (pickupPrefab != null)
        {
            Vector3 spawnPos = transform.position + (Vector3.up * 1f);
            Instantiate(pickupPrefab, spawnPos, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}