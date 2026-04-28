using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public enum GameState { Playing, Caught, LevelComplete, GameOver }
    public GameState currentState;

    public GameObject winCanvas;
    public GameObject loseCanvas;
    public GameObject timerCanvas;

    public bool timerWins = true;

    public TextMeshProUGUI timerText;
    public float timeLimit = 120f;
    private float currentTime;

    public AudioSource victorySound;
    public AudioSource lossSound;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        currentState = GameState.Playing;
        currentTime = timeLimit;
        winCanvas.SetActive(false);
        loseCanvas.SetActive(false);
        timerCanvas.SetActive(true);
        FadeManager.instance.FadeFromBlack();
    }

    void Update()
    {
        if (currentState == GameState.Playing)
        {
            currentTime -= Time.deltaTime;
            if (timerText != null)
                timerText.text = "Night ends in: " + Mathf.CeilToInt(currentTime) + "s";
            if (currentTime <= 0)
            {
                if (timerWins)
                    ChangeState(GameState.LevelComplete);
                else
                    ChangeState(GameState.GameOver);
            }
        }

        if (currentState == GameState.GameOver)
        {
            if (Keyboard.current.rKey.wasPressedThisFrame)
                StartCoroutine(FadeAndRestart());
        }

        if (currentState == GameState.LevelComplete)
        {
            if (Keyboard.current.enterKey.wasPressedThisFrame)
                StartCoroutine(FadeAndNextLevel());
        }
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;

        if (newState == GameState.LevelComplete)
        {
            winCanvas.SetActive(true);
            timerCanvas.SetActive(false);
            if (victorySound != null)
                victorySound.Play();
        }

        if (newState == GameState.GameOver)
        {
            loseCanvas.SetActive(true);
            timerCanvas.SetActive(false);
            FadeManager.instance.FadeToBlack();
            if (lossSound != null)
                lossSound.Play();
        }
    }

    IEnumerator FadeAndRestart()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator FadeAndNextLevel()
    {
        FadeManager.instance.FadeToBlack();
        yield return new WaitForSeconds(1f);
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextScene < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(nextScene);
        else
            SceneManager.LoadScene(0);
    }
}