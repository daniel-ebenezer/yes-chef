using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private OrderManager orderManager;
    [SerializeField] private UIManager uiManager;

    [SerializeField] private BoolEventChannelSO gamePausedEvent;

    [Header("Game Settings")]
    [SerializeField] private float gameDuration = 180f; 



    float gameTimer;
    bool isGameRunning = false;
    bool isPaused = false;

    public bool IsGameRunning => isGameRunning && !isPaused;
    public bool IsPaused => isPaused;

    public void StartGame()
    {
        gameTimer = gameDuration;
        isGameRunning = true;
        isPaused = false;

        uiManager.ShowGameplayUI();
        orderManager.gameObject.SetActive(true);
    }

    void Update()
    {
        if (!isGameRunning || isPaused) return;

        gameTimer -= Time.deltaTime;
        uiManager.UpdateGameTimer(gameTimer);

        if (gameTimer <= 0f)
        {
            EndGame();
        }
    }

    public void TogglePause()
    {
        if (!isGameRunning) return;

        isPaused = !isPaused;
        gamePausedEvent?.Raise(isPaused);

        if (isPaused)
        {
            Time.timeScale = 0f;
            uiManager.ShowPauseMenu();
        }
        else
        {
            Time.timeScale = 1f;
            uiManager.ShowGameplayUI();
        }
    }

    void EndGame()
    {
        isGameRunning = false;
        isPaused = false;
        Time.timeScale = 1f;

        orderManager.UpdateHighScore();
        uiManager.ShowGameOverScreen(orderManager.CurrentScore, orderManager.HighScore);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}   