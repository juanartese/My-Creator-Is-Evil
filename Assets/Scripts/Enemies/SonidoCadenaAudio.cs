
using UnityEngine;

public class SonidoCadena : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Gancho gancho;
    [SerializeField] private AudioSource audioCadena;

    private Vector3 posicionAnterior;

    void Start()
    {
        if (gancho != null)
            posicionAnterior = gancho.transform.position;

        if (audioCadena != null)
            audioCadena.Stop();
    }

    void LateUpdate()
    {
        if (gancho == null || audioCadena == null)
            return;

        Vector3 posicionActual = gancho.transform.position;

        bool seEstaMoviendo =
            (posicionActual - posicionAnterior).sqrMagnitude > 0.000001f;

        bool reproducir = gancho.EstaLanzado && seEstaMoviendo;

        if (reproducir && !audioCadena.isPlaying)
            audioCadena.Play();
        else if (!reproducir && audioCadena.isPlaying)
            audioCadena.Stop();

        posicionAnterior = posicionActual;
    }
}
