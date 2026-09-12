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

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !activada)
        {
            float parteSuperior = GetComponent<Collider>().bounds.max.y;

            foreach (ContactPoint contacto in collision.contacts)
            {
                if (contacto.point.y >= parteSuperior - 0.1f)
                {
                    activada = true;

                    chispas.Play();

                    Invoke("Caer", tiempoAntesDeCaer);

                    break;
                }
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

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.isKinematic = true;

        transform.position = posicionInicial;
        transform.rotation = rotacionInicial;

        activada = false;
    }
}