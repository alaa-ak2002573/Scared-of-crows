using UnityEngine;

public class CrowPatrol : MonoBehaviour
{
    [Header("Patrol Settings")]
    public float moveSpeed = 2f;
    public float patrolTime = 3f;

    private float detectionRadius = 1.5f;
    private float chaseSpeed = 4f;

    private float timer;
    private Vector3 currentDirection;
    private Transform player;
    private bool isChasing = false;

    void Start()
    {
        currentDirection = transform.forward;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius)
        {
            isChasing = true;
        }
        else
        {
            isChasing = false;
        }

        if (isChasing)
        {
            Chase();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        timer += Time.deltaTime;
        transform.Translate(currentDirection * moveSpeed * Time.deltaTime, Space.World);

        if (timer >= patrolTime)
        {
            currentDirection = -currentDirection;
            transform.Rotate(0, 180, 0);
            timer = 0;
        }
    }

    void Chase()
    {
    Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
    transform.LookAt(targetPosition);
    transform.Translate(Vector3.forward * chaseSpeed * Time.deltaTime);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}