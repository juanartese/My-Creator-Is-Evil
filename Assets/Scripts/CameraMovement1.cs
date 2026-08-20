using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraMovement1 : MonoBehaviour
{
    public Transform Player;
    public float speed;
    float zoff;
    public float ViewHeight;
    public float yRotation;
    public float xPosition;
    public float rotationSpeed;
    public float zOff;
    private Vector3 newPosition;


    // Start is called before the first frame update
    void Awake()
    {
        

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (Player.rotation.y < 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(30, -yRotation, 0), rotationSpeed * Time.deltaTime);
            newPosition = Player.position + new Vector3(+xPosition, ViewHeight, zOff);
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(30, yRotation, 0), rotationSpeed * Time.deltaTime);
            newPosition = Player.position + new Vector3(-xPosition, ViewHeight, zOff);
        }
        newPosition = Vector3.Lerp(transform.position, newPosition, speed * Time.deltaTime);
        transform.position = newPosition;
    }
}
