using UnityEngine;

public class Gancho : MonoBehaviour
{
    public float velocidad = 8f;
    public int danio = 25;

    // Tiempo maximo que el gancho puede perseguir al jugador
    public float tiempoPersecucion = 2f;

    private Transform puntoGancho;
    private Transform jugadorObjetivo;
    private Transform jugadorAtrapado;

    private bool lanzado = false;
    private bool volviendo = false;

    private float tiempoActualPersecucion = 0f;

    public bool EstaLanzado => lanzado;

    public void Preparar(Transform punto)
    {
        puntoGancho = punto;
    }

    public void Lanzar(Transform jugador)
    {
        transform.position = puntoGancho.position;

        jugadorObjetivo = jugador;
        jugadorAtrapado = null;

        lanzado = true;
        volviendo = false;

        tiempoActualPersecucion = 0f;
    }

    private void Update()
    {
        if (!lanzado)
        {
            transform.position = puntoGancho.position;
            return;
        }

        // =========================
        // IDA - PERSIGUE AL PLAYER
        // =========================
        if (!volviendo)
        {
            if (jugadorObjetivo != null)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    jugadorObjetivo.position,
                    velocidad * Time.deltaTime
                );
            }

            tiempoActualPersecucion += Time.deltaTime;

            // Si no logra atraparlo despues de cierto tiempo,
            // abandona la persecucion y vuelve
            if (tiempoActualPersecucion >= tiempoPersecucion)
            {
                volviendo = true;
            }
        }

        // =========================
        // VUELTA AL ENEMIGO
        // =========================
        else
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                puntoGancho.position,
                velocidad * Time.deltaTime
            );

            // Si atrapo al jugador, lo arrastra
            if (jugadorAtrapado != null)
            {
                jugadorAtrapado.position = transform.position;
            }

            // Llego al enemigo
            if (Vector3.Distance(
                transform.position,
                puntoGancho.position
            ) < 0.1f)
            {
                transform.position = puntoGancho.position;

                // Suelta al jugador
                jugadorAtrapado = null;
                jugadorObjetivo = null;

                lanzado = false;
                volviendo = false;

                tiempoActualPersecucion = 0f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") &&
            lanzado &&
            !volviendo)
        {
            jugadorAtrapado = other.transform;

            // Hace dano una sola vez al impactar
            PlayerHealth playerHealth =
                other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.RecibirDanio(danio);
            }

            // Deja de perseguir y vuelve arrastrando al Player
            volviendo = true;

            print("PLAYER ATRAPADO - DANO: " + danio);
        }
    }
}