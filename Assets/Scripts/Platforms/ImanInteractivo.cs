
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
    public Vector3 posicionEnMano = new Vector3(0.6f, 0.5f, 0f);

    private bool recogido = false;
    private bool instalado = false;

    private void OnEnable()
    {
        if (interactInput != null)
        {
            interactInput.action.performed += Interactuar;
        }
    }

    private void OnDisable()
    {
        if (interactInput != null)
        {
            interactInput.action.performed -= Interactuar;
        }
    }

    private void Interactuar(InputAction.CallbackContext context)
    {
        if (PauseMenu.GameIsPaused || jugador == null)
            return;

        if (!recogido)
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
    }

    private void IntentarRecoger()
    {
        if (puntoInteraccion == null)
            return;

        float distancia = Vector3.Distance(
            jugador.position,
            puntoInteraccion.position
        );

        if (distancia > distanciaInteraccion)
            return;

        recogido = true;

        // Colocar el iman junto al jugador
        transform.SetParent(jugador);
        transform.localPosition = posicionEnMano;
        transform.localRotation = Quaternion.identity;

        // Desactivar las colisiones del iman
        Collider[] colliders = GetComponentsInChildren<Collider>();

        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }

        Debug.Log("IMAN: Recogido por el jugador");
    }

    private void IntentarInstalar()
    {
        if (puntoInstalacion == null)
        {
            Debug.LogWarning("IMAN: Falta PuntoInstalacion");
            return;
        }

        float distancia = Vector3.Distance(
            jugador.position,
            puntoInstalacion.position
        );

        Debug.Log("IMAN: Distancia al cable: " + distancia);

        if (distancia > distanciaInstalacion)
        {
            Debug.Log("IMAN: Acercate al cable para instalarlo");
            return;
        }

        instalado = true;

        // Enganchar el iman al punto de instalacion
        transform.SetParent(puntoInstalacion);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        Debug.Log("IMAN: Instalado correctamente en el cable");
    }

    private void IntentarSubir()
    {
        if (ascensor == null || puntoInstalacion == null)
        {
            Debug.LogWarning("IMAN: Falta referencia al ascensor");
            return;
        }

        float distancia = Vector3.Distance(
            jugador.position,
            puntoInstalacion.position
        );

        if (distancia > distanciaInstalacion)
        {
            Debug.Log("IMAN: Acercate al cable para subir");
            return;
        }

        if (ascensor.EstaOcupado())
            return;

        ascensor.IniciarAscenso();
    }
}
