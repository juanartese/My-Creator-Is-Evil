using UnityEngine;

public class PisoElectrico : MonoBehaviour
{
    private float tiempoEntreDanio = 1f;
    private float tiempoActual = 0f;
    private bool jugadorEnPiso = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnPiso = true;
            tiempoActual = 0f;

            Debug.Log("ENTRO AL PISO ELECTRICO");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnPiso = false;
            tiempoActual = 0f;

            Debug.Log("SALIO DEL PISO ELECTRICO");
        }
    }

    private void Update()
    {
        if (jugadorEnPiso)
        {
            tiempoActual += Time.deltaTime;

            if (tiempoActual >= tiempoEntreDanio)
            {
                Debug.Log("Jugador recibe daño electrico");
                tiempoActual = 0f;
            }
        }
    }
}