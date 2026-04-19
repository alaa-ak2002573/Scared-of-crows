using UnityEngine;
using System.Collections;

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
            if (crowPatrol != null)
                crowPatrol.ResetToStart();
            StartCoroutine(CatchSequence(other.gameObject));
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (crowPatrol != null)
                crowPatrol.ResetToStart();
            StartCoroutine(CatchSequence(other.gameObject));
        }
    }

    IEnumerator CatchSequence(GameObject player)
    {
        GameManager.instance.ChangeState(GameManager.GameState.GameOver);
        yield return new WaitForSeconds(1f);
        CheckpointManager.instance.RespawnPlayer(player);
    }
}