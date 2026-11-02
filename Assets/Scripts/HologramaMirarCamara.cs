
using UnityEngine;

public class HologramaMirarCamara : MonoBehaviour
{
    [SerializeField] private Camera camaraJugador;

    void LateUpdate()
    {
        if (camaraJugador == null)
            return;

        transform.rotation = camaraJugador.transform.rotation;
    }
}
