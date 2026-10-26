
using UnityEngine;
using UnityEngine.UI;

public class SonidoReparacion : MonoBehaviour
{
    public Image llaveCarga;
    public AudioSource sonidoTaladro;

    private bool iniciado = false;

    private void Start()
    {
        // Buscar la llave si no esta asignada
        if (llaveCarga == null)
        {
            GameObject objetoLlave = GameObject.Find("LlaveCarga");

            if (objetoLlave != null)
            {
                llaveCarga = objetoLlave.GetComponent<Image>();
            }
        }

        // La llave empieza apagada
        if (llaveCarga != null)
        {
            llaveCarga.enabled = false;
        }

        // El sonido empieza apagado
        if (sonidoTaladro != null)
        {
            sonidoTaladro.Stop();
        }

        iniciado = true;
    }

    private void Update()
    {
        if (!iniciado || llaveCarga == null || sonidoTaladro == null)
            return;

        // Si aparece la llave, suena el taladro
        if (llaveCarga.enabled)
        {
            if (!sonidoTaladro.isPlaying)
            {
                sonidoTaladro.Play();
            }
        }
        else
        {
            // Si desaparece la llave, se corta el sonido
            if (sonidoTaladro.isPlaying)
            {
                sonidoTaladro.Stop();
            }
        }
    }
}
