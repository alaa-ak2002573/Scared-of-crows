using UnityEngine;

public class CrowPatrol : MonoBehaviour
{
    [Header("Patrol Settings")]
    public float moveSpeed = 2f;
    public float patrolTime = 3f;

    private float detectionRadius = 2f;
    private float chaseSpeed = 4f;
    private float returnSpeed = 3f;

    private float timer;
    private Vector3 currentDirection;
    private Transform player;
    private bool isChasing = false;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private bool isReturning = false;

    void Start()
    {
        currentDirection = transform.forward;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius)
        {
            isChasing = true;
            isReturning = false;
        }
        else if (isChasing)
        {
            isChasing = false;
            isReturning = true;
        }

        if (isChasing)
        {
            Chase();
        }
        else if (isReturning)
        {
            ReturnToStart();
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

    void ReturnToStart()
    {
        transform.position = Vector3.MoveTowards(transform.position, startPosition, returnSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, startRotation, Time.deltaTime * returnSpeed);

        if (Vector3.Distance(transform.position, startPosition) < 0.1f)
        {
            isReturning = false;
            timer = 0;
            currentDirection = transform.forward;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}