using UnityEngine;

public class WinCondition : MonoBehaviour
{
    public GameObject winCanvas;
    public CrowHealth[] crows;
    private bool winTriggered = false;

    public bool AllCrowsDead
    {
        get
        {
            foreach (CrowHealth crow in crows)
            {
                if (!crow.IsDead) return false;
            }
            return true;
        }
    }

    void Update()
    {
        if (winTriggered) return;

        foreach (CrowHealth crow in crows)
        {
            if (!crow.IsDead) return;
        }

        winTriggered = true;

        if (GameManager.instance != null)
        {
            GameManager.instance.ChangeState(GameManager.GameState.LevelComplete);
        }
        else if (winCanvas != null)
        {
            winCanvas.SetActive(true);
        }
    }
}