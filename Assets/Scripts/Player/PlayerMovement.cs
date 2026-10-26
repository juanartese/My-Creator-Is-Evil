
using Unity.VisualScripting;
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
    public float jPackDuration;
    public float jPackUse;
    private bool isGrounded;
    public bool unlockDoubleJump;
    public bool unlockJetpack;
    public bool unlockHook;
    private bool dJump;
    public Rigidbody rb;
    public float coyoteTime = 0.1f;
    private float coyoteTimer = 0f;
    public bool usingJetpack = false;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletForce = 200f;

    private int soundCodex = 1;
    public AudioSource sonidoSalto;
    public AudioSource sonidoS1;
    public AudioSource sonidoS2;
    public AudioSource sonidoS3;
    public AudioSource sonidoS4;
    public AudioSource sonidoS5;
    public AudioSource sonidoS6;
    public AudioSource sonidoS7;

    // Bananas propulsoras
    public GameObject bananaIzquierda;
    public GameObject bananaDerecha;
    public ParticleSystem propulsorBananaIzquierda;
    public ParticleSystem propulsorBananaDerecha;

    void Start()
    {
        isGrounded = true;

        jumpInput.action.performed += OnJump;
        attackInput.action.performed += OnFire;

        ActualizarBananas();

        DetenerPropulsores();
    }

    private void OnDestroy()
    {
        jumpInput.action.performed -= OnJump;
        attackInput.action.performed -= OnFire;
    }

    private void Update()
    {
        // Mostrar las bananas al desbloquear el doble salto
        ActualizarBananas();
    }

    private void ActualizarBananas()
    {
        if (bananaIzquierda != null && bananaIzquierda.activeSelf != unlockDoubleJump)
            bananaIzquierda.SetActive(unlockDoubleJump);

        if (bananaDerecha != null && bananaDerecha.activeSelf != unlockDoubleJump)
            bananaDerecha.SetActive(unlockDoubleJump);
    }

    private void DetenerPropulsores()
    {
        if (propulsorBananaIzquierda != null)
            propulsorBananaIzquierda.Stop(
                true,
                ParticleSystemStopBehavior.StopEmittingAndClear
            );

        if (propulsorBananaDerecha != null)
            propulsorBananaDerecha.Stop(
                true,
                ParticleSystemStopBehavior.StopEmittingAndClear
            );
    }

    void FixedUpdate()
    {
        if (PauseMenu.GameIsPaused) return;

        var moveDirection = (Vector3)moveInput.action.ReadValue<Vector2>() * Speed;

        rb.linearVelocity = new Vector3(
            moveDirection.x,
            rb.linearVelocity.y,
            moveDirection.y
        );

        if (isGrounded)
        {
            coyoteTimer = 0f;

            if (jPackUse < jPackDuration)
            {
                jPackUse = Mathf.Lerp(jPackUse, jPackDuration, 0.5f);
            }
        }
        else
        {
            coyoteTimer += Time.deltaTime;
        }

        if (usingJetpack == true)
        {
            rb.AddForce(Vector3.up * jPackForce, ForceMode.Force);
            jPackUse -= Time.deltaTime;
        }

        if (jPackUse <= 0)
        {
            usingJetpack = false;
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

            // Apagar las particulas al tocar el suelo
            DetenerPropulsores();
        }
    }

    private void OnCollisionExit()
    {
        isGrounded = false;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (PauseMenu.GameIsPaused) return;

        soundCodex = Random.Range(1, 8);

        if (soundCodex == 1)
            sonidoSalto = sonidoS1;

        if (soundCodex == 2)
            sonidoSalto = sonidoS2;

        if (soundCodex == 3)
            sonidoSalto = sonidoS3;

        if (soundCodex == 4)
            sonidoSalto = sonidoS4;

        if (soundCodex == 5)
            sonidoSalto = sonidoS5;

        if (soundCodex == 6)
            sonidoSalto = sonidoS6;

        if (soundCodex == 7)
            sonidoSalto = sonidoS7;

        if (coyoteTimer <= coyoteTime)
        {
            rb.linearVelocity = new Vector3(
                rb.linearVelocity.x,
                jumpForce,
                rb.linearVelocity.z
            );

            sonidoSalto.Play();
        }
        else
        {
            if (unlockDoubleJump && dJump)
            {
                rb.linearVelocity = new Vector3(
                    rb.linearVelocity.x,
                    dJumpForce,
                    rb.linearVelocity.z
                );

                dJump = false;
                sonidoSalto.Play();

                // Activar las bananas en el segundo salto
                if (propulsorBananaIzquierda != null)
                    propulsorBananaIzquierda.Play();

                if (propulsorBananaDerecha != null)
                    propulsorBananaDerecha.Play();
            }
            else
            {
                if (isGrounded == false &&
                    dJump == false &&
                    unlockJetpack &&
                    usingJetpack == false)
                {
                    usingJetpack = true;
                }
                else
                {
                    usingJetpack = false;
                }
            }
        }

        isGrounded = false;
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (PauseMenu.GameIsPaused) return;

        if (context.performed)
        {
            GameObject bullet = Instantiate(
                bulletPrefab,
                firePoint.position,
                firePoint.rotation
            );
        }
    }
}
