using UnityEngine;
using TMPro;
using System.Collections;

public class LevelIntro : MonoBehaviour
{
    [Header("UI")]
    public GameObject introCanvas;
    public TextMeshProUGUI dialogueText;

    [Header("Timing")]
    public float typingSpeed = 0.05f;
    public float displayDuration = 3f;

    private string[] slides = new string[]
    {
        "Digby: Pike. Get up. The farm needs guarding ... now.",
        "Digby: You see those crows? Chase them off. That's literally your only job.",
        "Digby: And keep it down! Crows hear everything. Don't go stomping around.",
        "Digby: I'm going to sleep. Don't mess this up."
    };

    void Start()
    {
        Time.timeScale = 0f;
        introCanvas.SetActive(true);
        StartCoroutine(PlayDialogue());
    }

    IEnumerator PlayDialogue()
    {
        foreach (string slide in slides)
        {
            dialogueText.text = "";

            foreach (char c in slide)
            {
                dialogueText.text += c;
                yield return new WaitForSecondsRealtime(typingSpeed);
            }

            yield return new WaitForSecondsRealtime(displayDuration);
        }

        introCanvas.SetActive(false);
        Time.timeScale = 1f;
    }
}