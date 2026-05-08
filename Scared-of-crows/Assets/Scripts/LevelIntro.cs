using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class LevelIntro : MonoBehaviour
{
    [Header("UI")]
    public GameObject introCanvas;
    public TextMeshProUGUI dialogueText;
    public Button nextButton;
    public TextMeshProUGUI buttonText;

    [Header("Typewriter Settings")]
    public float typingSpeed = 0.05f;

    [Header("Typing Sound")]
    public AudioSource typingAudio;

    [Header("Audio")]
    public AudioSource levelMusic;
    public AudioSource finaleAudio;

    [Header("Dialogue")]
    [TextArea(2, 5)]
    public string[] slides;

    private int currentSlide = 0;
    private bool isTyping = false;
    private bool audioSequenceStarted = false;
    private bool levelMusicStopped = false;

    [Header("Other UI")]
    public GameObject detectionCanvas;
    public GameObject carrotHintUI;

    private static string lastPlayedScene = "";

    void Start()
    {
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        if (lastPlayedScene == currentScene)
        {
            introCanvas.SetActive(false);
            if (detectionCanvas != null)
                detectionCanvas.SetActive(true);
            return;
        }

        lastPlayedScene = currentScene;

        if (detectionCanvas != null)
            detectionCanvas.SetActive(false);

        if (levelMusic != null && levelMusic.isPlaying)
            levelMusic.Stop();

        if (finaleAudio != null && finaleAudio.isPlaying)
            finaleAudio.Stop();

        Time.timeScale = 0f;
        introCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        nextButton.onClick.AddListener(OnNextClicked);
        StartCoroutine(TypeText(slides[0]));
    }

    void Update()
    {
        if (introCanvas.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (!levelMusicStopped && GameManager.instance != null)
        {
            if (GameManager.instance.currentState == GameManager.GameState.LevelComplete || GameManager.instance.currentState == GameManager.GameState.GameOver)
            {
                if (levelMusic != null && levelMusic.isPlaying)
                    levelMusic.Stop();

                levelMusicStopped = true;
            }
        }
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueText.text = "";
        buttonText.text = "Skip";

        if (typingAudio != null)
        {
            typingAudio.Stop();
            typingAudio.Play();
        }

        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }

        if (typingAudio != null)
            typingAudio.Stop();

        isTyping = false;
        buttonText.text = (currentSlide == slides.Length - 1) ? "Start" : "Next";
    }

    void OnNextClicked()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.text = slides[currentSlide];
            isTyping = false;

            if (typingAudio != null)
                typingAudio.Stop();

            buttonText.text = (currentSlide == slides.Length - 1) ? "Start" : "Next";
            return;
        }

        currentSlide++;
        if (currentSlide >= slides.Length)
        {
            introCanvas.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
            if (detectionCanvas != null)
                detectionCanvas.SetActive(true);
            if (carrotHintUI != null)
                StartCoroutine(ShowCarrotHint());
            if (!audioSequenceStarted)
            {
                audioSequenceStarted = true;
                StartCoroutine(PlayAudioSequence());
            }
        }
        else
        {
            StartCoroutine(TypeText(slides[currentSlide]));
        }
    }

    IEnumerator PlayAudioSequence()
    {
        if (finaleAudio != null)
        {
            finaleAudio.Play();
            if (finaleAudio.clip != null)
                yield return new WaitForSeconds(finaleAudio.clip.length);
        }

        if (levelMusic != null)
            levelMusic.Play();
    }
    IEnumerator ShowCarrotHint()
    {
        carrotHintUI.SetActive(true);
        yield return new WaitForSeconds(3f);
        carrotHintUI.SetActive(false);
    }
}