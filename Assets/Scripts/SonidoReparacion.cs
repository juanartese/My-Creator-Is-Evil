using UnityEngine;
using UnityEngine.UI;

public class SonidoReparacion : MonoBehaviour
{
    public Image llaveCarga;
    public AudioSource sonidoTaladro;

    private bool iniciado = false;

    private void Start()
    {
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
        // Si desaparece la llave, se corta el sonido
        else
        {
            if (sonidoTaladro.isPlaying)
            {
                sonidoTaladro.Stop();
            }
        }
    }
}