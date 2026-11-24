using System.Collections;
using UnityEngine;

public class RespawnTNT : MonoBehaviour
{
    public GameObject prefabTNT;
    public TNTTemporizador tntActual;
    public float tiempoRespawn = 3f;

    private Vector3 posicionInicial;
    private Quaternion rotacionInicial;
    private Vector3 escalaInicial;
    private BotonPuerta botonPuerta;
    private bool generando;

    private void Start()
    {
        if (tntActual == null)
        {
            Debug.LogWarning("TNT: Falta asignar la caja inicial");
            enabled = false;
            return;
        }

        posicionInicial = tntActual.transform.position;
        rotacionInicial = tntActual.transform.rotation;
        escalaInicial = tntActual.transform.localScale;

        // Conservar la referencia al boton de esta escena.
        botonPuerta = tntActual.botonPuerta;
    }

    private void Update()
    {
        if (tntActual == null && !generando && prefabTNT != null)
        {
            generando = true;
            StartCoroutine(CrearOtraTNT());
        }
    }

    private IEnumerator CrearOtraTNT()
    {
        yield return new WaitForSeconds(tiempoRespawn);

        GameObject nuevaCaja = Instantiate(
            prefabTNT,
            posicionInicial,
            rotacionInicial
        );

        nuevaCaja.transform.localScale = escalaInicial;

        tntActual = nuevaCaja.GetComponent<TNTTemporizador>();

        if (tntActual == null)
        {
            Debug.LogError("TNT: El prefab no tiene TNTTemporizador");
            Destroy(nuevaCaja);
            enabled = false;
            yield break;
        }

        tntActual.botonPuerta = botonPuerta;
        generando = false;

        Debug.Log("TNT: Nueva caja creada");
    }
}