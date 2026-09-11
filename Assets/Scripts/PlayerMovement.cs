using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float Speed;
    public InputActionReference moveInput;
    public InputActionReference jumpInput;
    public InputActionReference attackInput;
    public float jumpForce;
    public float dJumpForce;
    public float jPackForce;
    private bool isGrounded;
    public bool unlockDoubleJump;
    public bool unlockJetpack;
    public bool unlockHook;
    private bool dJump;
    public Rigidbody rb;
    public float coyoteTime = 0.1f;
    private float coyoteTimer = 0f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    
    [SerializeField] private float bulletForce = 200f;
    void Start()
    {
        isGrounded = true;
    }

    void FixedUpdate()
    {
        var moveDirection = (Vector3)moveInput.action.ReadValue<Vector2>() * Speed;
        rb.linearVelocity = new Vector3 (moveDirection.x, rb.linearVelocity.y, moveDirection.y);  
        jumpInput.action.performed+= OnJump;
        attackInput.action.performed += OnFire;
        if (isGrounded)
        {
            coyoteTimer = 0f;
        }
        else
        {
            coyoteTimer += Time.deltaTime;
        }
        
    }
    private void OnCollisionStay(Collision collision)
    {
        float parteSuperior = collision.transform.position.y + (collision.transform.localScale.y/2);

        float playerFeet = GetComponent<Collider>().bounds.min.y;

        if (parteSuperior <= playerFeet + 0.1f)
        {
            isGrounded = true;
            dJump = true;
            coyoteTimer = 0f;
        }
            
    }
    private void OnCollisionExit()
    {
          isGrounded = false;
    }
    private void OnJump(InputAction.CallbackContext context)
    {
        if (coyoteTimer <= coyoteTime)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y + jumpForce, rb.linearVelocity.z);
        }
        else 
        {
            if(unlockDoubleJump && dJump)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y + dJumpForce, rb.linearVelocity.z);
                dJump = false;
            }
        }
        isGrounded = false;
    }
    public void OnFire(InputAction.CallbackContext context)
    {
        // Only fire when the button is first pressed down
        if (context.performed)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }

}
