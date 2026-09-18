using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class LookAtMouseTest : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer; // Assign your ground/floor layer here
    [SerializeField] private float rotationSpeed = 10f; // Speed of the rotation smoothing
    private Vector3 playerPos;
    public float playerPosY;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        //Stop reading mouse position and rotating if the game is paused
        if (PauseMenu.GameIsPaused) return;

        RotateTowardsMouse();
    }

    private void RotateTowardsMouse()
    {
        playerPos = transform.position + Vector3.up * playerPosY;
        Plane horizontalPlane = new Plane(Vector3.up, playerPos);
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (horizontalPlane.Raycast(ray, out float entry))
        {
            Vector3 intersectionPoint = ray.GetPoint(entry);
            Vector3 direction = intersectionPoint - playerPos;

            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }
    }
}