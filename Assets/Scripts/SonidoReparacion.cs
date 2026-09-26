using UnityEngine;
using UnityEngine.UI;

public class SonidoReparacion : MonoBehaviour
{
    public Image llaveCarga;
    public AudioSource sonidoTaladro;

    private void Update()
    {
        if (llaveCarga == null || sonidoTaladro == null)
            return;

        if (llaveCarga.enabled)
        {
            if (!sonidoTaladro.isPlaying)
            {
                sonidoTaladro.Play();
            }
        }
        else
        {
            if (sonidoTaladro.isPlaying)
            {
                sonidoTaladro.Stop();
            }
        }
    }
}