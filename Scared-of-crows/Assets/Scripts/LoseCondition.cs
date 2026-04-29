using UnityEngine;

public class LoseCondition : MonoBehaviour
{
    public GameObject loseCanvas;
    private VegetableInventory vegetableInventory;

    void Start()
    {
        vegetableInventory = FindFirstObjectByType<VegetableInventory>();
    }

    void Update()
    {
        if (vegetableInventory == null) return;

        if (vegetableInventory.Count <= 0)
        {
            loseCanvas.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}