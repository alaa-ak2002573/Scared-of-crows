using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class StoryManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI storyText;
    public Button nextButton;
    public TextMeshProUGUI buttonText;

    [Header("Typewriter Settings")]
    public float typingSpeed = 0.05f;

    [Header("Typing Sound")]
    public AudioSource typingAudio;

    private int currentSlide = 0;
    private bool isTyping = false;

    private string[] slides = new string[]
    {
        "In a quiet farmland, far from the noise of the outside world, lived a hardworking rabbit named Digby.\n\nThe farm was his pride and only source of survival.",
        "But recently, a growing flock of crows began invading his fields, eating crops and destroying everything he worked for.\n\nDay after day, Digby tried to scare them away ... but nothing worked.",
        "Desperate and exhausted, Digby set out in search of a solution.\n\nOne evening, he encountered a strange and suspicious animal who offered him a peculiar blueprint ... a design for a scarecrow unlike any other.",
        "Back on the farm, Digby carefully followed the blueprint and built the scarecrow.\n\nHe named it Pike.",
        "At first, Pike was nothing more than wood, straw, and cloth… standing silently in the field.\n\nBut something unusual happened.\n\nPike came to life.",
        "Confused and afraid, Pike became aware of his surroundings ... the wind, the crops, and most importantly, the crows.\n\nIronically, the very creatures he was created to frighten became the source of his fear.",
        "From this moment, you will take control of Pike.\n\nNavigate the farm, avoid crow patrols, and survive using stealth, hiding, and careful movement.",
        "Are the crows truly enemies… or something more?\n\n"
    };

    void Start()
    {
        nextButton.onClick.AddListener(OnNextClicked);
        StartCoroutine(TypeText(slides[0]));
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;
        storyText.text = "";
        buttonText.text = "Skip";

        if (typingAudio != null)
        {
            typingAudio.Stop();
            typingAudio.Play();
        }

        foreach (char c in text)
        {
            storyText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        if (typingAudio != null)
            typingAudio.Stop();

        isTyping = false;
        buttonText.text = (currentSlide == slides.Length - 1) ? "Start Game" : "Next";
    }

    void OnNextClicked()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            storyText.text = slides[currentSlide];
            isTyping = false;

            if (typingAudio != null)
                typingAudio.Stop();

            buttonText.text = (currentSlide == slides.Length - 1) ? "Start Game" : "Next";
            return;
        }

        currentSlide++;
        if (currentSlide >= slides.Length)
            SceneManager.LoadScene("Level1");
        else
            StartCoroutine(TypeText(slides[currentSlide]));
    }
}