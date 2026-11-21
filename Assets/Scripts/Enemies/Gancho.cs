using UnityEngine;

public class Gancho : MonoBehaviour
{
    public float velocidad = 8f;
    public int danio = 25;

    public float tiempoPersecucion = 2f;

    private Transform puntoGancho;
    private Transform jugadorObjetivo;
    private Transform jugadorAtrapado;
    private PlayerHealth vidaJugadorAtrapado;

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
        vidaJugadorAtrapado = null;

        lanzado = true;
        volviendo = false;
        tiempoActualPersecucion = 0f;
    }

    private void Update()
    {
        if (puntoGancho == null)
            return;

        if (!lanzado)
        {
            transform.position = puntoGancho.position;
            return;
        }

        // Persigue al jugador.
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

            if (tiempoActualPersecucion >= tiempoPersecucion)
            {
                volviendo = true;
            }
        }
        else
        {
            // Regresa al enemigo.
            transform.position = Vector3.MoveTowards(
                transform.position,
                puntoGancho.position,
                velocidad * Time.deltaTime
            );

            if (jugadorAtrapado != null)
            {
                jugadorAtrapado.position = transform.position;
            }

            // Al llegar, hace daño una sola vez y lo suelta.
            if (Vector3.Distance(
                transform.position,
                puntoGancho.position
            ) < 0.1f)
            {
                transform.position = puntoGancho.position;

                PlayerHealth vidaParaDanar = vidaJugadorAtrapado;

                jugadorAtrapado = null;
                jugadorObjetivo = null;
                vidaJugadorAtrapado = null;

                lanzado = false;
                volviendo = false;
                tiempoActualPersecucion = 0f;

                if (vidaParaDanar != null)
                {
                    vidaParaDanar.RecibirDanio(danio);
                    Debug.Log("PLAYER LLEGO AL ENEMIGO - DANO: " + danio);
                }
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
            vidaJugadorAtrapado = other.GetComponentInParent<PlayerHealth>();

            volviendo = true;

            Debug.Log("PLAYER ATRAPADO - TODAVIA SIN DANO");
        }
    }
}