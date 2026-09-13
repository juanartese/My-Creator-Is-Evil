using UnityEngine;

public class EstacionVida : MonoBehaviour
{
    public int cantidadVida = 25;
    public float tiempoEntreCarga = 1f;

    public Renderer luzEstado;
    public Material materialVerde;
    public Material materialRojo;

    public GameObject rayoCarga;

    private float tiempoActual = 0f;
    private bool jugadorEnEstacion = false;
    private bool estacionUsada = false;

    private PlayerHealth playerHealth;

    private void Start()
    {
        luzEstado.material = materialVerde;
        rayoCarga.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnEstacion = true;
            tiempoActual = 0f;

            playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null && !estacionUsada &&
                playerHealth.vidaActual < playerHealth.vidaMaxima)
            {
                rayoCarga.SetActive(true);
            }

            print("JUGADOR ENTRO A LA ESTACION");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnEstacion = false;
            tiempoActual = 0f;
            playerHealth = null;

            rayoCarga.SetActive(false);

            print("JUGADOR SALIO DE LA ESTACION");
        }
    }

    private void Update()
    {
        if (jugadorEnEstacion && playerHealth != null && !estacionUsada)
        {
            if (playerHealth.vidaActual < playerHealth.vidaMaxima)
            {
                rayoCarga.SetActive(true);

                tiempoActual += Time.deltaTime;

                if (tiempoActual >= tiempoEntreCarga)
                {
                    playerHealth.RecuperarVida(cantidadVida);
                    tiempoActual = 0f;

                    if (playerHealth.vidaActual >= playerHealth.vidaMaxima)
                    {
                        estacionUsada = true;

                        rayoCarga.SetActive(false);
                        luzEstado.material = materialRojo;

                        print("ESTACION AGOTADA");
                    }
                }
            }
            else
            {
                rayoCarga.SetActive(false);
            }
        }
    }
}