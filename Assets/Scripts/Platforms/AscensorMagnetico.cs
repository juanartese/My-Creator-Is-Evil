
using System.Collections;
using UnityEngine;

public class AscensorMagnetico : MonoBehaviour
{
    [Header("Puntos del recorrido")]
    public Transform puntoInicio;
    public Transform puntoFinal;

    [Header("Configuracion")]
    public float velocidadAscenso = 5f;
    public float velocidadDescenso = 7f;
    public float separacionJugador = 1f;

    [Header("Jugador")]
    public Transform jugador;
    public Rigidbody rbJugador;
    public PlayerMovement movimientoJugador;
    public ConstantForce fuerzaJugador;
    public LookAtMouseTest lookAtMouse;

    [Header("Iman")]
    public Transform iman;
    public Transform puntoInstalacion;

    private Coroutine movimientoActual;
    private bool ocupado = false;
    private bool jugadorEnganchado = false;

    private bool gravedadOriginal;
    private bool kinematicOriginal;
    private bool movimientoOriginal;
    private bool fuerzaOriginal;
    private bool lookAtMouseOriginal;

    private Transform padreOriginalIman;
    private Vector3 posicionLocalOriginalIman;
    private Quaternion rotacionLocalOriginalIman;

    private void Update()
    {
        if (!jugadorEnganchado || PauseMenu.GameIsPaused)
            return;

        if (movimientoJugador != null &&
            movimientoJugador.jumpInput != null &&
            movimientoJugador.jumpInput.action.WasPressedThisFrame())
        {
            SoltarJugador();
        }
    }

    public bool EstaOcupado()
    {
        return ocupado;
    }

    public void IniciarAscenso()
    {
        if (ocupado)
            return;

        if (puntoInicio == null || puntoFinal == null ||
            jugador == null || rbJugador == null ||
            movimientoJugador == null || iman == null ||
            puntoInstalacion == null)
        {
            Debug.LogWarning("ASCENSOR: Faltan referencias");
            return;
        }

        ocupado = true;
        jugadorEnganchado = true;

        gravedadOriginal = rbJugador.useGravity;
        kinematicOriginal = rbJugador.isKinematic;
        movimientoOriginal = movimientoJugador.enabled;

        if (fuerzaJugador != null)
        {
            fuerzaOriginal = fuerzaJugador.enabled;
            fuerzaJugador.enabled = false;
        }

        // Desactivar temporalmente el apuntado con el mouse
        if (lookAtMouse != null)
        {
            lookAtMouseOriginal = lookAtMouse.enabled;
            lookAtMouse.enabled = false;
        }

        movimientoJugador.usingJetpack = false;
        movimientoJugador.enabled = false;

        rbJugador.linearVelocity = Vector3.zero;
        rbJugador.angularVelocity = Vector3.zero;
        rbJugador.useGravity = false;
        rbJugador.isKinematic = true;

        padreOriginalIman = iman.parent;
        posicionLocalOriginalIman = iman.localPosition;
        rotacionLocalOriginalIman = iman.localRotation;

        iman.SetParent(null, true);
        iman.position = puntoInicio.position;

        rbJugador.position = iman.position +
            Vector3.right * separacionJugador;

        movimientoActual = StartCoroutine(Subir());

        Debug.Log("ASCENSOR: Iniciando ascenso");
    }

    private IEnumerator Subir()
    {
        Vector3 desplazamientoJugador =
            Vector3.right * separacionJugador;

        while (jugadorEnganchado &&
               Vector3.Distance(iman.position, puntoFinal.position) > 0.01f)
        {
            yield return new WaitForFixedUpdate();

            if (!jugadorEnganchado)
                yield break;

            if (PauseMenu.GameIsPaused)
                continue;

            Vector3 nuevaPosicionIman = Vector3.MoveTowards(
                iman.position,
                puntoFinal.position,
                velocidadAscenso * Time.fixedDeltaTime
            );

            iman.position = nuevaPosicionIman;

            Vector3 nuevaPosicionJugador =
                nuevaPosicionIman + desplazamientoJugador;

            rbJugador.position = nuevaPosicionJugador;
        }

        if (jugadorEnganchado)
        {
            iman.position = puntoFinal.position;

            rbJugador.position =
                puntoFinal.position + desplazamientoJugador;

            Debug.Log("ASCENSOR: Llegamos al punto final");
        }

        movimientoActual = null;
    }

    private void SoltarJugador()
    {
        if (!jugadorEnganchado)
            return;

        jugadorEnganchado = false;

        if (movimientoActual != null)
        {
            StopCoroutine(movimientoActual);
            movimientoActual = null;
        }

        rbJugador.isKinematic = kinematicOriginal;
        rbJugador.useGravity = gravedadOriginal;

        if (fuerzaJugador != null)
        {
            fuerzaJugador.enabled = fuerzaOriginal;
        }

        movimientoJugador.enabled = movimientoOriginal;

        // Restaurar el apuntado con el mouse
        if (lookAtMouse != null)
        {
            lookAtMouse.enabled = lookAtMouseOriginal;
        }

        rbJugador.linearVelocity = new Vector3(
            0f,
            movimientoJugador.jumpForce,
            0f
        );

        Debug.Log("ASCENSOR: Jugador liberado");

        movimientoActual = StartCoroutine(BajarIman());
    }

    private IEnumerator BajarIman()
    {
        while (Vector3.Distance(
            iman.position, puntoInicio.position) > 0.01f)
        {
            yield return new WaitForFixedUpdate();

            if (PauseMenu.GameIsPaused)
                continue;

            iman.position = Vector3.MoveTowards(
                iman.position,
                puntoInicio.position,
                velocidadDescenso * Time.fixedDeltaTime
            );
        }

        iman.SetParent(padreOriginalIman, false);
        iman.localPosition = posicionLocalOriginalIman;
        iman.localRotation = rotacionLocalOriginalIman;

        ocupado = false;
        movimientoActual = null;

        Debug.Log("ASCENSOR: Iman listo para reutilizar");
    }

    private void OnDrawGizmos()
    {
        if (puntoInicio == null || puntoFinal == null)
            return;

        Gizmos.color = Color.cyan;

        Gizmos.DrawLine(
            puntoInicio.position,
            puntoFinal.position
        );

        Gizmos.DrawSphere(puntoInicio.position, 0.25f);
        Gizmos.DrawSphere(puntoFinal.position, 0.25f);
    }
}
