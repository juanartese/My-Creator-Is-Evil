using UnityEngine;

public class PlayerCheckpointSpawn : MonoBehaviour
{
    void Start()
    {
        if (Checkpoint.hasCheckpoint)
        {
            // 1. Warp Player back to the Site of Grace
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.position = Checkpoint.savedPosition;
            }

            transform.position = Checkpoint.savedPosition;
            transform.rotation = Checkpoint.savedRotation;

            // 2. Restore all unlocked abilities
            PlayerMovement pm = GetComponent<PlayerMovement>();
            if (pm != null)
            {
                pm.unlockDoubleJump = Checkpoint.savedDoubleJump;
                pm.unlockJetpack = Checkpoint.savedJetpack;
                pm.unlockHook = Checkpoint.savedHook;
            }
        }
    }
}