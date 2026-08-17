using UnityEngine;

public class PlataformaIntermitente : MonoBehaviour
{
    public float tiempoVisible = 2f;
    public float tiempoInvisible = 2f;

    private Renderer renderPlataforma;
    private Collider colliderPlataforma;

    void Start()
    {
        renderPlataforma = GetComponent<Renderer>();
        colliderPlataforma = GetComponent<Collider>();

        Invoke("Desaparecer", tiempoVisible);
    }

    void Desaparecer()
    {
        renderPlataforma.enabled = false;
        colliderPlataforma.enabled = false;

        Invoke("Aparecer", tiempoInvisible);
    }

    void Aparecer()
    {
        renderPlataforma.enabled = true;
        colliderPlataforma.enabled = true;

        Invoke("Desaparecer", tiempoVisible);
    }
}