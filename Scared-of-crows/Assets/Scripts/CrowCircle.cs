using UnityEngine;

public class CrowCircle : MonoBehaviour
{
    [Header("Circle Settings")]
    private float radius = 30f;
    private float speed = 20f;
    private float height = 20f;

    private float angle = 0f;
    private Vector3 centerPoint;
    public float startAngle = 0f;

    void Start()
    {
        centerPoint = transform.position;
        angle = startAngle;
    }

    void Update()
    {
        angle += speed * Time.deltaTime;

        float x = centerPoint.x + Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
        float z = centerPoint.z + Mathf.Sin(angle * Mathf.Deg2Rad) * radius;

        transform.position = new Vector3(x, height, z);
        transform.LookAt(new Vector3(x + Mathf.Cos((angle + 90) * Mathf.Deg2Rad), height, z + Mathf.Sin((angle + 90) * Mathf.Deg2Rad)));
    }
}