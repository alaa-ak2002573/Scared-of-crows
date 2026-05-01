using UnityEngine;
using System.Collections;

public class LoseCondition : MonoBehaviour
{
    public GameObject loseCanvas;
    public float checkDelay = 3f;
    private VegetableInventory vegetableInventory;
    private WinCondition winCondition;
    private bool checking = false;

    void Start()
    {
        vegetableInventory = FindFirstObjectByType<VegetableInventory>();
        winCondition = FindFirstObjectByType<WinCondition>();
    }

    void Update()
    {
        if (vegetableInventory == null || winCondition == null) return;
        if (checking) return;

        if (vegetableInventory.Count <= 0 && !winCondition.AllCrowsDead)
        {
            StartCoroutine(CheckAfterDelay());
        }
    }

    IEnumerator CheckAfterDelay()
    {
        checking = true;
        yield return new WaitForSeconds(checkDelay);

        if (!winCondition.AllCrowsDead)
        {
            loseCanvas.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}