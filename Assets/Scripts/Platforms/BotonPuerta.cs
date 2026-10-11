
using UnityEngine;

public class BotonPuerta : MonoBehaviour
{
    public Material materialRojo;
    public Material materialVerde;

    public Material luzIndicadoraRoja;
    public Material luzIndicadoraVerde;

    public PuertaLaboratorio puerta;
    public Light luzPuerta;

    public Renderer luzIndicadora;

    private Renderer renderBoton;
    private Vector3 posicionInicial;

    private int objetosEncima = 0;

    void Start()
    {
        renderBoton = GetComponent<Renderer>();
        posicionInicial = transform.position;

        luzPuerta.color = Color.red;
        luzIndicadora.material = luzIndicadoraRoja;
    }

    void OnTriggerEnter(Collider otro)
    {
        if (otro.gameObject.name == "Player" || otro.CompareTag("ObjetoActivador"))
        {
            objetosEncima++;

            ActualizarBoton();

            print("Boton activado");
        }
    }

    void OnTriggerExit(Collider otro)
    {
        if (otro.gameObject.name == "Player" || otro.CompareTag("ObjetoActivador"))
        {
            objetosEncima--;

            if (objetosEncima < 0)
            {
                objetosEncima = 0;
            }

            ActualizarBoton();

            print("Boton desactivado");
        }
    }

    public void TNTExploto()
    {
        if (objetosEncima > 0)
        {
            objetosEncima--;
        }

        ActualizarBoton();
    }

    void ActualizarBoton()
    {
        if (objetosEncima > 0)
        {
            renderBoton.material = materialVerde;
            transform.position = posicionInicial + new Vector3(0, -0.05f, 0);

            luzPuerta.color = Color.green;
            luzIndicadora.material = luzIndicadoraVerde;

            puerta.AbrirPuerta();
        }
        else
        {
            renderBoton.material = materialRojo;
            transform.position = posicionInicial;

            luzPuerta.color = Color.red;
            luzIndicadora.material = luzIndicadoraRoja;

            puerta.CerrarPuerta();
        }
    }
}
