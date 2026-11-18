using UnityEngine;

public class EnemigoGancho : MonoBehaviour, IDamageable

{



    [Header("Feedback")]

    public AudioSource hurtSound;

    public Gancho gancho;

    public Transform puntoGancho;



    public float tiempoEntreDisparos = 2f;

    public float velocidadGiro = 5f;



    // MOVIMIENTO DEL ENEMIGO

    public float velocidadMovimiento;

    public float distanciaMaximaPersecucion;

    public float distanciaMinimaJugador;



    public Renderer cilindroVisual;

    public Material materialVerde;

    public Material materialRojo;



    private Transform jugador;

    public GameObject Player;

    public GameObject jetPack;

    private bool jugadorDetectado = false;

    private float tiempoActual = 0f;

   

    // Changed from float to int to match the IDamageable system

    public int Life = 100;



    // Lugar donde estaba el enemigo al comenzar

    private Vector3 posicionInicial;



    private void Start()

    {

        gancho.Preparar(puntoGancho);

        cilindroVisual.material = materialVerde;

        posicionInicial = transform.position;

    }



    private void Update()

    {

        if (jugadorDetectado && jugador != null)

        {

            if (!gancho.EstaLanzado)

            {

                MirarAlJugador();

                PerseguirJugador();



                tiempoActual += Time.deltaTime;



                if (tiempoActual >= tiempoEntreDisparos)

                {

                    gancho.Lanzar(jugador);

                    tiempoActual = 0f;

                }

            }

        }

        else

        {

            VolverAPosicionInicial();

        }

       

    }



    private void MirarAlJugador()

    {

        Vector3 direccionJugador = jugador.position - transform.position;

        direccionJugador.y = 0f;



        Vector3 direccionPuntoGancho = puntoGancho.position - transform.position;

        direccionPuntoGancho.y = 0f;



        if (direccionJugador != Vector3.zero && direccionPuntoGancho != Vector3.zero)

        {

            float angulo = Vector3.SignedAngle(direccionPuntoGancho, direccionJugador, Vector3.up);

            Quaternion rotacionObjetivo = Quaternion.AngleAxis(angulo, Vector3.up) * transform.rotation;

            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, velocidadGiro * Time.deltaTime);

        }

    }



    private void PerseguirJugador()

    {

        float distanciaJugador = Vector3.Distance(transform.position, jugador.position);



        if (distanciaJugador <= distanciaMinimaJugador) return;



        Vector3 direccion = jugador.position - transform.position;

        direccion.y = 0f;

        direccion.Normalize();



        Vector3 siguientePosicion = transform.position + direccion * velocidadMovimiento * Time.deltaTime;

        float distanciaDesdeInicio = Vector3.Distance(posicionInicial, siguientePosicion);



        if (distanciaDesdeInicio <= distanciaMaximaPersecucion)

        {

            transform.position = siguientePosicion;

        }

    }



    private void VolverAPosicionInicial()

    {

        Vector3 destino = posicionInicial;

        destino.y = transform.position.y;

        float distancia = Vector3.Distance(transform.position, destino);



        if (distancia > 0.1f)

        {

            transform.position = Vector3.MoveTowards(transform.position, destino, velocidadMovimiento * Time.deltaTime);

        }

    }



    private void OnTriggerEnter(Collider other)

    {

        if (other.CompareTag("Player"))

        {

            jugador = other.transform;

            jugadorDetectado = true;

            cilindroVisual.material = materialRojo;

            tiempoActual = tiempoEntreDisparos - 0.3f;

        }

    }



    private void OnTriggerExit(Collider other)

    {

        if (other.CompareTag("Player"))

        {

            jugadorDetectado = false;

            cilindroVisual.material = materialVerde;

        }

    }



    public void TakeDamage(int amount)

    {

        Life -= amount;



        //FLASH ON HIT LINES HERE

        FlashOnHit flash = GetComponent<FlashOnHit>();

        if (flash != null) flash.TriggerFlash();



        // Audio Feedback

        if (hurtSound != null && hurtSound.clip != null)

        {

            hurtSound.pitch = Random.Range(0.8f, 1.2f);

           

            //This layers the sound instead of restarting it

            hurtSound.PlayOneShot(hurtSound.clip);

        }

       

        if (Life <= 0)

        {

            if (Player.GetComponent<PlayerMovement>().unlockJetpack == false)

            {

                Player.GetComponent<PlayerMovement>().unlockJetpack = true;

            }

           

            if (jetPack != null) jetPack.SetActive(true);

           

            Destroy(gameObject);

        }

    }

} 