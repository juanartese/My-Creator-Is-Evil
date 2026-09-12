using UnityEngine;

public class RayoElectrico : MonoBehaviour
{
    private float tiempo = 0f;
    private float tiempoCambio;
    private LineRenderer rayo;

    void Start()
    {
        rayo = GetComponent<LineRenderer>();

        tiempoCambio = Random.Range(0.1f, 0.5f);
    }

    void Update()
    {
        tiempo += Time.deltaTime;

        if (tiempo >= tiempoCambio)
        {
            rayo.enabled = !rayo.enabled;

            tiempo = 0f;
            tiempoCambio = Random.Range(0.1f, 0.5f);
        }
    }
}