using UnityEngine;

public class CrowPatrol : MonoBehaviour
{
    [Header("Patrol Settings")]
    private float moveSpeed = 30f;          // matches player walk
    private float patrolTime = 3f;

    [Header("Detection")]
    private float visualRadius = 90f;       // sight
    private float soundRadius = 140f;       // sprint hearing

    [Header("Chase Speeds")]
    private float chaseSpeed = 40f;
    private float returnSpeed = 45f;

    private float timer;
    private Vector3 currentDirection;
    private Transform player;

    private bool isChasing = false;
    private bool isReturning = false;

    private Vector3 startPosition;
    private Quaternion startRotation;

    private BellNoise bellNoise;

    [Header("Audio")]
    public AudioClip crowAlert;
    private AudioSource audioSource;

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
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Visual detection
        bool inVisualRange = distance <= visualRadius && CanSeePlayer();

        // Sound detection
        bool isSprinting = bellNoise != null && bellNoise.IsRinging && bellNoise.GetComponent<AudioSource>().volume > 0.6f;
        bool inSoundRange = distance <= soundRadius && isSprinting;

        // Decide state
        if (inVisualRange || inSoundRange)
        {
            if (!isChasing)
                audioSource.PlayOneShot(crowAlert);
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
            audioSource.PlayOneShot(crowAlert);
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

    bool CanSeePlayer()
    {
        Vector3 origin = transform.position + Vector3.up * 1.5f;
        Vector3 target = player.position + Vector3.up * 1.5f;

        Vector3 direction = (target - origin);
        float distance = direction.magnitude;

        RaycastHit[] hits = Physics.RaycastAll(origin, direction.normalized, distance);

        // Sort hits by distance
        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        foreach (RaycastHit hit in hits)
        {
            // Ignore self
            if (hit.transform == transform) continue;

            // ONLY hiding objects block vision
            if (hit.transform.CompareTag("UseToHide"))
                return false;
        }

        // If nothing blocks → player is visible
        return true;
    }
}
