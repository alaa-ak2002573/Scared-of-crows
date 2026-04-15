using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public enum GameState { Playing, Caught, LevelComplete, GameOver }
    public GameState currentState;

    [Header("Timer")]
    public float timeRemaining = 120f;
    public TextMeshProUGUI timerText;
    private bool timerRunning = false;

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
        timerRunning = true;
        Debug.Log("Game started! State: " + currentState);
    }

    void Update()
    {
        if (timerRunning && currentState == GameState.Playing)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerDisplay();
            }
            else
            {
                timeRemaining = 0;
                timerRunning = false;
                ChangeState(GameState.LevelComplete);
            }
        }
    }

    void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            int seconds = Mathf.CeilToInt(timeRemaining);
            timerText.text = "Night ends in: " + seconds + "s";
        }
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;
        Debug.Log("Game state changed to: " + currentState);
    }
}