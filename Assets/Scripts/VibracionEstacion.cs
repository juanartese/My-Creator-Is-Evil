
using UnityEngine;
using UnityEngine.UI;

public class VibracionEstacion : MonoBehaviour
{
    public Image llaveCarga;
    public Transform maquina;
    public ParticleSystem chispasReparacion;
    public ParticleSystem humoReparacion;

    public float intensidadVibracion = 0.05f;
    public float velocidadVibracion = 35f;

    private Vector3 posicionOriginal;
    private bool posicionGuardada = false;

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

        // Guardar la posicion original de la maquina
        if (maquina != null)
        {
            posicionOriginal = maquina.localPosition;
            posicionGuardada = true;
        }

        DetenerEfectos();
    }

    private void Update()
    {
        if (llaveCarga == null || maquina == null || !posicionGuardada)
            return;

        bool reparando = llaveCarga.enabled && llaveCarga.gameObject.activeInHierarchy;

        if (reparando)
        {
            float vibracionX = Mathf.Sin(Time.time * velocidadVibracion) * intensidadVibracion;
            float vibracionY = Mathf.Cos(Time.time * velocidadVibracion * 1.3f) * intensidadVibracion;

            maquina.localPosition = posicionOriginal +
                new Vector3(vibracionX, vibracionY, 0f);

            if (chispasReparacion != null && !chispasReparacion.isPlaying)
                chispasReparacion.Play();

            if (humoReparacion != null && !humoReparacion.isPlaying)
                humoReparacion.Play();
        }
        else
        {
            maquina.localPosition = posicionOriginal;
            DetenerEfectos();
        }
    }

    private void DetenerEfectos()
    {
        if (chispasReparacion != null)
            chispasReparacion.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        if (humoReparacion != null)
            humoReparacion.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    private void OnDisable()
    {
        if (maquina != null && posicionGuardada)
            maquina.localPosition = posicionOriginal;

        DetenerEfectos();
    }
}
