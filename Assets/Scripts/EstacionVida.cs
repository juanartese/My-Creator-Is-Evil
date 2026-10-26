
using UnityEngine;
using UnityEngine.UI;

public class EstacionVida : MonoBehaviour
{
    public int cantidadVida = 25;
    public float tiempoEntreCarga = 1f;

    public Renderer luzEstado;
    public Material materialVerde;
    public Material materialRojo;

    public Image llaveCarga;

    private float tiempoActual = 0f;
    private bool jugadorEnEstacion = false;
    private bool estacionUsada = false;

    private PlayerHealth playerHealth;

    private void Start()
    {
        // Buscar la llave si no esta asignada
        if (llaveCarga == null)
        {
            GameObject objetoLlave = GameObject.Find("LlaveCarga");

            if (objetoLlave != null)
            {
                llaveCarga = objetoLlave.GetComponent<Image>();
            }
        }

        if (luzEstado != null && materialVerde != null)
        {
            luzEstado.material = materialVerde;
        }

        if (llaveCarga != null)
        {
            llaveCarga.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnEstacion = true;
            tiempoActual = 0f;

            playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null &&
                !estacionUsada &&
                playerHealth.vidaActual < playerHealth.vidaMaxima)
            {
                if (llaveCarga != null)
                {
                    llaveCarga.enabled = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnEstacion = false;
            tiempoActual = 0f;
            playerHealth = null;

            if (llaveCarga != null)
            {
                llaveCarga.enabled = false;
            }
        }
    }

    private void Update()
    {
        if (jugadorEnEstacion && playerHealth != null && !estacionUsada)
        {
            if (playerHealth.vidaActual < playerHealth.vidaMaxima)
            {
                if (llaveCarga != null)
                {
                    llaveCarga.enabled = true;
                }

                tiempoActual += Time.deltaTime;

                if (tiempoActual >= tiempoEntreCarga)
                {
                    playerHealth.RecuperarVida(cantidadVida);
                    tiempoActual = 0f;

                    if (playerHealth.vidaActual >= playerHealth.vidaMaxima)
                    {
                        estacionUsada = true;

                        if (llaveCarga != null)
                        {
                            llaveCarga.enabled = false;
                        }

                        if (luzEstado != null && materialRojo != null)
                        {
                            luzEstado.material = materialRojo;
                        }
                    }
                }
            }
        }
    }
}
