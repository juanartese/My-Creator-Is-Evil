using UnityEngine;

public class BotonPuerta : MonoBehaviour
{
    public Material materialRojo;
    public Material materialVerde;

    public PuertaLaboratorio puerta;

    private Renderer renderBoton;
    private Vector3 posicionInicial;

    private int objetosEncima = 0;

    void Start()
    {
        renderBoton = GetComponent<Renderer>();
        posicionInicial = transform.position;
    }

    void OnTriggerEnter(Collider otro)
    {
        if (otro.gameObject.name == "Player" || otro.CompareTag("ObjetoActivador"))
        {
            objetosEncima++;

            renderBoton.material = materialVerde;
            transform.position = posicionInicial + new Vector3(0, -0.05f, 0);

            puerta.AbrirPuerta();

            print("Boton activado");
        }
    }

    void OnTriggerExit(Collider otro)
    {
        if (otro.gameObject.name == "Player" || otro.CompareTag("ObjetoActivador"))
        {
            objetosEncima--;

            if (objetosEncima <= 0)
            {
                objetosEncima = 0;

                renderBoton.material = materialRojo;
                transform.position = posicionInicial;

                puerta.CerrarPuerta();

                print("Boton desactivado");
            }
        }
    }
}