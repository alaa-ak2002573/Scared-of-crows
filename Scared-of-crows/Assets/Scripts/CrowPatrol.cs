using UnityEngine;

public class CrowPatrol : MonoBehaviour
{
    [Header("Patrol Settings")]
    public float moveSpeed = 45f;          // matches player walk
    public float patrolTime = 3f;

    [Header("Detection")]
    public float visualRadius = 90f;       // sight
    public float soundRadius = 140f;       // sprint hearing

    [Header("Chase Speeds")]
    public float chaseSpeed = 60f;     
    public float returnSpeed = 45f;

    private float timer;
    private Vector3 currentDirection;
    private Transform player;

    private bool isChasing = false;
    private bool isReturning = false;

    private Vector3 startPosition;
    private Quaternion startRotation;

    private BellNoise bellNoise;

    public bool IsChasing => isChasing;
    public Transform Player => player;
    public float DetectionRadius => visualRadius;

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

        float distance = Vector3.Distance(transform.position, player.position);

        // Visual detection
        bool inVisualRange = distance <= visualRadius;

        // Sound detection
        bool isSprinting = bellNoise != null && bellNoise.IsRinging && bellNoise.GetComponent<AudioSource>().volume > 0.6f;
        bool inSoundRange = distance <= soundRadius && isSprinting;

        // Decide state
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

        // State behavior
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
        Vector3 target = new Vector3(player.position.x, transform.position.y, player.position.z);

        transform.LookAt(target);
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
        // visual radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, visualRadius);

        // sound radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, soundRadius);
    }

    public void ResetToStart()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;

        isChasing = false;
        isReturning = false;

        timer = 0;
        currentDirection = startRotation * Vector3.forward;
    }
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