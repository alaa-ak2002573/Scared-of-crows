using UnityEngine;
using UnityEngine.AI;

public class VegetablesDistract : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // if it hits a crow directly
        if (collision.gameObject.CompareTag("Crow"))
        {
            DistractCrow(collision.gameObject);
        }

        // when it lands anywhere, distract nearest crow
        DistractNearestCrow();
    }

    void DistractNearestCrow()
    {
        GameObject[] crows = GameObject.FindGameObjectsWithTag("Crow");
        if (crows.Length == 0) return;

        GameObject nearest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject crow in crows)
        {
            float dist = Vector3.Distance(transform.position, crow.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = crow;
            }
        }

        if (nearest != null) DistractCrow(nearest);
    }

    void DistractCrow(GameObject crow)
    {
        // stop chasing Pike
        CrowPatrol patrol = crow.GetComponent<CrowPatrol>();
        if (patrol != null) patrol.enabled = false;

        // walk toward vegetable
        NavMeshAgent agent = crow.GetComponent<NavMeshAgent>();
        if (agent != null) agent.SetDestination(transform.position);
    }
}