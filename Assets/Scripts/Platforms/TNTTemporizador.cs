using System.Collections;
using UnityEngine;

public class TNTTemporizador : MonoBehaviour
{
    public float tiempoExplosion = 10f;
    public float velocidadMinimaCaida = 1.5f;
    public GameObject prefabExplosion;
    public AudioClip sonidoExplosion;
    public BotonPuerta botonPuerta;

    [Header("Pulso visual")]
    public Transform modeloVisual;

    [Range(0f, 0.5f)]
    public float aumentoEscala = 0.12f;

    public float pulsosPorSegundoInicio = 1f;
    public float pulsosPorSegundoFinal = 4f;

    private bool activada = false;
    private bool estabaCayendo = false;
    private Rigidbody rb;

    private Vector3 escalaOriginal;
    private float tiempoPulso;
    private float fasePulso;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (modeloVisual != null)
            escalaOriginal = modeloVisual.localScale;
    }

    void FixedUpdate()
    {
        if (rb != null && rb.linearVelocity.y < -velocidadMinimaCaida)
            estabaCayendo = true;
    }

    void Update()
    {
        if (!activada || modeloVisual == null)
            return;

        tiempoPulso += Time.deltaTime;

        float progreso = Mathf.Clamp01(
            tiempoPulso / Mathf.Max(tiempoExplosion, 0.01f)
        );

        float frecuencia = Mathf.Lerp(
            pulsosPorSegundoInicio,
            pulsosPorSegundoFinal,
            progreso
        );

        fasePulso += frecuencia * Mathf.PI * 2f * Time.deltaTime;

        // Va suavemente de tamaño normal a inflado y vuelve.
        float pulso = (1f - Mathf.Cos(fasePulso)) * 0.5f;

        modeloVisual.localScale =
            escalaOriginal * (1f + pulso * aumentoEscala);
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
                tiempoPulso = 0f;
                fasePulso = 0f;

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

            ParticleSystem particulas =
                explosion.GetComponent<ParticleSystem>();

            if (particulas != null)
                particulas.Play();

            Destroy(explosion, 2f);
        }

        // Actualizar boton y puerta
        if (botonPuerta != null)
            botonPuerta.TNTExploto();

        Destroy(gameObject);
    }
}