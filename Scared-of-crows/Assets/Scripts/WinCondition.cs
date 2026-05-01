using UnityEngine;

public class WinCondition : MonoBehaviour
{
    public GameObject winCanvas;
    public CrowHealth[] crows;
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
        foreach (CrowHealth crow in crows)
        {
            if (!crow.IsDead) return;
        }

        winCanvas.SetActive(true);
    }
}