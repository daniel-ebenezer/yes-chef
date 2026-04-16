using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject startScreenPanel;
    [SerializeField] private GameObject gameplayPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject pausePanel;           

    [Header("Gameplay UI Elements")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI interactionPromptText;

    [Header("Game Over UI Elements")]
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI finalHighScoreText;

    [Header("Events")]
    [SerializeField] private IntEventChannelSO scoreChangedChannel;
    [SerializeField] private IntEventChannelSO highScoreUpdatedChannel;
    [SerializeField] private EventChannelSO<string> interactionPromptChannel;

    void Start()
    {
        ShowStartScreen();
    }

    void OnEnable()
    {
        if (scoreChangedChannel != null)
            scoreChangedChannel.OnRaised.AddListener(UpdateScoreUI);

        if (highScoreUpdatedChannel != null)
            highScoreUpdatedChannel.OnRaised.AddListener(UpdateHighScoreUI);
        if (interactionPromptChannel != null)
            interactionPromptChannel.OnRaised.AddListener(UpdateInteractionPrompt);
    }

    void OnDisable()
    {
        if (scoreChangedChannel != null)
            scoreChangedChannel.OnRaised.RemoveListener(UpdateScoreUI);

        if (highScoreUpdatedChannel != null)
            highScoreUpdatedChannel.OnRaised.RemoveListener(UpdateHighScoreUI);
        if (interactionPromptChannel != null)
            interactionPromptChannel.OnRaised.RemoveListener(UpdateInteractionPrompt);
    }

    public void ShowStartScreen()
    {
        startScreenPanel.SetActive(true);
        gameplayPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        pausePanel.SetActive(false);
    }

    public void ShowGameplayUI()
    {
        startScreenPanel.SetActive(false);
        gameplayPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        pausePanel.SetActive(false);
    }

    public void ShowPauseMenu()
    {
        pausePanel.SetActive(true);
    }

    public void ShowGameOverScreen(int finalScore, int highScore)
    {
        gameplayPanel.SetActive(false);
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(true);

        finalScoreText.text = $"Final Score: {finalScore}";
        finalHighScoreText.text = highScore >= finalScore 
            ? $"High Score: {highScore}" 
            : $"New High Score! {finalScore}";
    }

    public void UpdateGameTimer(float timeRemaining)
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    void UpdateScoreUI(int newScore)
    {
        scoreText.text = $"Score: {newScore}";
    }

    void UpdateHighScoreUI(int newHighScore)
    {
        highScoreText.text = $"High Score: {newHighScore}";
    }

    void UpdateInteractionPrompt(string promptText)
    {
        interactionPromptText.text = promptText;
        interactionPromptText.gameObject.SetActive(!string.IsNullOrEmpty(promptText));
        
    }

    
}