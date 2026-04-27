using UnityEngine;
using System.Collections;

public class CrowHealth : MonoBehaviour
{
    [Header("Death Settings")]

    public float fallSpeed = 5f;

    private bool isDead = false;
    private CrowPatrol crowPatrol;
    private CrowCatch crowCatch;
    public bool IsDead => isDead;

    void Start()
    {
        crowPatrol = GetComponent<CrowPatrol>();
        crowCatch = GetComponent<CrowCatch>();
    }

    void Update()
    {
        if (isDead)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                new Vector3(transform.position.x, 0f, transform.position.z),
                fallSpeed * Time.deltaTime
            );
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("HIT BY: " + other.gameObject.name + " | Tag: " + other.tag + " | isDead: " + isDead);

        if (other.tag.Trim() == "Vegetable" && !isDead)
        {
            Debug.Log("CALLING DIE!");
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        if (crowPatrol != null)
            crowPatrol.enabled = false;

        if (crowCatch != null)
            crowCatch.enabled = false;

        GetComponent<BoxCollider>().enabled = false;

        // Fly away
        StartCoroutine(FlyAway());
    }

    IEnumerator FlyAway()
    {
        Debug.Log("FlyAway started!");
        float duration = 2f;
        float elapsed = 0f;
        Vector3 flyDirection = (transform.forward + Vector3.up * 2f).normalized;

        while (elapsed < duration)
        {
            transform.position += flyDirection * 50f * Time.deltaTime;
            elapsed += Time.deltaTime;
            yield return null;
        }

        Debug.Log("FlyAway done!");
        gameObject.SetActive(false);
    }
}