
using UnityEngine;

public class FlechaFlotante : MonoBehaviour
{
    [SerializeField] private float velocidad = 2f;
    [SerializeField] private float distancia = 5f;

    private RectTransform rectTransform;
    private Vector2 posicionOriginal;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        posicionOriginal = rectTransform.anchoredPosition;
    }

    void Update()
    {
        float movimiento = Mathf.Sin(Time.time * velocidad) * distancia;

        rectTransform.anchoredPosition =
            posicionOriginal + new Vector2(0f, movimiento);
    }
}
