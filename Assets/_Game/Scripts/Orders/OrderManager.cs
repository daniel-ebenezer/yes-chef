using UnityEngine;
using System.Collections.Generic;

public class OrderManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private IngredientDatabaseSO ingredientDatabase;
    [SerializeField] private CustomerWindow[] customerWindows;

    [Header("Events")]

    [SerializeField] private IntEventChannelSO scoreChangedChannel;
    [SerializeField] private IntEventChannelSO highScoreUpdatedChannel;

    [Header("Order Settings")]
    [SerializeField] private float respawnDelay = 5f;
    [SerializeField] private int maxActiveOrders = 4;

    List<Order> activeOrders = new List<Order>();
    int currentScore = 0;
    int highScore = 0;

    public int CurrentScore => currentScore;
    public int HighScore => highScore;

    Awaitable currentRespawnTask;

    void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        SpawnInitialOrders();
    }

    public void SpawnInitialOrders()
    {
        for (int i = 0; i < maxActiveOrders && i < customerWindows.Length; i++)
        {
            SpawnNewOrder(i);
        }
    }

    void SpawnNewOrder(int windowIndex)
    {
        List<IngredientSO> ingredients = GenerateRandomOrder();

        Order newOrder = new Order(ingredients, Time.time, windowIndex);
        activeOrders.Add(newOrder);
        customerWindows[windowIndex]?.AssignOrder(newOrder);
    }

    private List<IngredientSO> GenerateRandomOrder()
    {
        List<IngredientSO> ingredients = new List<IngredientSO>();
        int count = Random.value < 0.5f ? 2 : 3;

        for (int i = 0; i < count; i++)
        {
            IngredientSO randomIngredient = ingredientDatabase.GetRandomIngredient();
            ingredients.Add(randomIngredient);
        }

        return ingredients;
    }

    public void OnOrderCompleted(int windowIndex, int scoreAdded)
    {
        currentScore += scoreAdded;

        RemoveCompletedOrder(windowIndex);
        scoreChangedChannel?.Raise(currentScore);

        currentRespawnTask?.Cancel();
        currentRespawnTask = RespawnAfterDelayAsync();
    }

    private async Awaitable RespawnAfterDelayAsync()
    {
        try
        {
            await Awaitable.WaitForSecondsAsync(respawnDelay, destroyCancellationToken);
            RespawnOrder();
        }
        catch (System.OperationCanceledException)
        {
            
        }
    }

    void RemoveCompletedOrder(int windowIndex)
    {
        activeOrders.RemoveAll(order => order.windowIndex == windowIndex);
    }

    void RespawnOrder()
    {
        for (int i = 0; i < customerWindows.Length; i++)
        {
            if (customerWindows[i] != null && customerWindows[i].CurrentOrder == null)
            {
                SpawnNewOrder(i);
                return;
            }
        }
    }

    public void UpdateHighScore()
    {
        if (currentScore > highScore)
        {
            highScore = currentScore;
            highScoreUpdatedChannel?.Raise(highScore);
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }
    }

    void OnDestroy()
    {
        currentRespawnTask?.Cancel();
    }
}