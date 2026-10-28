using UnityEngine;

public class GrupoTNTCombate : MonoBehaviour
{
    public EnemigoGancho enemigo;
    public Transform centroExplosion;

    public float radioExplosion = 5f;
    
    //Changed this from float to int
    public int danoExplosion = 250; 

    private bool danoAplicado = false;

    public void AplicarDano()
    {
        Debug.Log("TNT DEBUG - AplicarDano fue llamado");

        // Evitar aplicar dano mas de una vez
        if (danoAplicado)
        {
            Debug.Log("TNT DEBUG - El dano ya fue aplicado");
            return;
        }

        // Comprobar referencias
        if (enemigo == null || centroExplosion == null)
        {
            Debug.LogWarning("TNT DEBUG - Faltan referencias");
            return;
        }

        // Calcular distancia al enemigo
        float distancia = Vector3.Distance(
            centroExplosion.position,
            enemigo.transform.position
        );

        Debug.Log("TNT DEBUG - Distancia al enemigo: " + distancia);
        Debug.Log("TNT DEBUG - Radio configurado: " + radioExplosion);

        // Aplicar dano si esta dentro del radio
        if (distancia <= radioExplosion)
        {
            // Use the TakeDamage method instead of directly subtracting Life
            enemigo.TakeDamage(danoExplosion);

            Debug.Log("TNT: Enemigo recibio 250 de dano");
            Debug.Log("TNT DEBUG - Vida restante: " + enemigo.Life);
        }
        else
        {
            Debug.Log("TNT DEBUG - Enemigo fuera del radio");
        }

        // Evitar dano duplicado
        danoAplicado = true;
    }
}