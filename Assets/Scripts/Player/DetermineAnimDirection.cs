using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DetermineAnimDirection : MonoBehaviour
{
    private Animator animator;
    public InputActionReference moveInput;
    public float lookDirection;
    public float moveDirection;
    public float animDirection;
    public float playerAngle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        var direct = (Vector3)moveInput.action.ReadValue<Vector2>();
        playerAngle = transform.eulerAngles.y;
        if (playerAngle > 0 && playerAngle <90)
        {
            lookDirection = 1;
        }
        if (playerAngle > 90 && playerAngle < 180)
        {
            lookDirection = 2;
        }
        if (playerAngle > 180 && playerAngle < 270)
        {
            lookDirection = 3;
        }
        if (playerAngle > 270 && playerAngle < 360)
        {
            lookDirection = 4;
        }
        if (direct.y == 1)
        {
            moveDirection = 4;
        }
        if (direct.y == -1)
        {
            moveDirection = 2;
        }
        if (direct.x == 1)
        {
            moveDirection = 3;
        }
        if (direct.x == -1)
        {
            moveDirection = 1;
        }
        if (direct.x == 0 && direct.y ==0)
        {
            moveDirection = (-5);
        }

        animDirection = lookDirection + moveDirection;
        if (animDirection >= 5)
        {
            animDirection = animDirection - 4;
        }
        if (animDirection == 4)
        {
            animator.SetFloat("xAxis", 1);
        }
        if (animDirection == 3)
        {
            animator.SetFloat("yAxis", -1);
        }
        if (animDirection == 2)
        {
            animator.SetFloat("xAxis", -1);
        }
        if (animDirection == 1)
        {
            animator.SetFloat("yAxis", 1);
        }
        if (animDirection < 1)
        {
            animator.SetFloat("yAxis", 0);
            animator.SetFloat("xAxis", 0);
        }
    }
}
