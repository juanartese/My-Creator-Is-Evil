using UnityEngine;

public class PuertaLaboratorio : MonoBehaviour
{
    public float distanciaApertura = 5f;
    public float velocidad = 3f;

    private Vector3 posicionCerrada;
    private Vector3 posicionAbierta;

    private bool abrir = false;

    void Start()
    {
        posicionCerrada = transform.position;
        posicionAbierta = posicionCerrada + new Vector3(0, distanciaApertura, 0);
    }

    void Update()
    {
        if (abrir)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                posicionAbierta,
                velocidad * Time.deltaTime
            );
        }
        else
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                posicionCerrada,
                velocidad * Time.deltaTime
            );
        }
    }

    public void AbrirPuerta()
    {
        abrir = true;
    }

    public void CerrarPuerta()
    {
        abrir = false;
    }
}