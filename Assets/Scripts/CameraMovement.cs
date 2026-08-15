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

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(25, -yRotation, 0), rotationSpeed * Time.deltaTime);



        }
        else
        {

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(25, yRotation, 0), rotationSpeed * Time.deltaTime);


        }
        if (transform.position.x < Player.position.x -xPosition || transform.position.x > Player.position.x + xPosition)
        {
            var newpos = new Vector3(0, 0, 0);
            if (Player.position.x < transform.position.x + xPosition)
            {
            newpos = Player.position + new Vector3(+xPosition, ViewHeight,0);
            newpos.z = zoff;
            newpos = Vector3.Lerp(transform.position, newpos, speed * Time.deltaTime);           
            }
            if (Player.position.x > transform.position.x - xPosition)
            {
                newpos = Player.position + new Vector3(-xPosition, ViewHeight, 0);
                newpos.z = zoff;
                newpos = Vector3.Lerp(transform.position, newpos, speed * Time.deltaTime);
            }
            transform.position = newpos;
        }
      
    }
}
