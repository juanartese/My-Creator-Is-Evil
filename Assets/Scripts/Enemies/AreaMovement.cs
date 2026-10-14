using UnityEngine;

public class AreaMovement : MonoBehaviour
{
    public GameObject Player;
    float distance;
    public float activeDistance;
    public float attackRange;
    public float speed;
    public float Life;

    [Header("UI Feedback")]
    public AbilityNotificationUI notificationUI;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        Vector2 A = new Vector2(transform.position.x, transform.position.z);
        Vector2 B = new Vector2(Player.transform.position.x, Player.transform.position.z);
        distance = Vector2.Distance(A, B);

        if (distance < activeDistance && distance > attackRange)
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }

if (Life <= 0)
{

    if (Player != null)
    {
        Player.GetComponent<PlayerMovement>().unlockDoubleJump = true;
    }

    if (AbilityNotificationUI.Instance != null)
    {
        AbilityNotificationUI.Instance.ShowUnlockNotification(
            "DOUBLE JUMP UNLOCKED",
            "Press [Space] in mid-air to jump again"
        );
    }


    Destroy(gameObject);
}
    }
}