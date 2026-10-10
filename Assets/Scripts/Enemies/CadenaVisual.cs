
using System.Collections.Generic;
using UnityEngine;

public class CadenaVisual : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform puntoGancho;
    [SerializeField] private Gancho gancho;
    [SerializeField] private GameObject prefabEslabon;

    [Header("Configuracion")]
    [SerializeField] private float separacion = 0.30f;
    [SerializeField] private float escala = 0.25f;

    [Header("Orientacion")]
    [SerializeField] private float giroAlternado = 45f;

    private List<Transform> eslabones = new List<Transform>();

    void LateUpdate()
    {
        if (puntoGancho == null || gancho == null || prefabEslabon == null)
            return;

        if (!gancho.EstaLanzado)
        {
            MostrarEslabones(0);
            return;
        }

        Vector3 inicio = puntoGancho.position;
        Vector3 fin = gancho.transform.position;
        Vector3 direccion = fin - inicio;

        float distancia = direccion.magnitude;

        if (distancia < 0.01f)
        {
            MostrarEslabones(0);
            return;
        }

        Vector3 ejeCadena = direccion.normalized;

        int cantidad = Mathf.Max(
            1,
            Mathf.CeilToInt(distancia / Mathf.Max(0.01f, separacion))
        );

        while (eslabones.Count < cantidad)
        {
            GameObject nuevo = Instantiate(prefabEslabon, transform);
            nuevo.transform.localScale = Vector3.one * escala;
            eslabones.Add(nuevo.transform);
        }

        MostrarEslabones(cantidad);

        Camera camara = Camera.main;

        Vector3 haciaCamara = camara != null
            ? camara.transform.position - inicio
            : Vector3.forward;

        Vector3 normalPlano = Vector3.ProjectOnPlane(
            haciaCamara,
            ejeCadena
        );

        if (normalPlano.sqrMagnitude < 0.0001f)
            normalPlano = Vector3.ProjectOnPlane(Vector3.up, ejeCadena);

        if (normalPlano.sqrMagnitude < 0.0001f)
            normalPlano = Vector3.ProjectOnPlane(Vector3.forward, ejeCadena);

        normalPlano.Normalize();

        Vector3 arriba = Vector3.Cross(normalPlano, ejeCadena);

        Quaternion orientacion = Quaternion.LookRotation(
            normalPlano,
            arriba
        ) * Quaternion.Euler(0f, 0f, -90f);

        for (int i = 0; i < cantidad; i++)
        {
            Transform eslabon = eslabones[i];

            float t = (i + 0.5f) / cantidad;

            eslabon.position = Vector3.Lerp(inicio, fin, t);

            float giro = i % 2 == 0 ? 0f : giroAlternado;

            eslabon.rotation = orientacion *
                Quaternion.Euler(giro, 0f, 0f);

            eslabon.localScale = Vector3.one * escala;
        }
    }

    void MostrarEslabones(int cantidad)
    {
        for (int i = 0; i < eslabones.Count; i++)
        {
            bool mostrar = i < cantidad;

            if (eslabones[i].gameObject.activeSelf != mostrar)
                eslabones[i].gameObject.SetActive(mostrar);
        }
    }
}
