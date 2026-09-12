using UnityEngine;

public class EnemigoGancho : MonoBehaviour
{
    public Gancho gancho;
    public Transform puntoGancho;

    public float tiempoEntreDisparos = 2f;
    public float velocidadGiro = 5f;

    public Renderer cilindroVisual;
    public Material materialVerde;
    public Material materialRojo;

    private Transform jugador;
    private bool jugadorDetectado = false;
    private float tiempoActual = 0f;

    private void Start()
    {
        gancho.Preparar(puntoGancho);

        cilindroVisual.material = materialVerde;
    }

    private void Update()
    {
        if (jugadorDetectado)
        {
            if (!gancho.EstaLanzado)
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

                tiempoActual += Time.deltaTime;

                if (tiempoActual >= tiempoEntreDisparos)
                {
                    gancho.Lanzar(jugador);
                    tiempoActual = 0f;
                }
            }
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