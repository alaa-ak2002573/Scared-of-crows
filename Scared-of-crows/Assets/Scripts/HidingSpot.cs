using UnityEngine;

public class HidingSpot : MonoBehaviour
{
    private DetectionUI detectionUI;

    private void Start()
    {
        detectionUI = FindFirstObjectByType<DetectionUI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            detectionUI.SetHidden(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            detectionUI.SetHidden(false);
        }
    }
}