using UnityEngine;

public class DMGOnCollision : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float delay;
    public float timer = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && timer <= 0)
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            playerHealth.RecibirDanio(25);
            timer = delay;
        }
    }
}
