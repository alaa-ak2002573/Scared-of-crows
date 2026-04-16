using UnityEngine;
using TMPro;
using StarterAssets;

public class DetectionUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI detectionText;
    [SerializeField] private BellNoise bellNoise;
    [SerializeField] private CrowPatrol[] crows;
    [SerializeField] private StarterAssetsInputs starterAssetsInputs;

    public bool isHidden = false;

    private void Start()
    {
        if (starterAssetsInputs == null)
            starterAssetsInputs = FindFirstObjectByType<StarterAssetsInputs>();
    }

    private void Update()
    {
        bool anyCrowChasing = false;
        bool nearCrow = false;
        bool isMoving = starterAssetsInputs != null && starterAssetsInputs.move.sqrMagnitude > 0.01f;
        bool isRunning = starterAssetsInputs != null && starterAssetsInputs.sprint && isMoving;

        foreach (CrowPatrol crow in crows)
        {
            if (crow == null || crow.Player == null) continue;

            if (crow.IsChasing)
            {
                anyCrowChasing = true;
                break;
            }

            float distance = Vector3.Distance(crow.transform.position, crow.Player.position);
            if (distance <= crow.DetectionRadius)
            {
                nearCrow = true;
            }
        }

        if (anyCrowChasing)
        {
            detectionText.text = "DETECTED!";
            detectionText.color = Color.red;
        }
        else if (isHidden || !isMoving)
        {
            detectionText.text = "Hidden";
            detectionText.color = Color.green;
        }
        else if (isRunning || nearCrow)
        {
            detectionText.text = "Suspicious";
            detectionText.color = Color.yellow;
        }
        else
        {
            detectionText.text = "";
        }
    }
}