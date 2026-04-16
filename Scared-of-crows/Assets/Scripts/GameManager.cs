using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public enum GameState { Playing, Caught, LevelComplete, GameOver }
    public GameState currentState;

    public GameObject winCanvas;
    public GameObject loseCanvas;

    public TextMeshProUGUI timerText;
    public float timeLimit = 120f;
    private float currentTime;

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
    }

    void Update()
    {
        if (currentState == GameState.Playing)
        {
            currentTime -= Time.deltaTime;
            if (timerText != null)
                timerText.text = "Night ends in: " + Mathf.CeilToInt(currentTime) + "s";
            if (currentTime <= 0)
                ChangeState(GameState.LevelComplete);
        }
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;
        if (newState == GameState.LevelComplete)
            winCanvas.SetActive(true);
        if (newState == GameState.GameOver)
            loseCanvas.SetActive(true);
    }
}