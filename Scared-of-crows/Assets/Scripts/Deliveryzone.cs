using UnityEngine;
using TMPro;
using System.Collections;

public class Deliveryzone : MonoBehaviour
{
    private int vegetableCount = 0;
    private bool levelComplete = false;
    public int totalVegetables = 3;
    public TextMeshProUGUI vegCounterText;
    public GameObject digby;

    void Start()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        if (vegCounterText != null)
            vegCounterText.text = "Carrots: " + vegetableCount + "/" + totalVegetables;
    }

    void OnTriggerEnter(Collider other)
    {
        if (levelComplete) return;

        if (other.gameObject.tag.Trim() == "Player")
        {
            vegetablePickup[] allVegs = FindObjectsByType<vegetablePickup>(FindObjectsSortMode.None);

            foreach (vegetablePickup veg in allVegs)
            {
                if (veg.IsCarried())
                {
                    veg.Drop();
                    vegetableCount++;
                    UpdateUI();

                    if (vegetableCount >= totalVegetables)
                    {
                        levelComplete = true;
                        StartCoroutine(LevelCompleteSequence());
                    }

                    return;
                }
            }
        }
    }

    IEnumerator LevelCompleteSequence()
    {
        if (digby != null)
            digby.SetActive(false);

        yield return new WaitForSeconds(1f);

        GameManager.instance.ChangeState(GameManager.GameState.LevelComplete);
    }
}