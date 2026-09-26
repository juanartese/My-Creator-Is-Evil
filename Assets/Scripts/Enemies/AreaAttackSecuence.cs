using System.Collections;
using System.Drawing;
using UnityEngine;

public class AreaAttackSecuence : MonoBehaviour
{
    public MonoBehaviour Movement;
    public MonoBehaviour Attack;
    public Transform Player;
    public GameObject attackType;
    float distance;
    public float activeDistance;
    public float attackRange;
    public float execTime;
    public float delay;
    public float duration;
    public float jumpForce;
    public Transform shotPoint;
    private Rigidbody rb;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(AttackDelay());
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    IEnumerator AttackDelay()
    {
        distance = Vector3.Distance(transform.position, Player.position);
        WaitForSeconds attackDelay = new WaitForSeconds(delay);
        yield return attackDelay;
        if (distance < activeDistance)
        {
            Movement.enabled = false;
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            WaitForSeconds attackExecTime = new WaitForSeconds(execTime);
            yield return attackExecTime;
            Instantiate(attackType, shotPoint.position, shotPoint.rotation);
        }
        StartCoroutine(AttackSecuence());
    }
    IEnumerator AttackSecuence()
    {
        distance = Vector3.Distance(transform.position, Player.position);
        WaitForSeconds attackDuration = new WaitForSeconds(duration);
        yield return attackDuration;
        if (distance < activeDistance)
        {
            Movement.enabled = true;
        }
        StartCoroutine(AttackDelay());


    }

}
