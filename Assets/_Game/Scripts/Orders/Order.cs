using UnityEngine;
using System.Collections.Generic;

public class Order
{
    public List<IngredientSO> requiredIngredients { get; private set; }
    public float spawnTime { get; private set; }
    public int windowIndex { get; set; }

    int totalBaseScore;

    public Order(List<IngredientSO> _ingredients, float _spawnTime, int _windowIndex)
    {
        requiredIngredients = new List<IngredientSO>(_ingredients);
        spawnTime = _spawnTime;
        windowIndex = _windowIndex;
        CalculateTotalScore();
    }

    void CalculateTotalScore()
    {
        totalBaseScore = 0;
        foreach (var ingredient in requiredIngredients)
            totalBaseScore += ingredient.baseScore;
    }

    public bool TryDeliverIngredient(Ingredient deliveredIngredient)
    {
        if (deliveredIngredient == null || !deliveredIngredient.IsPrepared)
            return false;

        return requiredIngredients.Remove(deliveredIngredient.data);
    }

    public bool IsComplete => requiredIngredients.Count == 0;

    public float TimeSinceSpawned => Time.time - spawnTime;

    public int CalculateScore()
    {
        return totalBaseScore - Mathf.FloorToInt(TimeSinceSpawned);
    }

    public List<IngredientSO> GetRemainingIngredients() => new List<IngredientSO>(requiredIngredients);
}