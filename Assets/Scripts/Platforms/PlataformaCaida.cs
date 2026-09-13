using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlataformaCaida : MonoBehaviour
{
    public float tiempoAntesDeCaer = 1f;
    public float tiempoParaVolver = 3f;

    public ParticleSystem chispas;

    private Rigidbody rb;
    private Vector3 posicionInicial;
    private Quaternion rotacionInicial;
    private Renderer renderPlataforma;
    private bool activada = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        renderPlataforma = GetComponent<Renderer>();
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
                    StartCoroutine(Parpadeo(rb));
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
        StopAllCoroutines();
        renderPlataforma.enabled = true;
        activada = false;
    }

    private IEnumerator Parpadeo(Rigidbody rb)
    {
        while (rb.useGravity = true)
        {
            yield return new WaitForSeconds(0.2f);

            renderPlataforma.enabled = true;

            yield return new WaitForSeconds(0.32f);
            renderPlataforma.enabled = false;
        }
    }
}