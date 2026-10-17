
using System.Collections;
using UnityEngine;

public class TNTExplosiva : MonoBehaviour
{
    public GameObject prefabExplosion;
    public AudioClip sonidoExplosion;
    public GrupoTNTCombate grupoTNT;

    public float radioCadena = 5f;
    public float retrasoCadena = 0.2f;

    private bool exploto = false;

    public void Explotar()
    {
        if (exploto)
            return;

        exploto = true;

        // Aplicar dano al enemigo una sola vez por grupo
        if (grupoTNT != null)
        {
            grupoTNT.AplicarDano();
        }

        // Buscar otras TNT cercanas
        PropagarCadena();

        // Ejecutar explosion
        CrearExplosion();

        Destroy(gameObject);
    }

    public void ActivarCadena(float retraso)
    {
        if (exploto)
            return;

        exploto = true;

        StartCoroutine(ExplosionRetardada(retraso));
    }

    private IEnumerator ExplosionRetardada(float retraso)
    {
        yield return new WaitForSeconds(retraso);

        // Propagar la explosion a otras TNT
        PropagarCadena();

        CrearExplosion();

        Destroy(gameObject);
    }

    private void PropagarCadena()
    {
        Collider[] objetos = Physics.OverlapSphere(
            transform.position,
            radioCadena
        );

        foreach (Collider objeto in objetos)
        {
            TNTExplosiva otraTNT =
                objeto.GetComponent<TNTExplosiva>();

            if (otraTNT != null && otraTNT != this)
            {
                otraTNT.ActivarCadena(retrasoCadena);
            }
        }
    }

    private void CrearExplosion()
    {
        print("TNT: BOOM!");

        // Reproducir sonido desde la camara
        if (sonidoExplosion != null)
        {
            Vector3 posicionSonido = transform.position;

            if (Camera.main != null)
            {
                posicionSonido = Camera.main.transform.position;
            }

            AudioSource.PlayClipAtPoint(
                sonidoExplosion,
                posicionSonido,
                1f
            );
        }

        // Crear efecto visual de explosion
        if (prefabExplosion != null)
        {
            GameObject explosion = Instantiate(
                prefabExplosion,
                transform.position,
                Quaternion.identity
            );

            ParticleSystem particulas =
                explosion.GetComponent<ParticleSystem>();

            if (particulas != null)
            {
                particulas.Play();
            }

            Destroy(explosion, 2f);
        }
    }
}
