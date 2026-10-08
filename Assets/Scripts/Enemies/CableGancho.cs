using UnityEngine;

public class CableGancho : MonoBehaviour
{
    [SerializeField] Transform puntoGancho;
    [SerializeField] Gancho gancho;
    [SerializeField] LineRenderer cable;

    void Start()
    {
        cable.positionCount = 2;
        cable.enabled = false;
    }

    void LateUpdate()
    {
        if (gancho.EstaLanzado)
        {
            cable.enabled = true;

            cable.SetPosition(0, puntoGancho.position);
            cable.SetPosition(1, gancho.transform.position);
        }
        else
        {
            cable.enabled = false;
        }
    }
}