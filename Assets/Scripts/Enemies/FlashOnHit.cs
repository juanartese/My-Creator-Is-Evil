using System.Collections;
using UnityEngine;

public class FlashOnHit : MonoBehaviour
{
    [Header("Flash Settings")]
    [SerializeField] private Color flashColor = Color.white;
    [SerializeField] private float flashDuration = 0.1f;

    [Header("Hit Detection Filter")]
    [Tooltip("Leave blank to trigger on anything, or type the tag of your bullet/projectile (e.g., Bullet)")]
    [SerializeField] private string projectileTag = "";

    private Renderer[] renderers;
    private Color[][] defaultColors;
    private Coroutine currentFlashRoutine;

    // URP Lit uses _BaseColor; Built-in standard uses _Color
    private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");

    private void Awake()
    {
        // Finds all renderers on this object and any children
        renderers = GetComponentsInChildren<Renderer>();

        // Store initial colors so we can restore them cleanly
        defaultColors = new Color[renderers.Length][];
        for (int i = 0; i < renderers.Length; i++)
        {
            Material[] mats = renderers[i].materials;
            defaultColors[i] = new Color[mats.Length];

            for (int m = 0; m < mats.Length; m++)
            {
                if (mats[m].HasProperty(BaseColorId))
                {
                    defaultColors[i][m] = mats[m].GetColor(BaseColorId);
                }
                else if (mats[m].HasProperty("_Color"))
                {
                    defaultColors[i][m] = mats[m].GetColor("_Color");
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (ShouldTrigger(collision.gameObject))
        {
            TriggerFlash();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (ShouldTrigger(other.gameObject))
        {
            TriggerFlash();
        }
    }

    private bool ShouldTrigger(GameObject hitObject)
    {
        if (!string.IsNullOrEmpty(projectileTag))
        {
            return hitObject.CompareTag(projectileTag);
        }

        return !hitObject.name.ToLower().Contains("floor") && !hitObject.name.ToLower().Contains("plane");
    }

    public void TriggerFlash()
    {
        if (currentFlashRoutine != null)
        {
            StopCoroutine(currentFlashRoutine);
        }
        currentFlashRoutine = StartCoroutine(DoFlash());
    }

    private IEnumerator DoFlash()
    {
        ApplyColor(flashColor, false);
        yield return new WaitForSeconds(flashDuration);
        ApplyColor(Color.clear, true);
        currentFlashRoutine = null;
    }

    private void ApplyColor(Color color, bool restoreDefaults)
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            Material[] mats = renderers[i].materials;
            for (int m = 0; m < mats.Length; m++)
            {
                int prop = mats[m].HasProperty(BaseColorId) ? BaseColorId : Shader.PropertyToID("_Color");
                mats[m].SetColor(prop, restoreDefaults ? defaultColors[i][m] : color);
            }
        }
    }
}