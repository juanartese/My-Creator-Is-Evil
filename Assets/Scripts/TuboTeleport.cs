using UnityEngine;
using System.Collections;

public class TuboTeleport : MonoBehaviour
{
    public Transform puntoSalida;
    public GameObject modeloVisual;

    private bool teletransportando = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !teletransportando)
        {
            StartCoroutine(Teletransportar(other.transform));
        }
    }

    private IEnumerator Teletransportar(Transform player)
    {
        teletransportando = true;

        modeloVisual.SetActive(false);

        yield return new WaitForSeconds(0.25f);

        player.position = puntoSalida.position;

        modeloVisual.SetActive(true);

        print("PLAYER TELETRANSPORTADO");

        teletransportando = false;
    }
}