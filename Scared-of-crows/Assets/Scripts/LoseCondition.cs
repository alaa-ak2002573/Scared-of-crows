using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoseCondition : MonoBehaviour
{
    public GameObject loseCanvas;
    public float checkDelay = 3f;
    private VegetableInventory vegetableInventory;
    private WinCondition winCondition;
    private bool checking = false;
    private bool hasLost = false;

    void Start()
    {
        vegetableInventory = FindFirstObjectByType<VegetableInventory>();
        winCondition = FindFirstObjectByType<WinCondition>();
    }

    void Update()
    {
        if (vegetableInventory == null || winCondition == null) return;
        
        if (hasLost && Keyboard.current.rKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return;
        }
        
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
            GameManager.instance.ChangeState(GameManager.GameState.GameOver);
            hasLost = true;
        }
    }
}