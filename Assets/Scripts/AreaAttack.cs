using UnityEngine;

public class AreaAttack : MonoBehaviour
{
    public float startingSize;
    public float duration;
    public float expansionSpeed;
    public Transform playerTransform;
    float distance;
    private Renderer renderAttack;
    public Material luzIndicadoraRoja;
    void Start()
    {
        transform.localScale = new Vector3(1, 1, 1) * startingSize;
        Destroy(gameObject, duration);
        renderAttack = GetComponent<Renderer>();
    }

    void Update()
    {
        distance = Vector3.Distance(this.transform.position, playerTransform.position);
        transform.localScale += new Vector3(1, 1, 1) * expansionSpeed * Time.deltaTime;
        if (distance > transform.localScale.x/2 -1 && distance < transform.localScale.x/2 + 1)
        {
            
                renderAttack.material = luzIndicadoraRoja;
          
        }
    }
}
