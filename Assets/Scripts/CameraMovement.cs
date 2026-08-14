using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform Player;
    public float speed;
    float zoff;
    public float ViewHeight;

    // Start is called before the first frame update
    void Awake()
    {
        zoff = transform.position.z;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        var newpos = Player.position + new Vector3(0, ViewHeight,0);
        newpos.z = zoff;
        newpos = Vector3.Lerp(transform.position, newpos, speed * Time.deltaTime);
        transform.position = newpos;
    }
}
