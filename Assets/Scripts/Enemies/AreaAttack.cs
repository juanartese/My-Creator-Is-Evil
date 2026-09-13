using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;

public class AreaAttack : MonoBehaviour
{
    public float startingSize;
    public float duration;
    public float expansionSpeed;
    public GameObject player;
    float xDistance;
    float yDistance;
    private Renderer renderAttack;
    public Material luzIndicadoraRoja;
    private PlayerHealth playerHealth;
    private bool hitOnce = true;
    void Start()
    {
        transform.localScale = new Vector3(1, 1, 1) * startingSize;
        Destroy(gameObject, duration);
        renderAttack = GetComponent<Renderer>();
        player = SceneAsset.FindAnyObjectByType<PlayerMovement>().gameObject;
    }

    void Update()
    {
        xDistance = Vector3.Distance(this.transform.position, player.transform.position);
        yDistance = (this.transform.position.y - player.transform.position.y);
        transform.localScale += new Vector3(1, 1, 1) * expansionSpeed * Time.deltaTime;
        if (xDistance > transform.localScale.x / 2 - 0.5f && xDistance < transform.localScale.x / 2 + 0.5f && hitOnce)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
            if (yDistance < 2.5 && yDistance > -.5)
            {
                renderAttack.material = luzIndicadoraRoja;
                if (playerHealth != null)
                {
                    playerHealth.RecibirDanio(25);
                    hitOnce = false;
                }

            }
          
        }
    }
}
