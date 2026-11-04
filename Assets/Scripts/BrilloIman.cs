
using UnityEngine;

public class BrilloIman : MonoBehaviour
{
    [Header("Configuracion del brillo")]
    [SerializeField] private Renderer[] piezasIman;
    [SerializeField] private Color colorBrillo = Color.red;
    [SerializeField] private float intensidadMinima = 1f;
    [SerializeField] private float intensidadMaxima = 4f;
    [SerializeField] private float velocidad = 2f;

    private Material materialBrillante;
    private ImanInteractivo imanInteractivo;
    private Color colorOriginal;

    private void Start()
    {
        imanInteractivo = GetComponent<ImanInteractivo>();

        if (piezasIman.Length == 0 || piezasIman[0] == null)
            return;

        materialBrillante = new Material(piezasIman[0].sharedMaterial);
        materialBrillante.EnableKeyword("_EMISSION");

        colorOriginal = materialBrillante.GetColor("_EmissionColor");

        foreach (Renderer pieza in piezasIman)
        {
            if (pieza != null)
                pieza.sharedMaterial = materialBrillante;
        }
    }

    private void Update()
    {
        if (materialBrillante == null)
            return;

        // Apagar el brillo al recoger el iman
        if (imanInteractivo != null && imanInteractivo.EstaRecogido())
        {
            materialBrillante.SetColor("_EmissionColor", Color.black);
            return;
        }

        // Brillo pulsante mientras el iman esta disponible
        float pulso = (Mathf.Sin(Time.time * velocidad) + 1f) / 2f;

        float intensidad = Mathf.Lerp(
            intensidadMinima,
            intensidadMaxima,
            pulso
        );

        materialBrillante.SetColor(
            "_EmissionColor",
            colorBrillo * intensidad
        );
    }

    private void OnDestroy()
    {
        if (materialBrillante != null)
            Destroy(materialBrillante);
    }
}
