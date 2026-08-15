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
    bool isGrounded;
    bool unlockDoubleJump = false;
    bool unlockJetpack = false;
    bool unlockHook = false;
    public Rigidbody rb;
    void Start()
    {
        isGrounded = true;
    }

    void Update()
    {
        var moveDirection = (Vector3)moveInput.action.ReadValue<Vector2>();
        transform.position += new Vector3 (moveDirection.x,0,moveDirection.y) * (Time.deltaTime * Speed);  
        jumpInput.action.performed+= OnJump;
        if (moveDirection.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        if (moveDirection.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

    }
    private void OnCollisionStay()
    {
        
        
         isGrounded = true;
        
           
        
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
    }
}
