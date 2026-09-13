using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int vidaMaxima = 100;
    public int vidaActual;

    public Image barraVida;

    private Coroutine flashActual;
    private Coroutine pulsoCritico;

    private void Start()
    {
        vidaActual = vidaMaxima;
        ActualizarBarra();
    }

    public void RecibirDanio(int cantidad)
    {
        vidaActual -= cantidad;

        if (vidaActual < 0)
        {
            vidaActual = 0;
        }

        ActualizarBarra();

        if (flashActual != null)
        {
            StopCoroutine(flashActual);
        }

        flashActual = StartCoroutine(FlashDanio());

        print("Vida: " + vidaActual);
    }

    public void RecuperarVida(int cantidad)
    {
        vidaActual += cantidad;

        if (vidaActual > vidaMaxima)
        {
            vidaActual = vidaMaxima;
        }

        ActualizarBarra();

        print("Vida: " + vidaActual);
    }

    private void ActualizarBarra()
    {
        float porcentajeActual = (float)vidaActual / vidaMaxima;

        float paso = 0.25f;
        int nivel = Mathf.CeilToInt(porcentajeActual / paso);
        float valorAjustado = nivel * paso;

        barraVida.fillAmount = Mathf.Clamp01(valorAjustado);

        if (vidaActual >= 75)
        {
            barraVida.color = Color.green;
            DetenerPulsoCritico();
        }
        else if (vidaActual >= 50)
        {
            barraVida.color = Color.yellow;
            DetenerPulsoCritico();
        }
        else if (vidaActual > 0)
        {
            barraVida.color = Color.red;

            if (pulsoCritico == null)
            {
                pulsoCritico = StartCoroutine(PulsoCritico());
            }
        }
        else
        {
            DetenerPulsoCritico();
        }
    }

    private IEnumerator FlashDanio()
    {
        barraVida.color = Color.red;

        yield return new WaitForSeconds(0.2f);

        ActualizarBarra();

        flashActual = null;
    }

    private IEnumerator PulsoCritico()
    {
        while (vidaActual > 0 && vidaActual < 50)
        {
            Color color = barraVida.color;

            color.a = 0.35f;
            barraVida.color = color;

            yield return new WaitForSeconds(0.35f);

            color.a = 1f;
            barraVida.color = color;

            yield return new WaitForSeconds(0.35f);
        }

        pulsoCritico = null;
    }

    private void DetenerPulsoCritico()
    {
        if (pulsoCritico != null)
        {
            StopCoroutine(pulsoCritico);
            pulsoCritico = null;
        }

        Color color = barraVida.color;
        color.a = 1f;
        barraVida.color = color;
    }
}