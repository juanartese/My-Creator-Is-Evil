using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraMovement : MonoBehaviour
{
    public Transform Player;
    public float speed;
    float zoff;
    public float ViewHeight;
    public float yRotation;
    public float xPosition;
    public float rotationSpeed;
    private Vector3 newPosition;


    // Start is called before the first frame update
    void Awake()
    {
        zoff = transform.position.z;

    }

    // Update is called once per frame
    void LateUpdate()
    {
       
        if (Player.position.x < transform.position.x)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(30, -yRotation, 0), rotationSpeed * Time.deltaTime);
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(30, yRotation, 0), rotationSpeed * Time.deltaTime);
        }
        if (transform.position.x > Player.position.x -xPosition && transform.position.x < Player.position.x + xPosition)
        {            
            newPosition = new Vector3(transform.position.x, Player.position.y + ViewHeight, zoff);
        }
        else
        {
            if (Player.position.x < transform.position.x + xPosition)
            {
              newPosition = Player.position + new Vector3(+xPosition, ViewHeight, zoff);                      
            }
            if (Player.position.x > transform.position.x - xPosition)
            {
              newPosition = Player.position + new Vector3(-xPosition, ViewHeight, zoff);
            }
        }
        newPosition = Vector3.Lerp(transform.position, newPosition, speed * Time.deltaTime);
        transform.position = newPosition;
    }
}
