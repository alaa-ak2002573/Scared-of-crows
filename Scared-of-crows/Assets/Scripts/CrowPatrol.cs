using UnityEngine;

public class CrowPatrol : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float patrolTime = 3f;

    private float timer;
    private Vector3 currentDirection;

    void Start()
    {
        currentDirection = transform.forward;
    }

    void Update()
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
}