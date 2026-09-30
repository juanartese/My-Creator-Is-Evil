using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int vidaMaxima = 100;
    public int vidaActual;

    public Image vida1;
    public Image vida2;
    public Image vida3;
    public Image vida4;

    public Image llaveCarga;

    public GameObject modeloVisual;

    private Coroutine pulsoCritico;
    private Coroutine titileoActual;
    private Coroutine animacionLlave;

    private Color verde = Color.green;
    private Color amarillo = Color.yellow;
    private Color rojo = Color.red;
    private Color apagado = new Color(0.12f, 0.14f, 0.16f, 1f);

    private void Start()
    {
        vidaActual = vidaMaxima;
        ActualizarBarra();

        if (llaveCarga != null)
        {
            llaveCarga.enabled = false;
        }
    }

    public void RecibirDanio(int cantidad)
    {
        if (vidaActual <= 0) return; 

        vidaActual -= cantidad;

        if (vidaActual <= 0)
        {
            vidaActual = 0; // Keep health at 0
            
            // TRIGGER THE GAME OVER SCREEN HERE
            if (LevelManager.instance != null)
            {
                LevelManager.instance.GameOver();
            }
        }

        ActualizarBarra();

        if (titileoActual != null)
        {
            StopCoroutine(titileoActual);
        }

        titileoActual = StartCoroutine(TitilarPlayer());

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

        if (llaveCarga != null)
        {
            if (animacionLlave != null)
            {
                StopCoroutine(animacionLlave);
            }

            animacionLlave = StartCoroutine(MostrarLlave());
        }

        print("Vida: " + vidaActual);
    }

    private IEnumerator MostrarLlave()
    {
        llaveCarga.enabled = true;

        yield return new WaitForSeconds(0.8f);

        llaveCarga.enabled = false;

        animacionLlave = null;
    }

    private void ActualizarBarra()
    {
        vida1.color = apagado;
        vida2.color = apagado;
        vida3.color = apagado;
        vida4.color = apagado;

        if (vidaActual >= 75)
        {
            if (vidaActual >= 25) vida1.color = verde;
            if (vidaActual >= 50) vida2.color = verde;
            if (vidaActual >= 75) vida3.color = verde;
            if (vidaActual >= 100) vida4.color = verde;

            DetenerPulsoCritico();
        }
        else if (vidaActual >= 50)
        {
            vida1.color = amarillo;
            vida2.color = amarillo;

            DetenerPulsoCritico();
        }
        else if (vidaActual > 0)
        {
            vida1.color = rojo;

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

    private IEnumerator TitilarPlayer()
    {
        for (int i = 0; i < 3; i++)
        {
            modeloVisual.SetActive(false);
            yield return new WaitForSeconds(0.1f);

            modeloVisual.SetActive(true);
            yield return new WaitForSeconds(0.1f);
        }

        titileoActual = null;
    }

    private IEnumerator PulsoCritico()
    {
        while (vidaActual > 0 && vidaActual < 50)
        {
            Color color = vida1.color;

            color.a = 0.35f;
            vida1.color = color;

            yield return new WaitForSeconds(0.35f);

            color.a = 1f;
            vida1.color = color;

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

        if (vida1 != null)
        {
            Color color = vida1.color;
            color.a = 1f;
            vida1.color = color;
        }
    }
}