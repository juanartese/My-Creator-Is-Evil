
using System.Collections;
using UnityEngine;

public class TNTTemporizador : MonoBehaviour
{
    public float tiempoExplosion = 10f;
    public float velocidadMinimaCaida = 1.5f;
    public GameObject prefabExplosion;
    public AudioClip sonidoExplosion;
    public BotonPuerta botonPuerta;

    private bool activada = false;
    private bool estabaCayendo = false;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (rb.linearVelocity.y < -velocidadMinimaCaida)
            estabaCayendo = true;
    }

    void OnCollisionEnter(Collision colision)
    {
        if (activada || !estabaCayendo)
            return;

        foreach (ContactPoint contacto in colision.contacts)
        {
            if (Vector3.Dot(contacto.normal, Vector3.up) > 0.5f)
            {
                activada = true;
                StartCoroutine(CuentaRegresiva());
                break;
            }
        }
    }

    IEnumerator CuentaRegresiva()
    {
        float tiempoRestante = tiempoExplosion;

        while (tiempoRestante > 0f)
        {
            print("TNT: " + Mathf.CeilToInt(tiempoRestante));

            yield return new WaitForSeconds(1f);

            tiempoRestante -= 1f;
        }

        print("BOOM! La TNT exploto");

        // Reproducir sonido desde la camara
        if (sonidoExplosion != null)
        {
            if (Camera.main != null)
            {
                AudioSource.PlayClipAtPoint(
                    sonidoExplosion,
                    Camera.main.transform.position,
                    1f
                );
            }
            else
            {
                AudioSource.PlayClipAtPoint(
                    sonidoExplosion,
                    transform.position,
                    1f
                );
            }
        }

        // Crear efecto visual de explosion
        if (prefabExplosion != null)
        {
            GameObject explosion = Instantiate(
                prefabExplosion,
                transform.position,
                Quaternion.identity
            );

            ParticleSystem particulas = explosion.GetComponent<ParticleSystem>();

            if (particulas != null)
                particulas.Play();

            Destroy(explosion, 2f);
        }

        // Actualizar boton y puerta
        if (botonPuerta != null)
            botonPuerta.TNTExploto();

        // Destruir TNT
        Destroy(gameObject);
    }
}
