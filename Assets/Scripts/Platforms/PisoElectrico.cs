using UnityEngine;

public class PisoElectrico : MonoBehaviour
{
    private float tiempoEntreDanio = 1f;
    private float tiempoActual = 1f;
    private bool jugadorEnPiso = false;

    private PlayerHealth playerHealth;

    public ParticleSystem chispasElectricas;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnPiso = true;
            tiempoActual = 0.5f;

            playerHealth = other.GetComponent<PlayerHealth>();

            Debug.Log("ENTRO AL PISO ELECTRICO");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnPiso = false;
            tiempoActual = 1f;
            playerHealth = null;

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
                Debug.Log("JUGADOR RECIBE DANO ELECTRICO");

                if (playerHealth != null)
                {
                    playerHealth.RecibirDanio(25);

                    if (chispasElectricas != null)
                    {
                        Debug.Log("CHISPAS ACTIVADAS");
                        chispasElectricas.Play();
                    }
                }

                tiempoActual = 0f;
            }
        }
    }
}