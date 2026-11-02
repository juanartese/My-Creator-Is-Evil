
using UnityEngine;

public class PulsoHolografico : MonoBehaviour
{
    [SerializeField] private float velocidad = 2f;
    [SerializeField] private float intensidad = 0.05f;

    private Vector3 escalaOriginal;

    void Start()
    {
        escalaOriginal = transform.localScale;
    }

    void Update()
    {
        float pulso = 1f + Mathf.Sin(Time.time * velocidad) * intensidad;

        transform.localScale = escalaOriginal * pulso;
    }
}
