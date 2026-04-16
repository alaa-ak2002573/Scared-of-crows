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
        player.transform.position = lastCheckpoint;
    }
}