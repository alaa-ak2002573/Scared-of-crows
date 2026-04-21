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

    [SerializeField] private float safeDisplayDuration = 2f;
    private float safeTimer = 0f;
    private bool wasBeingChased = false;

    private void Start()
    {
        if (starterAssetsInputs == null)
            starterAssetsInputs = FindFirstObjectByType<StarterAssetsInputs>();
    }

    public void SetHidden(bool value)
    {
        isHidden = value;
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

        if (wasBeingChased && !anyCrowChasing)
        {
            safeTimer = safeDisplayDuration;
        }

        wasBeingChased = anyCrowChasing;

        if (safeTimer > 0f)
        {
            safeTimer -= Time.deltaTime;
        }

        if (anyCrowChasing)
        {
            detectionText.text = "DETECTED!";
            detectionText.color = Color.red;
        }
        else if (isHidden)
        {
            detectionText.text = "Hidden";
            detectionText.color = Color.green;
        }
        else if (safeTimer > 0f)
        {
            detectionText.text = "Escaped";
            detectionText.color = Color.cyan;
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