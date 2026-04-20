using UnityEngine;

public class CrowPatrol : MonoBehaviour
{
    [Header("Patrol Settings")]
    public float moveSpeed = 30f;
    public float patrolTime = 3f;

    public float detectionRadius = 80f;
    public float chaseSpeed = 40f;
    public float returnSpeed = 30f;

    private float timer;
    private Vector3 currentDirection;
    private Transform player;
    private bool isChasing = false;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private bool isReturning = false;
    public bool IsChasing => isChasing;
    public float DetectionRadius => detectionRadius;
    public Transform Player => player;

    // sound detection mechanic
    [Header("Sound Detection")]
    private float soundRadius = 150f;
    private BellNoise bellNoise;
    void Start()
    {
        currentDirection = transform.forward;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        startPosition = transform.position;
        startRotation = transform.rotation;
        bellNoise = player.GetComponent<BellNoise>();
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        bool inVisualRange = distanceToPlayer <= detectionRadius;
        bool inSoundRange = distanceToPlayer <= soundRadius && bellNoise != null && bellNoise.IsRinging;

        if (inVisualRange || inSoundRange)
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
            Chase();
        else if (isReturning)
            ReturnToStart();
        else
            Patrol();
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
    public void ResetToStart()
    {
        transform.position = startPosition;  // instant teleport, no path
        transform.rotation = startRotation;
        isChasing = false;
        isReturning = false;
        timer = 0;
        currentDirection = startRotation * Vector3.forward;
    }

    // bool CanSeePlayer()
    // {
    //     Vector3 directionToPlayer = player.position - transform.position;
    //     float distance = directionToPlayer.magnitude;

    //     RaycastHit hit;
    //     if (Physics.Raycast(transform.position, directionToPlayer.normalized, out hit, distance))
    //     {
    //         if (hit.transform.CompareTag("Player") || hit.transform.root.CompareTag("Player"))
    //             return true;
    //         return false;
    //     }
    //     return true; // if nothing blocks the ray, player is visible
    // }
}