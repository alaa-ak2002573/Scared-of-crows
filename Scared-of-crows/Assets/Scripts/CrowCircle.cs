using UnityEngine;

public class CrowCircle : MonoBehaviour
{
    [Header("Circle Settings")]
    private float radius = 30f;
    private float speed = 30f;
    private float height = 30f;

    private float angle = 0f;
    private Vector3 centerPoint;
    public float startAngle = 0f;
    public Transform digby;

    void Start()
    {
        angle = startAngle;
    }

    void Update()
    {
        if (digby != null)
            centerPoint = digby.position;

        angle += speed * Time.deltaTime;

        float x = centerPoint.x + Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
        float z = centerPoint.z + Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
        float y = height;

        transform.position = new Vector3(x, y, z);
        transform.LookAt(new Vector3(x + Mathf.Cos((angle + 90) * Mathf.Deg2Rad), y, z + Mathf.Sin((angle + 90) * Mathf.Deg2Rad)));
    }
}