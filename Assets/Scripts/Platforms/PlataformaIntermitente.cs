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

    private Renderer renderPlataforma;
    private Collider colliderPlataforma;

    void Start()
    {
        renderPlataforma = GetComponent<Renderer>();
        colliderPlataforma = GetComponent<Collider>();

        renderPlataforma.material = materialVerde;

        Invoke("CambiarAmarillo", tiempoVisible - tiempoAmarillo);
        Invoke("CambiarRojo", tiempoVisible - tiempoRojo);
        Invoke("Desaparecer", tiempoVisible);
    }

    void CambiarAmarillo()
    {
        renderPlataforma.material = materialAmarillo;
    }

    void CambiarRojo()
    {
        renderPlataforma.material = materialRojo;
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

        renderPlataforma.material = materialVerde;

        Invoke("CambiarAmarillo", tiempoVisible - tiempoAmarillo);
        Invoke("CambiarRojo", tiempoVisible - tiempoRojo);
        Invoke("Desaparecer", tiempoVisible);
    }
}