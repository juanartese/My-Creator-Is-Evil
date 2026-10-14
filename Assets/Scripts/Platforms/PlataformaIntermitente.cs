
using UnityEngine;

public class PlataformaIntermitente : MonoBehaviour
{
    public float tiempoVisible = 2f;
    public float tiempoInvisible = 2f;

    public Material materialVerde;
    public Material materialAmarillo;
    public Material materialRojo;

    public float tiempoAmarillo = 1f;
    public float tiempoRojo = 0.5f;

    // Referencias para el nuevo modelo
    public Renderer[] renderersLED;
    public GameObject modeloPlataforma;

    private Renderer renderPlataforma;
    private Collider colliderPlataforma;

    void Start()
    {
        renderPlataforma = GetComponent<Renderer>();
        colliderPlataforma = GetComponent<Collider>();

        CambiarMaterial(materialVerde);

        Invoke(nameof(CambiarAmarillo),
            Mathf.Max(0f, tiempoVisible - tiempoAmarillo));

        Invoke(nameof(CambiarRojo),
            Mathf.Max(0f, tiempoVisible - tiempoRojo));

        Invoke(nameof(Desaparecer), tiempoVisible);
    }

    void CambiarMaterial(Material material)
    {
        // Nuevo modelo: cambiar solamente los LEDs
        if (renderersLED != null && renderersLED.Length > 0)
        {
            foreach (Renderer led in renderersLED)
            {
                if (led != null)
                    led.material = material;
            }
        }
        // Plataformas anteriores: cambiar el cubo completo
        else if (renderPlataforma != null)
        {
            renderPlataforma.material = material;
        }
    }

    void CambiarAmarillo()
    {
        CambiarMaterial(materialAmarillo);
    }

    void CambiarRojo()
    {
        CambiarMaterial(materialRojo);
    }

    void Desaparecer()
    {
        // Ocultar el nuevo modelo completo
        if (modeloPlataforma != null)
        {
            modeloPlataforma.SetActive(false);
        }
        // Compatibilidad con las plataformas anteriores
        else if (renderPlataforma != null)
        {
            renderPlataforma.enabled = false;
        }

        if (colliderPlataforma != null)
            colliderPlataforma.enabled = false;

        Invoke(nameof(Aparecer), tiempoInvisible);
    }

    void Aparecer()
    {
        // Mostrar el nuevo modelo completo
        if (modeloPlataforma != null)
        {
            modeloPlataforma.SetActive(true);
        }
        else if (renderPlataforma != null)
        {
            renderPlataforma.enabled = true;
        }

        if (colliderPlataforma != null)
            colliderPlataforma.enabled = true;

        CambiarMaterial(materialVerde);

        Invoke(nameof(CambiarAmarillo),
            Mathf.Max(0f, tiempoVisible - tiempoAmarillo));

        Invoke(nameof(CambiarRojo),
            Mathf.Max(0f, tiempoVisible - tiempoRojo));

        Invoke(nameof(Desaparecer), tiempoVisible);
    }
}
