using UnityEngine;

public class EnemigoGancho : MonoBehaviour
{
    public Gancho gancho;
    public Transform puntoGancho;

    public float tiempoEntreDisparos = 2f;
    public float velocidadGiro = 5f;

    // MOVIMIENTO DEL ENEMIGO
    public float velocidadMovimiento = 3f;
    public float distanciaMaximaPersecucion = 12f;
    public float distanciaMinimaJugador = 3f;

    public Renderer cilindroVisual;
    public Material materialVerde;
    public Material materialRojo;

    private Transform jugador;
    public GameObject Player;
    private bool jugadorDetectado = false;
    private float tiempoActual = 0f;
    public float Life;

    // Lugar donde estaba el enemigo al comenzar
    private Vector3 posicionInicial;

    private void Start()
    {
        gancho.Preparar(puntoGancho);

        cilindroVisual.material = materialVerde;

        // Guardamos su posicion original
        posicionInicial = transform.position;
    }

    private void Update()
    {
        if (jugadorDetectado && jugador != null)
        {
            // Mientras el gancho esta afuera,
            // el enemigo se queda quieto
            if (!gancho.EstaLanzado)
            {
                MirarAlJugador();
                PerseguirJugador();

                tiempoActual += Time.deltaTime;

                if (tiempoActual >= tiempoEntreDisparos)
                {
                    gancho.Lanzar(jugador);
                    tiempoActual = 0f;
                }
            }
        }
        else
        {
            // Si perdio al jugador, vuelve a su lugar
            VolverAPosicionInicial();
        }
        if (Life <= 0)
        {

            Player.GetComponent<PlayerMovement>().unlockJetpack = true;
            Destroy(gameObject);
        }
    }

    private void MirarAlJugador()
    {
        Vector3 direccionJugador =
            jugador.position - transform.position;

        direccionJugador.y = 0f;

        Vector3 direccionPuntoGancho =
            puntoGancho.position - transform.position;

        direccionPuntoGancho.y = 0f;

        if (direccionJugador != Vector3.zero &&
            direccionPuntoGancho != Vector3.zero)
        {
            float angulo = Vector3.SignedAngle(
                direccionPuntoGancho,
                direccionJugador,
                Vector3.up
            );

            Quaternion rotacionObjetivo =
                Quaternion.AngleAxis(angulo, Vector3.up)
                * transform.rotation;

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                rotacionObjetivo,
                velocidadGiro * Time.deltaTime
            );
        }
    }

    private void PerseguirJugador()
    {
        // No queremos que se meta encima del Player
        float distanciaJugador =
            Vector3.Distance(transform.position, jugador.position);

        if (distanciaJugador <= distanciaMinimaJugador)
        {
            return;
        }

        // Direccion hacia el jugador
        Vector3 direccion =
            jugador.position - transform.position;

        direccion.y = 0f;
        direccion.Normalize();

        Vector3 siguientePosicion =
            transform.position +
            direccion * velocidadMovimiento * Time.deltaTime;

        // Comprobamos cuanto se alejaria de su punto inicial
        float distanciaDesdeInicio =
            Vector3.Distance(posicionInicial, siguientePosicion);

        if (distanciaDesdeInicio <= distanciaMaximaPersecucion)
        {
            transform.position = siguientePosicion;
        }
    }

    private void VolverAPosicionInicial()
    {
        Vector3 destino = posicionInicial;
        destino.y = transform.position.y;

        float distancia =
            Vector3.Distance(transform.position, destino);

        if (distancia > 0.1f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                destino,
                velocidadMovimiento * Time.deltaTime
            );
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugador = other.transform;
            jugadorDetectado = true;

            cilindroVisual.material = materialRojo;

            tiempoActual = tiempoEntreDisparos - 0.3f;

            print("JUGADOR DETECTADO");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorDetectado = false;

            cilindroVisual.material = materialVerde;

            print("JUGADOR SALIO DEL RANGO");
        }
    }
}