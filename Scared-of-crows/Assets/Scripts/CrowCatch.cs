using UnityEngine;

public class CrowCatch : MonoBehaviour
{
    private CrowPatrol crowPatrol;

    void Start()
    {
        crowPatrol = GetComponent<CrowPatrol>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CheckpointManager.instance.RespawnPlayer(other.gameObject);
            if (crowPatrol != null)
                crowPatrol.ResetToStart();
        }
    }
}