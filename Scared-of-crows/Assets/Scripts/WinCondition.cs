using UnityEngine;

public class WinCondition : MonoBehaviour
{
    public GameObject winCanvas;
    public CrowHealth[] crows;

    void Update()
    {
        foreach (CrowHealth crow in crows)
        {
            if (!crow.IsDead) return;
        }

        winCanvas.SetActive(true);
        Time.timeScale = 0f;
    }
}