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

    [Header("Pitido")]
    [Range(0f, 1f)]
    public float volumenPitido = 0.25f;

    private bool activada;
    private bool estabaCayendo;
    private Rigidbody rb;

    private Vector3 escalaOriginal;
    private float tiempoPulso;
    private float fasePulso;

    private AudioSource fuentePitido;
    private AudioClip clipPitido;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (modeloVisual != null)
            escalaOriginal = modeloVisual.localScale;

        CrearPitido();

        // Fuente propia para no modificar otros sonidos de la caja.
        fuentePitido = gameObject.AddComponent<AudioSource>();
        fuentePitido.playOnAwake = false;
        fuentePitido.loop = false;
        fuentePitido.spatialBlend = 1f;
        fuentePitido.rolloffMode = AudioRolloffMode.Linear;
        fuentePitido.minDistance = 3f;
        fuentePitido.maxDistance = 30f;
        fuentePitido.dopplerLevel = 0f;
    }

    void CrearPitido()
    {
        int frecuenciaMuestreo = 44100;
        float duracion = 0.08f;
        float tono = 1000f;

        int cantidadMuestras =
            Mathf.RoundToInt(frecuenciaMuestreo * duracion);

        float[] muestras = new float[cantidadMuestras];

        for (int i = 0; i < cantidadMuestras; i++)
        {
            float tiempo = (float)i / frecuenciaMuestreo;

            // Entrada y salida suaves para evitar chasquidos.
            float envolvente = Mathf.Sin(
                Mathf.PI * i / (cantidadMuestras - 1)
            );

            muestras[i] =
                Mathf.Sin(2f * Mathf.PI * tono * tiempo)
                * envolvente
                * 0.5f;
        }

        clipPitido = AudioClip.Create(
            "Pitido TNT",
            cantidadMuestras,
            1,
            frecuenciaMuestreo,
            false
        );

        clipPitido.SetData(muestras, 0);
    }

    void ReproducirPitido()
    {
        if (fuentePitido != null && clipPitido != null)
            fuentePitido.PlayOneShot(clipPitido, volumenPitido);
    }

    void FixedUpdate()
    {
        if (rb != null && rb.linearVelocity.y < -velocidadMinimaCaida)
            estabaCayendo = true;
    }

    void Update()
    {
        if (!activada)
            return;

        tiempoPulso += Time.deltaTime;

        float progreso = Mathf.Clamp01(
            tiempoPulso / Mathf.Max(tiempoExplosion, 0.01f)
        );

        float frecuencia = Mathf.Max(0f, Mathf.Lerp(
            pulsosPorSegundoInicio,
            pulsosPorSegundoFinal,
            progreso
        ));

        float vueltaCompleta = Mathf.PI * 2f;

        fasePulso += frecuencia * vueltaCompleta * Time.deltaTime;

        // Una vuelta equivale a inflarse y desinflarse.
        if (fasePulso >= vueltaCompleta)
        {
            fasePulso %= vueltaCompleta;
            ReproducirPitido();
        }

        if (modeloVisual != null)
        {
            float pulso = (1f - Mathf.Cos(fasePulso)) * 0.5f;

            modeloVisual.localScale =
                escalaOriginal * (1f + pulso * aumentoEscala);
        }
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

                ReproducirPitido();
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

        activada = false;
        fuentePitido.Stop();

        print("BOOM! La TNT exploto");

        if (sonidoExplosion != null)
        {
            Vector3 posicionSonido = Camera.main != null
                ? Camera.main.transform.position
                : transform.position;

            AudioSource.PlayClipAtPoint(
                sonidoExplosion,
                posicionSonido,
                1f
            );
        }

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

        if (botonPuerta != null)
            botonPuerta.TNTExploto();

        Destroy(gameObject);
    }

    void OnDestroy()
    {
        if (clipPitido != null)
            Destroy(clipPitido);
    }
}