using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
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
        
        jumpInput.action.performed += OnJump;
        attackInput.action.performed += OnFire;
    }
    private void OnDestroy()
{
    jumpInput.action.performed -= OnJump;
    attackInput.action.performed -= OnFire;
}

    void FixedUpdate()
    {
        if (PauseMenu.GameIsPaused) return;

        var moveDirection = (Vector3)moveInput.action.ReadValue<Vector2>() * Speed;
        rb.linearVelocity = new Vector3 (moveDirection.x, rb.linearVelocity.y, moveDirection.y);  
        
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
        float parteSuperior = collision.gameObject.GetComponent<Collider>().bounds.max.y;
        float playerFeet = GetComponent<Collider>().bounds.min.y;

        if (parteSuperior <= playerFeet + 0.2f)
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
        if (PauseMenu.GameIsPaused) return;

        if (coyoteTimer <= coyoteTime)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        }
        else 
        {
            if(unlockDoubleJump && dJump)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, dJumpForce, rb.linearVelocity.z);
                dJump = false;
            }
        }
        isGrounded = false;
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (PauseMenu.GameIsPaused) return;

        if (context.performed)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }
}