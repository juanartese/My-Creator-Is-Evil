using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ImanInteractivo : MonoBehaviour
{
    [Header("Referencias")]
    public Transform jugador;
    public Transform puntoInteraccion;
    public Transform puntoInstalacion;
    public AscensorMagnetico ascensor;
    public InputActionReference interactInput;

    [Header("Configuracion")]
    public float distanciaInteraccion = 3f;
    public float distanciaInstalacion = 3f;
    public Vector3 posicionEnMano;

    [Header("Holograma para recoger")]
    [SerializeField] private GameObject canvasInteracciones;

    [Header("Holograma del cable")]
    [SerializeField] private GameObject canvasCable;
    [SerializeField] private TMP_Text textoCable;

    [Header("Caja del iman")]
    public Transform puntoCaja;
    public GameObject tapaCaja;
    public Renderer indicadorCaja;
    public Material materialDisponible;
    public Material materialUsado;

    [Header("Holograma de la caja")]
    public GameObject canvasCaja;

    [Header("Salida del iman")]
    public float alturaSalida = 1.5f;
    public float duracionSalida = 0.8f;

    [Header("Sonido de apertura")]
    public AudioSource audioCaja;
    public AudioClip sonidoApertura;

    private bool cajaAbierta = false;
    private bool saliendo = false;
    private bool recogido = false;
    private bool instalado = false;

    private void Start()
    {
        MostrarCanvas(canvasInteracciones, false);
        MostrarCanvas(canvasCable, false);
        MostrarCanvas(canvasCaja, false);

        posicionEnMano = new Vector3(-1.2f, 3.7f, 0f);

        // Sin caja asignada, funciona como antes.
        cajaAbierta = puntoCaja == null;

        if (!cajaAbierta)
        {
            if (tapaCaja != null)
                tapaCaja.SetActive(true);

            if (indicadorCaja != null &&
                materialDisponible != null)
            {
                indicadorCaja.material = materialDisponible;
            }
        }
    }

    private void Update()
    {
        ActualizarAvisos();
    }

    private bool EstaCerca(Transform punto, float distancia)
    {
        return jugador != null &&
               punto != null &&
               Vector3.Distance(jugador.position, punto.position)
               <= distancia;
    }

    private void ActualizarAvisos()
    {
        bool mostrarRecoger = false;
        bool mostrarCable = false;
        bool mostrarCaja = false;

        if (jugador != null && !PauseMenu.GameIsPaused)
        {
            mostrarCaja = !cajaAbierta &&
                EstaCerca(puntoCaja, distanciaInteraccion);

            mostrarRecoger = cajaAbierta &&
                !saliendo &&
                !recogido &&
                EstaCerca(puntoInteraccion, distanciaInteraccion);

            mostrarCable = recogido &&
                ascensor != null &&
                !ascensor.EstaOcupado() &&
                EstaCerca(puntoInstalacion, distanciaInstalacion);
        }

        if (textoCable != null)
        {
            textoCable.text = instalado
                ? "RIDE UP"
                : "ATTACH MAGNET";
        }

        MostrarCanvas(canvasCaja, mostrarCaja);
        MostrarCanvas(canvasInteracciones, mostrarRecoger);
        MostrarCanvas(canvasCable, mostrarCable);
    }

    private void MostrarCanvas(GameObject canvas, bool mostrar)
    {
        if (canvas != null && canvas.activeSelf != mostrar)
            canvas.SetActive(mostrar);
    }

    public bool EstaRecogido()
    {
        return recogido;
    }

    private void OnEnable()
    {
        if (interactInput != null)
            interactInput.action.performed += Interactuar;
    }

    private void OnDisable()
    {
        if (interactInput != null)
            interactInput.action.performed -= Interactuar;

        MostrarCanvas(canvasCaja, false);
        MostrarCanvas(canvasInteracciones, false);
        MostrarCanvas(canvasCable, false);
    }

    private void Interactuar(InputAction.CallbackContext context)
    {
        if (PauseMenu.GameIsPaused || jugador == null || saliendo)
            return;

        if (!cajaAbierta)
        {
            IntentarAbrirCaja();
        }
        else if (!recogido)
        {
            IntentarRecoger();
        }
        else if (!instalado)
        {
            IntentarInstalar();
        }
        else
        {
            IntentarSubir();
        }

        ActualizarAvisos();
    }

    private void IntentarAbrirCaja()
    {
        if (!EstaCerca(puntoCaja, distanciaInteraccion))
            return;

        cajaAbierta = true;
        saliendo = true;

        if (tapaCaja != null)
            tapaCaja.SetActive(false);

        if (indicadorCaja != null && materialUsado != null)
            indicadorCaja.material = materialUsado;

        if (audioCaja != null && sonidoApertura != null)
            audioCaja.PlayOneShot(sonidoApertura);

        StartCoroutine(SacarIman());

        Debug.Log("MAGNET BOX: Opened");
    }

    private IEnumerator SacarIman()
    {
        Vector3 inicio = transform.position;
        Vector3 destino = inicio + Vector3.up * alturaSalida;
        float tiempo = 0f;

        while (tiempo < duracionSalida)
        {
            if (!PauseMenu.GameIsPaused)
            {
                tiempo += Time.deltaTime;

                float progreso = Mathf.Clamp01(
                    tiempo / duracionSalida
                );

                transform.position = Vector3.Lerp(
                    inicio,
                    destino,
                    Mathf.SmoothStep(0f, 1f, progreso)
                );
            }

            yield return null;
        }

        transform.position = destino;
        saliendo = false;
        ActualizarAvisos();

        Debug.Log("MAGNET: Ready to pick up");
    }

    private void IntentarRecoger()
    {
        if (!cajaAbierta || saliendo ||
            !EstaCerca(puntoInteraccion, distanciaInteraccion))
            return;

        recogido = true;

        transform.SetParent(jugador);
        transform.localPosition = posicionEnMano;
        transform.localRotation = Quaternion.identity;

        Collider[] colliders = GetComponentsInChildren<Collider>();

        foreach (Collider col in colliders)
            col.enabled = false;

        Debug.Log("MAGNET: Picked up");
    }

    private void IntentarInstalar()
    {
        if (!EstaCerca(puntoInstalacion, distanciaInstalacion))
            return;

        instalado = true;

        transform.SetParent(puntoInstalacion);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        Debug.Log("MAGNET: Attached to cable");
    }

    private void IntentarSubir()
    {
        if (ascensor == null ||
            !EstaCerca(puntoInstalacion, distanciaInstalacion) ||
            ascensor.EstaOcupado())
            return;

        ascensor.IniciarAscenso();
    }
}