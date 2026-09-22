using UnityEngine;

public class Gancho : MonoBehaviour
{
    public float velocidad = 8f;

    private Transform puntoGancho;

    private Vector3 objetivo;
    private bool lanzado = false;
    private bool volviendo = false;

    public bool EstaLanzado => lanzado;

    public void Preparar(Transform punto)
    {
        puntoGancho = punto;
    }

    public void Lanzar(Transform jugador)
    {
        transform.position = puntoGancho.position;

        objetivo = jugador.position;

        lanzado = true;
        volviendo = false;
    }

    void Update()
    {
        if (!lanzado)
        {
            transform.position = puntoGancho.position;
            return;
        }

        if (!volviendo)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                objetivo,
                velocidad * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, objetivo) < 0.1f)
            {
                volviendo = true;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                puntoGancho.position,
                velocidad * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, puntoGancho.position) < 0.1f)
            {
                transform.position = puntoGancho.position;

                lanzado = false;
                volviendo = false;
            }
        }
    }
}