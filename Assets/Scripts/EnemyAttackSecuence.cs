using System.Collections;
using System.Drawing;
using UnityEngine;

public class EnemyAttackSecuence : MonoBehaviour
{
    public MonoBehaviour Movement;
    public MonoBehaviour Attack;
    public Transform Player;
    public GameObject attackType;
    float distance;
    public float activeDistance;
    public float attackRange;
    public float delay;
    public float duration;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(AttackDelay());
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
            Instantiate(attackType, transform.position, transform.rotation);
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
