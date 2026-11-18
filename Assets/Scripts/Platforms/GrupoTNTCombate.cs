using UnityEngine;

public class GrupoTNTCombate : MonoBehaviour
{
    public EnemigoGancho enemigo;
    public Transform centroExplosion;

    public float radioExplosion = 5f;
    public int danoExplosion = 250;

    [Header("Humo compartido del grupo")]
    public GameObject prefabHumoGrupo;
    public float duracionHumo = 6f;

    private bool danoAplicado = false;
    private bool humoCreado = false;

    public void AplicarDano()
    {
        Debug.Log("TNT DEBUG - AplicarDano fue llamado");

        // Evitar aplicar dano mas de una vez.
        if (danoAplicado)
        {
            Debug.Log("TNT DEBUG - El dano ya fue aplicado");
            return;
        }

        // Comprobar referencias.
        if (enemigo == null || centroExplosion == null)
        {
            Debug.LogWarning("TNT DEBUG - Faltan referencias");
            return;
        }

        float distancia = Vector3.Distance(
            centroExplosion.position,
            enemigo.transform.position
        );

        Debug.Log("TNT DEBUG - Distancia al enemigo: " + distancia);
        Debug.Log("TNT DEBUG - Radio configurado: " + radioExplosion);

        if (distancia <= radioExplosion)
        {
            enemigo.TakeDamage(danoExplosion);

            Debug.Log(
                "TNT: Enemigo recibio " + danoExplosion + " de dano"
            );

            Debug.Log(
                "TNT DEBUG - Vida restante: " + enemigo.Life
            );
        }
        else
        {
            Debug.Log("TNT DEBUG - Enemigo fuera del radio");
        }

        danoAplicado = true;
    }

    public void CrearHumoGrupo()
    {
        // Crear una sola nube para todo este grupo.
        if (humoCreado)
            return;

        if (prefabHumoGrupo == null || centroExplosion == null)
            return;

        humoCreado = true;

        GameObject humo = Instantiate(
            prefabHumoGrupo,
            centroExplosion.position,
            Quaternion.identity
        );

        // Reproducir las particulas del objeto y sus hijos.
        ParticleSystem[] sistemas =
            humo.GetComponentsInChildren<ParticleSystem>();

        foreach (ParticleSystem sistema in sistemas)
        {
            sistema.Play(false);
        }

        Destroy(humo, duracionHumo);
    }
}