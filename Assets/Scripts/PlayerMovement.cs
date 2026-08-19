using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float Speed;
    public InputActionReference moveInput;
    public InputActionReference jumpInput;
    public float jumpForce;
    public float dJumpForce;
    public float jPackForce;
    private bool isGrounded;
    public bool unlockDoubleJump;
    public bool unlockJetpack;
    public bool unlockHook;
    private bool dJump;
    public Rigidbody rb;
    void Start()
    {
        isGrounded = true;
    }

    void FixedUpdate()
    {
        var moveDirection = (Vector3)moveInput.action.ReadValue<Vector2>() * Speed;
        rb.linearVelocity = new Vector3 (moveDirection.x, rb.linearVelocity.y, moveDirection.y);  
        jumpInput.action.performed+= OnJump;
    }
    private void OnCollisionStay(Collision collision)
    {
        float parteSuperior = collision.transform.position.y + (collision.transform.localScale.y/2);

        float playerFeet = GetComponent<Collider>().bounds.min.y;

        if (parteSuperior <= playerFeet + 0.1f)
        {
            isGrounded = true;
            dJump = true;
        }
            
    }
    private void OnCollisionExit()
    {
        isGrounded = false;
    }
    private void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y + jumpForce, rb.linearVelocity.z);
            isGrounded = false;
        }
        else 
        {
            if(unlockDoubleJump && dJump)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y + dJumpForce, rb.linearVelocity.z);
                dJump = false;
            }
        }
    }
 
}
