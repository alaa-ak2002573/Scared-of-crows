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

    [Header("Dialogue")]
    [TextArea(2, 5)]
    public string[] slides;

    [Header("Audio")]
    public AudioSource levelMusic;

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

        if (levelMusic != null)
            levelMusic.Play();
    }
}