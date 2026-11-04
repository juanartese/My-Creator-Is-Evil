using System.Collections;
using UnityEngine;

public class FlashOnHit : MonoBehaviour
{
    [Header("Flash Settings")]
    [Tooltip("Drag your Mat_FlashWhite material here")]
    public Material flashMaterial; 
    public float flashDuration = 0.1f;

    private Renderer[] renderers;
    private Material[][] originalMaterials;
    private Coroutine currentFlashRoutine;

    private void Awake()
    {
        // Find all 3D meshes on this enemy
        renderers = GetComponentsInChildren<Renderer>();
        originalMaterials = new Material[renderers.Length][];
        
        // Save their exact original UV materials so we can restore them later
        for (int i = 0; i < renderers.Length; i++)
        {
            originalMaterials[i] = renderers[i].materials;
        }
    }

    public void TriggerFlash()
    {
        if (currentFlashRoutine != null) StopCoroutine(currentFlashRoutine);
        currentFlashRoutine = StartCoroutine(DoFlash());
    }

    private IEnumerator DoFlash()
    {
        // 1. Swap every slot to the pure white material
        for (int i = 0; i < renderers.Length; i++)
        {
            Material[] flashMats = new Material[renderers[i].materials.Length];
            for (int m = 0; m < flashMats.Length; m++) flashMats[m] = flashMaterial;
            renderers[i].materials = flashMats;
        }

        // 2. Wait for a split second
        yield return new WaitForSeconds(flashDuration);

        // 3. Swap back to the original UV materials
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].materials = originalMaterials[i];
        }
        
        currentFlashRoutine = null;
    }
}