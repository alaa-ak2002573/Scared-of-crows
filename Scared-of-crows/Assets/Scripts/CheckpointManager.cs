using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager instance;
    private Vector3 lastCheckpoint;

    void Awake()
    {
        instance = this;
        lastCheckpoint = GameObject.FindGameObjectWithTag("Player").transform.position;
    }

    public void SetCheckpoint(Vector3 position)
    {
        lastCheckpoint = position;
    }

    public void RespawnPlayer(GameObject player)
    {
        // Stop all movement first
        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null)
        {
            cc.enabled = false;          // disable before teleport
            player.transform.position = lastCheckpoint;
            cc.enabled = true;           // re-enable after
        }
        else
        {
            player.transform.position = lastCheckpoint;
        }

        // Also drop any carried vegetable on respawn
        vegetablePickup carried = FindFirstObjectByType<vegetablePickup>();
        if (carried != null && carried.IsCarried())
            carried.Drop();
    }
}