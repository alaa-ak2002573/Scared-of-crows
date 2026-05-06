using UnityEngine;
using TMPro;

public class Deliveryzone : MonoBehaviour
{
    private int vegetableCount = 0;
    private bool levelComplete = false;
    public int totalVegetables = 3;
    public TextMeshProUGUI vegCounterText;

    void Start()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        if (vegCounterText != null)
        {
            vegCounterText.text = "Carrots: " + vegetableCount + "/" + totalVegetables;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (levelComplete) return;

        if (other.CompareTag("Player"))
        {
            vegetablePickup[] allVegs = FindObjectsByType<vegetablePickup>(FindObjectsSortMode.None);

            foreach (vegetablePickup veg in allVegs)
            {
                if (veg.IsCarried())
                {
                    veg.Drop();
                    vegetableCount++;
                    UpdateUI();

                    // Debug.Log("Vegetables delivered: " + vegetableCount);

                    if (vegetableCount >= 3)
                    {
                        levelComplete = true;
                        GameManager.instance.ChangeState(GameManager.GameState.LevelComplete);
                    }

                    return;
                }
            }
        }
    }
}