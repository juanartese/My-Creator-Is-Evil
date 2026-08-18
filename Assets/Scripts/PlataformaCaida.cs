using UnityEngine;

public class PlataformaCaida : MonoBehaviour
{
    public float tiempoAntesDeCaer = 1f;
    public float tiempoParaVolver = 3f;

    public ParticleSystem chispas;

    private Rigidbody rb;
    private Vector3 posicionInicial;
    private Quaternion rotacionInicial;

    private bool activada = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        posicionInicial = transform.position;
        rotacionInicial = transform.rotation;

        chispas.Stop();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player" && !activada)
        {
            ContactPoint contacto = collision.contacts[0];

            float parteSuperior = GetComponent<Collider>().bounds.max.y;

            if (contacto.point.y >= parteSuperior - 0.1f)
            {
                activada = true;

                chispas.Play();

                Invoke("Caer", tiempoAntesDeCaer);
            }
        }
    }

    void Caer()
    {
        chispas.Stop();

        rb.useGravity = true;
        rb.isKinematic = false;

        Invoke("Volver", tiempoParaVolver);
    }

    void Volver()
    {
        rb.useGravity = false;
        rb.isKinematic = true;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        transform.position = posicionInicial;
        transform.rotation = rotacionInicial;

        activada = false;
    }
}