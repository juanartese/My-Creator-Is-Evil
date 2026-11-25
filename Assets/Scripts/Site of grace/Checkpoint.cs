using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public static Vector3 savedPosition;
    public static Quaternion savedRotation;
    public static bool hasCheckpoint = false;

    public static bool savedDoubleJump = false;
    public static bool savedJetpack = false;
    public static bool savedHook = false;

    [Header("Visual & Audio Feedback")]
    public GameObject activeVisuals; // Drag your Fire Particle System here
    public AudioSource activateSound;
    public Color activatedColor = new Color(1f, 0.75f, 0f); // Gold/Orange
    public float spinSpeed = 90f;

    private bool isActivated = false;
    private Renderer cubeRenderer;

    private void Start()
    {
        cubeRenderer = GetComponent<Renderer>();

        if (hasCheckpoint)
        {
            // Keep it lit and spinning if the player just respawned here
            ActivateVisuals();
        }
        else if (activeVisuals != null)
        {
            activeVisuals.SetActive(false);
        }
    }

    private void Update()
    {
        if (isActivated)
        {
            // Spin on Y and slightly tilt-spin on X for a floating relic look
            transform.Rotate(Vector3.up * spinSpeed * Time.deltaTime, Space.World);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isActivated || !other.CompareTag("Player")) return;

        hasCheckpoint = true;
        savedPosition = transform.position + Vector3.up * 0.5f;
        savedRotation = transform.rotation;

        PlayerMovement pm = other.GetComponent<PlayerMovement>();
        if (pm != null)
        {
            savedDoubleJump = pm.unlockDoubleJump;
            savedJetpack = pm.unlockJetpack;
            savedHook = pm.unlockHook;
        }

        ActivateVisuals();

        if (activateSound != null && activateSound.clip != null)
        {
            activateSound.PlayOneShot(activateSound.clip);
        }

        Debug.Log("Checkpoint Reached! Position and abilities saved.");
    }

    private void ActivateVisuals()
    {
        isActivated = true;

        if (activeVisuals != null)
        {
            activeVisuals.SetActive(true);
        }

        if (cubeRenderer != null)
        {
            cubeRenderer.material.color = activatedColor;
        }
    }

    public static void ClearCheckpoint()
    {
        hasCheckpoint = false;
        savedDoubleJump = false;
        savedJetpack = false;
        savedHook = false;
    }
}